using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Contracts
{
    public interface ISimpleConfig 
    {
        string this[string key] { get; }
    }
}
