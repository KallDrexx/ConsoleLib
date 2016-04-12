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
        WpfDataUi.DataUiGrid grid;
        public WpfDataUi.DataUiGrid Grid
        {
            get { return grid; }
            set { grid = value; UpdateGridCategories(); }
        }

        public PropertiesController()
        {
        }

        private void UpdateGridCategories()
        {
            var category = new MemberCategory("Test Category");

            var instanceMember = new InstanceMember("Some value", this);
            instanceMember.CustomSetEvent += (owner, value) =>
            {
                System.Console.WriteLine($"Setting the value of {owner} to {value}");
            };

            instanceMember.CustomGetEvent += (owner) =>
            {
                System.Console.WriteLine($"Returning the value for {owner}");
                return 10;
            };

            instanceMember.CustomGetTypeEvent += (owner) =>
            {
                System.Console.WriteLine($"Returning the type for {owner}");

                return typeof(int);
            };

            category.Members.Add(instanceMember);

            Grid.Categories.Add(category);
        }
    }
}
