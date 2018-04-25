using Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://msdn.microsoft.com/en-us/library/system.diagnostics.debuggertypeproxyattribute(v=vs.110).aspx

namespace Contracts
{
    [DebuggerTypeProxy(typeof(DebugView))]
    public class ActivationSetting
    {
        #region Ctor

        public ActivationSetting(
            ImportanceLevel minImportance,
            IEnumerable<ActivationConstrict> constricts,
            IEnumerable<ActivationExtend> extends)
        {
            MinImportance = minImportance;
            Constricts = constricts?.ToArray() ?? Array.Empty<ActivationConstrict>();
            Extends = extends?.ToArray() ?? Array.Empty<ActivationExtend>();
        }

        #endregion // Ctor

        #region MinImportance

        public ImportanceLevel MinImportance { get; }

        #endregion // MinImportance

        #region Constricts

        public ActivationConstrict[] Constricts { get; }

        #endregion // Constricts

        #region Extends
        public ActivationExtend[] Extends { get; }

        #endregion // Extends

        #region DebugView

        internal class DebugView
        {
            private ActivationSetting _instance;

            public DebugView(ActivationSetting instance)
            {
                this._instance = instance;
            }


            public ImportanceLevel MinImportance => _instance.MinImportance;

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ActivationConstrict[] Constricts => _instance.Constricts.ToArray();

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public ActivationExtend[] Extends => _instance.Extends.ToArray();
        }

        #endregion // DebugView
    }
}
