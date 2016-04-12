using FlatRedBall.Glue.Plugins.ExportedImplementations;
using RuntimePlugin.RuntimeReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfDataUi.DataTypes;

namespace RuntimePlugin
{
    public class PropertiesController
    {
        List<InstanceReference> Instances = new List<InstanceReference>();


        WpfDataUi.DataUiGrid grid;
        public WpfDataUi.DataUiGrid Grid
        {
            get { return grid; }
            set { grid = value; UpdateGridCategories(); }
        }

        public PropertiesController()
        {
        }

        private void RefreshInstances()
        {
            Instances.Clear();

            Instances.AddRange(GetInstances());

        }

        private IEnumerable<InstanceReference> GetInstances()
        {
            yield break;
        }

        private void UpdateGridCategories()
        {
            var category = new MemberCategory("Test Category");

            var instanceName = GlueState.Self.CurrentNamedObjectSave?.InstanceName;

            var foundInstance = Instances.FirstOrDefault(item => item.Name == instanceName);

            if (foundInstance != null)
            {
                List<PropertyReference> objectProperties = new List<PropertyReference>(); //GetObjectProperties(host, "CurrentScreen", availableEntities[0]);

                foreach (var property in objectProperties)
                {
                    var member = new ConnectedInstanceMember();
                    member.InstanceReference = foundInstance;
                    member.PropertyReference = property;

                    // todo: get the type
                    member.Type = typeof(object);
                    member.Connection = null;

                    category.Members.Add(member);
                }

                grid.Categories.Add(category);
            }

            //var instanceMember = new InstanceMember("Some value", this);
            //instanceMember.CustomSetEvent += (owner, value) =>
            //{
            //    System.Console.WriteLine($"Setting the value of {owner} to {value}");
            //};

            //instanceMember.CustomGetEvent += (owner) =>
            //{
            //    System.Console.WriteLine($"Returning the value for {owner}");
            //    return 10;
            //};

            //instanceMember.CustomGetTypeEvent += (owner) =>
            //{
            //    System.Console.WriteLine($"Returning the type for {owner}");

            //    return typeof(int);
            //};

            //category.Members.Add(instanceMember);

            //Grid.Categories.Add(category);
        }

        private List<PropertyReference> GetObjectProperties(object host, string v, object p)
        {
            throw new NotImplementedException();
        }
    }
}
