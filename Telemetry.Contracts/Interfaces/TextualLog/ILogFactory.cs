using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace Contracts
{
    public interface ILogFactory
    {
        ILogger Create();
    }
}
