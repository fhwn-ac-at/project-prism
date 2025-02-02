﻿//-----------------------------------------------------------------------
// <copyright file="FileOpenerOptions.cs" company="FH Wiener Neustadt">
// Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Ralf Kuehmayer</author>
//-----------------------------------------------------------------------

namespace LoggerLib;

using FrenziedMarmot.DependencyInjection;

/// <summary>
/// The configuration options for the <see cref="FileOpener"/>
/// </summary>
[InjectableOptions("FileOptions")]
public record FileOpenerOptions
{
    /// <summary>
    /// Gets the wait time in milliseconds.
    /// </summary>
    public ushort WaitTime { get; init; } = 10;

    /// <summary>
    /// Gets the amount of retries.
    /// </summary>
    public byte Retires { get; init; } = 1;
}