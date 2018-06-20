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
    public class ChannelCollection : 
        ConfigurationElementCollection,
        IEnumerable<KeyValuePair<string, ActivationUnit>>
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
            return new ChannelConfigItemElement();
        }

        #endregion // CreateNewElement

        #region GetElementKey

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ChannelConfigItemElement)element).GetHashCode();
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
            get { return "channel"; }
        }

        #endregion // ElementName

        #region IEnumerable<ActivationConstrict> Members

        IEnumerator<KeyValuePair<string, ActivationUnit>> IEnumerable<KeyValuePair<string, ActivationUnit>>.GetEnumerator()
        {
            foreach (ChannelConfigItemElement element in this)
            {
                yield return new KeyValuePair<string, ActivationUnit>(element.Key, 
                    new ActivationUnit(
                            element.MetricThreshold,
                            element.TextualThreshold,
                            element.Constricts,
                            element.Extends));
            }
        }

        #endregion // IEnumerable<ActivationConstrict> Members
    }
}
