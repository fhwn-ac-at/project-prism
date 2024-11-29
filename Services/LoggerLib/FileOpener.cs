//-----------------------------------------------------------------------
// <copyright file="FileOpener.cs" company="FH Wiener Neustadt">
// Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Ralf Kuehmayer</author>
//-----------------------------------------------------------------------

namespace LoggerLib;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

/// <summary>
///  A class to open a file-stream with given retires and a wait time between these.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="FileOpener"/> class.
/// </remarks>
/// <param name="options">The options to configure the FileOpener.</param>
/// <param name="logger">The logger.</param>
/// <exception cref="ArgumentNullException">
/// Is thrown if the presenter is null.
/// </exception>
public class FileOpener(IOptions<FileOpenerOptions> options, ILogger<FileOpener>? logger)
{
    /// <summary>
    /// The wait time.
    /// </summary>
    private readonly ushort waitTime = options.Value.WaitTime;

    /// <summary>
    /// The amount of retries.
    /// </summary>
    private readonly byte retires = options.Value.Retires;

    /// <summary>
    /// The logger.
    /// </summary>
    private readonly ILogger<FileOpener>? logger = logger; // can be null as it is used for the file logger

    /// <summary>
    /// Opens a file-stream with read access.
    /// </summary>
    /// <param name="path">The path of the file to open.</param>
    /// <returns>The open file stream.</returns>
    /// <exception cref="CouldFileNotOpenException">
    /// Is thrown if the file stream couldn't be opened.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Is thrown if the path is null.
    /// </exception>
    public async Task<FileStream> OpenRead(string path)
    {
        return path == null
            ? throw new ArgumentNullException(nameof(path), "The parameter must not be null.")
            : await this.Open(path, FileMode.Open, FileAccess.Read);
    }

    /// <summary>
    /// Opens a file-stream with write access and a given mode.
    /// </summary>
    /// <param name="path">The path of the file to open.</param>
    /// <param name="mode">The mode how the system should open the file.</param>
    /// <returns>The open file stream.</returns>
    /// <exception cref="CouldFileNotOpenException">
    /// Is thrown if the file stream couldn't be opened.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Is thrown if the path is null.
    /// </exception>
    public async Task<FileStream> OpenWrite(string path, FileMode mode)
    {
        return path == null
            ? throw new ArgumentNullException(nameof(path), "The parameter must not be null.")
            : await this.Open(path, mode, FileAccess.Write);
    }

    /// <summary>
    /// Opens a file-stream with read and write access.
    /// </summary>
    /// <param name="path">The path of the file to open.</param>
    /// <returns>The open file stream.</returns>
    /// <exception cref="CouldFileNotOpenException">
    /// Is thrown if the file stream couldn't be opened.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Is thrown if the path is null.
    /// </exception>
    public async Task<FileStream> OpenReadWrite(string path)
    {
        return path == null
            ? throw new ArgumentNullException(nameof(path), "The parameter must not be null.")
            : await this.Open(path, FileMode.Open, FileAccess.ReadWrite);
    }

    /// <summary>
    /// Opens a file-stream with given retries and a wait time between these.
    /// </summary>
    /// <param name="path">The path of the file to open.</param>
    /// <param name="mode">The mode of how to open the file.</param>
    /// <param name="access">The access to the files.</param>
    /// <returns>The open file stream.</returns>
    /// <exception cref="CouldFileNotOpenException">
    /// Is thrown if the file stream couldn't be opened.
    /// </exception>
    private async Task<FileStream> Open(string path, FileMode mode, FileAccess access)
    {
        path = Path.GetFullPath(path);

        this.logger?.Log(LogLevel.Debug, "Opening file {path} with FileMode {mode} and FileAccess {access}", path, mode, access);
        for (int i = 0; i < this.retires; i++)
        {
            try
            {
                return new FileStream(path, mode, access, FileShare.Read);
            }
            catch (Exception exception)
            {
                this.logger?.Log(LogLevel.Debug, "{exception}", exception);

                this.logger?.Log(LogLevel.Information, "The file {path} could not be opened. Attempt {amount} of {retires}", path, i + 1, this.retires);

                if (i < this.retires - 1)
                {
                    this.logger?.Log(LogLevel.Information, "Wait {waitTime}ms before the next attempt", this.waitTime);
                }
            }

            await Task.Delay(this.waitTime);
        }

        this.logger?.Log(LogLevel.Error, "The file {path} could not be opened.", path);
        throw new CouldFileNotOpenException($"The file {path} could not be opened.");
    }
}
