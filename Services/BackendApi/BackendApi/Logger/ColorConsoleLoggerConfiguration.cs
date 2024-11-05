//-----------------------------------------------------------------------
// <copyright file="ColorConsoleLoggerConfiguration.cs" company="FH Wiener Neustadt">
// Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Ralf Kuehmayer</author>
//-----------------------------------------------------------------------

namespace BackendApi.Logger;

using Microsoft.Extensions.Logging;

public sealed record ColorConsoleLoggerConfiguration
{
    /// <summary>
    /// Gets or sets the EventId for the configuration.
    /// </summary>
    public int EventId { get; set; }

    /// <summary>
    /// Gets a value indicating whether the console logger should be used.
    /// </summary>
    public bool Use { get; init; } = false;

    /// <summary>
    /// Gets or sets the color map for different log-levels.
    /// </summary>
    public Dictionary<LogLevel, ConsoleColor> LogLevelToColorMap { get; set; } = new()
    {
        [LogLevel.Information] = ConsoleColor.Green,
    };
}