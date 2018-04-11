using Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://msdn.microsoft.com/en-us/library/system.diagnostics.debuggertypeproxyattribute(v=vs.110).aspx

namespace Telemetry.Providers.ConfigFile
{
    public class FilterCollection : ConfigurationElementCollection
    {
        #region this[int index]

        public FilterConfigElement this[int index]
        {
            get { return (FilterConfigElement)BaseGet(index); }
        }

        #endregion // this[int index]

        #region CollectionType

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        #endregion // CollectionType

        #region CreateNewElement

        protected override ConfigurationElement CreateNewElement()
        {
            return new FilterConfigElement();
        }

        #endregion // CreateNewElement

        #region GetElementKey

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FilterConfigElement)element).Path;
        }

        #endregion // GetElementKey

        #region ElementName

        protected override string ElementName
        {
            get { return "filter"; }
        }

        #endregion // ElementName
    }
}
