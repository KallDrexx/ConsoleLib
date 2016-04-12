using RuntimePlugin.RuntimeReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfDataUi.DataTypes;

namespace RuntimePlugin
{
    class ConnectedInstanceMember : InstanceMember
    {
        public InstanceReference InstanceReference { get; set; }
        public PropertyReference PropertyReference { get; set; }

        public Type Type { get; set; }

        public IConnection Connection { get; set; }

        public ConnectedInstanceMember()
        {
            this.CustomGetEvent += HandleGet;
            this.CustomSetEvent += HandleSet;
            this.CustomGetTypeEvent += HandleGetType;
        }

        private Type HandleGetType(object owner)
        {
            return Type;
        }

        private void HandleSet(object owner, object value)
        {
            Connection.SetProperty(InstanceReference, PropertyReference, value);
        }

        private object HandleGet(object owner)
        {
            return Connection.GetProperty(InstanceReference, PropertyReference);
        }
    }
}
