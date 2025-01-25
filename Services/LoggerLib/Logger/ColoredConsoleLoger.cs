//-----------------------------------------------------------------------
// <copyright file="ColoredConsoleLoger.cs" company="FH Wiener Neustadt">
// Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Ralf Kuehmayer</author>
//-----------------------------------------------------------------------

namespace LoggerLib.Logger;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Runtime.Versioning;

// https://learn.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider

/// <summary>
/// A class to replace the dependency injected logger.
/// </summary>
public sealed class ColorConsoleLogger : ILogger
{
    private readonly Func<ColorConsoleLoggerConfiguration> getCurrentConfig;
    private readonly string name;
    private readonly LoggerLockObject lockObject;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorConsoleLogger"/> class.
    /// </summary>
    /// <param name="name">The name of the logger.</param>
    /// <param name="getCurrentConfig">The configuration of the logger.</param>
    public ColorConsoleLogger(string name, Func<ColorConsoleLoggerConfiguration> getCurrentConfig, LoggerLockObject lockObject)
    {
        this.name=name;
        this.getCurrentConfig=getCurrentConfig;
        this.lockObject=lockObject;
    }

    /// <inheritdoc/>
    public IDisposable? BeginScope<TState>(TState state)
        where TState : notnull
    {
        return default!;
    }

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel)
    {
        return this.getCurrentConfig().Use&&this.getCurrentConfig().LogLevelToColorMap.ContainsKey(logLevel);
    }

    /// <inheritdoc/>
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!this.IsEnabled(logLevel))
        {
            return;
        }

        ColorConsoleLoggerConfiguration config = this.getCurrentConfig();
        if (config.EventId==0||config.EventId==eventId.Id)
        {
            lock (this.lockObject)
            {
                ConsoleColor originalColor = Console.ForegroundColor;

                Console.ForegroundColor=config.LogLevelToColorMap[logLevel];
                Console.WriteLine($"[{eventId.Id,2}: {logLevel,-12} - {this.name}]");

                Console.ForegroundColor=originalColor;
                Console.WriteLine($"{formatter(state, exception)}");
            }
        }
    }
}

/// <summary>
/// A class to provide the logger for dependency injection.
/// </summary>
[UnsupportedOSPlatform("browser")]
[ProviderAlias("ColorConsole")]
public sealed class ColorConsoleLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? onChangeToken;
    private readonly LoggerLockObject lockObject;
    private readonly ConcurrentDictionary<string, ColorConsoleLogger> loggers =
        new(StringComparer.OrdinalIgnoreCase);

    private ColorConsoleLoggerConfiguration currentConfig;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorConsoleLoggerProvider"/> class.
    /// </summary>
    /// <param name="config">The configuration of the logger.</param>
    public ColorConsoleLoggerProvider(
        IOptionsMonitor<ColorConsoleLoggerConfiguration> config, LoggerLockObject lockObject)
    {
        this.currentConfig=config.CurrentValue;
        this.onChangeToken=config.OnChange(updatedConfig => this.currentConfig=updatedConfig);
        this.lockObject = lockObject;
    }

    /// <inheritdoc>/>.
    public ILogger CreateLogger(string categoryName)
    {
        return this.loggers.GetOrAdd(categoryName, name => new ColorConsoleLogger(name, this.GetCurrentConfig, lockObject));
    }

    /// <inheritdoc>/>.
    public void Dispose()
    {
        this.loggers.Clear();
        this.onChangeToken?.Dispose();
    }

    private ColorConsoleLoggerConfiguration GetCurrentConfig()
    {
        return this.currentConfig;
    }
}

/// <summary>
/// A static class the extend the logger builder.
/// </summary>
public static class ColorConsoleLoggerExtensions
{
    /// <summary>
    /// Adds a the console color logger to the builder.
    /// </summary>
    /// <param name="builder">The builder to extend.</param>
    /// <returns>The builder with the logger.</returns>
    public static ILoggingBuilder AddColorConsoleLogger(
        this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, ColorConsoleLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions
            <ColorConsoleLoggerConfiguration, ColorConsoleLoggerProvider>(builder.Services);

        return builder;
    }

    /// <summary>
    /// Adds a the console color logger with a configuration to the builder.
    /// </summary>
    /// <param name="builder">The builder to extend.</param>
    /// <param name="configure">The configuration for the logger.</param>
    /// <returns>The builder with the logger.</returns>
    public static ILoggingBuilder AddColorConsoleLogger(
        this ILoggingBuilder builder,
        Action<ColorConsoleLoggerConfiguration> configure)
    {
        builder.AddColorConsoleLogger();
        builder.Services.Configure(configure);

        return builder;
    }
}