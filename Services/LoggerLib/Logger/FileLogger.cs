//-----------------------------------------------------------------------
// <copyright file="FileLogger.cs" company="FH Wiener Neustadt">
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

// from https://learn.microsoft.com/en-us/answers/questions/1377949/logging-in-c-to-a-text-file

// Customized ILoggerProvider, writes logs to text files

/// <summary>
/// A class to provid the logger for dependency injection.
/// </summary>
[UnsupportedOSPlatform("browser")]
[ProviderAlias("FileLogger")]
public class CustomFileLoggerProvider : ILoggerProvider
{
    private readonly IDisposable? onChangeToken;
    private readonly StreamWriter fileStream;
    private readonly ConcurrentDictionary<string, CustomFileLogger> loggers = new(StringComparer.OrdinalIgnoreCase);
    private FileLoggerOptions currentConfig;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomFileLoggerProvider"/> class.
    /// </summary>
    /// <param name="config">The monitor to detect changes in the config.</param>
    /// <param name="fileOpenerOptions">The configuration of the logger.</param>
    public CustomFileLoggerProvider(IOptionsMonitor<FileLoggerOptions> config, IOptions<FileOpenerOptions> fileOpenerOptions)
    {
        this.currentConfig=config.CurrentValue;
        this.onChangeToken=config.OnChange(updatedConfig => this.currentConfig=updatedConfig);

        FileOpener fileOpener = new FileOpener(fileOpenerOptions, null);

        if (this.currentConfig.Path!=null)
        {
            Task<FileStream> fileStreamTask = fileOpener.OpenWrite(this.currentConfig.Path, this.currentConfig.Append ? FileMode.Append : FileMode.Create);
            fileStreamTask.Wait();

            FileStream fileStream = fileStreamTask.Result;

            if (fileStream==null)
            {
                throw new ArgumentException(nameof(config));
            }

            this.fileStream=new StreamWriter(fileStream);
        }
        else
        {
            this.fileStream=new StreamWriter(new MemoryStream());
        }
    }

    /// <inheritdoc/>
    public ILogger CreateLogger(string categoryName)
    {
        return this.loggers.GetOrAdd(categoryName, name => new CustomFileLogger(categoryName, this.GetCurrentConfig, this.fileStream));
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        this.loggers.Clear();
        this.onChangeToken?.Dispose();
        this.fileStream?.Dispose();
    }

    private FileLoggerOptions GetCurrentConfig()
    {
        return this.currentConfig;
    }
}

// Customized ILogger, writes logs to text files

/// <summary>
/// A class to replace the dependency injected logger.
/// </summary>
public class CustomFileLogger : ILogger
{
    private readonly string categoryName;
    private readonly StreamWriter logFileWriter;
    private readonly Func<FileLoggerOptions> getCurrentConfig;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomFileLogger"/> class.
    /// </summary>
    /// <param name="categoryName">The name of the logger.</param>
    /// <param name="getCurrentConfig">The configuration of the logger.</param>
    /// <param name="fileStream">The file stream to write to.</param>
    public CustomFileLogger(string categoryName, Func<FileLoggerOptions> getCurrentConfig, StreamWriter fileStream)
    {
        this.categoryName=categoryName;
        this.logFileWriter=fileStream;
        this.getCurrentConfig=getCurrentConfig;
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
        return this.getCurrentConfig().Use;
    }

    /// <inheritdoc/>
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        // Ensure that only information level and higher logs are recorded
        if (!this.IsEnabled(logLevel))
        {
            return;
        }

        string message = formatter(state, exception);

        // Write log messages to text file
        this.logFileWriter.WriteLine($"[{logLevel}] [{this.categoryName}] [{DateTime.Now:yyyy-MM-dd'T'hh:mm:ss.fff zzz}] {message}");
        this.logFileWriter.Flush();
    }
}

/// <summary>
/// A static class the extend the logger builder.
/// </summary>
public static class FileLoggerExtensions
{
    /// <summary>
    /// Adds a the file color logger to the builder.
    /// </summary>
    /// <param name="builder">The builder to extend.</param>
    /// <returns>The builder with the logger.</returns>
    public static ILoggingBuilder AddFileLogger(
        this ILoggingBuilder builder)
    {
        builder.AddConfiguration();

        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, CustomFileLoggerProvider>());

        LoggerProviderOptions.RegisterProviderOptions
            <FileLoggerOptions, CustomFileLoggerProvider>(builder.Services);

        return builder;
    }

    /// <summary>
    /// Adds a the console file logger with a configuration to the builder.
    /// </summary>
    /// <param name="builder">The builder to extend.</param>
    /// <param name="configure">The configuration for the logger.</param>
    /// <returns>The builder with the logger.</returns>
    public static ILoggingBuilder AddFileLogger(
        this ILoggingBuilder builder,
        Action<FileLoggerOptions> configure)
    {
        builder.AddFileLogger();
        builder.Services.Configure(configure);

        return builder;
    }
}