using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public enum ImportanceLevel
    {
        Low,
        Normal,
        High,
        /// <summary>
        /// The critical cannot be switched off
        /// </summary>
        Critical
    }
}
