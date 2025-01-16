//-----------------------------------------------------------------------
// <copyright file="FileOpener.cs" company="FH Wiener Neustadt">
// Copyright (c) FH Wiener Neustadt. All rights reserved.
// </copyright>
// <author>Ralf Kuehmayer</author>
//-----------------------------------------------------------------------

namespace LoggerLib
{
    using System.IO;
    using System.Threading.Tasks;

    public interface IFileOpener
    {
        Task<Stream> OpenRead(string path);
        Task<Stream> OpenReadWrite(string path);
        Task<Stream> OpenWrite(string path, FileMode mode);
    }
}