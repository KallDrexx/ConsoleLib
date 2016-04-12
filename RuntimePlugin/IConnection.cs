using RuntimePlugin.RuntimeReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuntimePlugin
{
    interface IConnection
    {
        void SetProperty(InstanceReference instanceReference, PropertyReference propertyReference, object value);
        object GetProperty(InstanceReference instanceReference, PropertyReference propertyReference);
    }
}
