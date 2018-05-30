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
    public class ConstrictCollection : 
        ConfigurationElementCollection,
        IEnumerable<ActivationItem>
    {
        #region CollectionType

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        #endregion // CollectionType

        #region CreateNewElement

        protected override ConfigurationElement CreateNewElement()
        {
            return new ConfigItemElement();
        }

        #endregion // CreateNewElement

        #region GetElementKey

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ConfigItemElement)element).GetHashCode();
        }

        #endregion // GetElementKey

        #region BaseAdd

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        #endregion // BaseAdd

        #region ElementName

        protected override string ElementName
        {
            get { return "constrict"; }
        }

        #endregion // ElementName

        #region IEnumerable<ActivationItem> Members

        IEnumerator<ActivationItem> IEnumerable<ActivationItem>.GetEnumerator()
        {
            foreach (ConfigItemElement element in this)
            {
                yield return new ActivationItem(
                                        element.MetricThreshold,
                                        element.TextualThreshold,
                                        element.Filters);
            }
        }

        #endregion // IEnumerable<ActivationItem> Members
    }
}
