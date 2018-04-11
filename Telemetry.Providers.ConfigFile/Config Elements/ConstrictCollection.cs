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
        IEnumerable<ActivationConstrict>
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
            return new ConstrictConfigElement();
        }

        #endregion // CreateNewElement

        #region GetElementKey

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ConstrictConfigElement)element).GetHashCode();
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

        #region IEnumerable<ActivationConstrict> Members

        IEnumerator<ActivationConstrict> IEnumerable<ActivationConstrict>.GetEnumerator()
        {
            foreach (ConstrictConfigElement element in this)
            {
                yield return new ActivationConstrict(
                                        element.Importance,
                                        element.ComponentTag,
                                        element.Filters);
            }
        }

        #endregion // IEnumerable<ActivationConstrict> Members
    }
}
