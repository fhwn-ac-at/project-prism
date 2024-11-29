//-----------------------------------------------------------------------
// <copyright file="Exceptions.cs" company="FH Wiener Neustadt">
// Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Ralf Kuehmayer</author>
//-----------------------------------------------------------------------
namespace LoggerLib;
using System;



/// <summary>
/// A exception if the file could not be opened.
/// </summary>
public class CouldFileNotOpenException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CouldFileNotOpenException"/> class.
    /// </summary>
    /// <param name="message">The message of the exception.</param>
    public CouldFileNotOpenException(string message)
        : base(message)
    {
    }
}