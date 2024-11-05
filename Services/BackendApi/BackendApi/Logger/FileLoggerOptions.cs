//-----------------------------------------------------------------------
// <copyright file="FileLoggerOptions.cs" company="FH Wiener Neustadt">
//     Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Ralf Kuehmayer</author>
//-----------------------------------------------------------------------

namespace BackendApi.Logger;

/// <summary>
/// A record for configuring a file logger.
/// </summary>
public record FileLoggerOptions
{
    /// <summary>
    /// Gets the path of the logfile.
    /// </summary>
    public string? Path { get; init; } = Directory.GetCurrentDirectory() + "log.log";

    /// <summary>
    /// Gets a value indicating whether the logger should be used.
    /// </summary>
    public bool Use { get; init; } = false;

    /// <summary>
    /// Gets a value indicating whether the logger should append to a file or override it.
    /// </summary>
    public bool Append { get; init; } = false;
}
