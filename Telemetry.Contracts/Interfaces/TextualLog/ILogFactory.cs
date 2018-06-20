using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace Contracts
{
    public interface ILogFactory
    {
        /// <summary>
        /// Creates the specified instance.
        /// </summary>
        /// <returns></returns>
        ILogger Create();
        /// <summary>
        /// Creates the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        ILogger Create<T>(T instance);
        /// <summary>
        /// Creates the specified instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ILogger Create<T>();
    }
}
