using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Fields
    {
        public List<FieldProperty> PropertyFields { get; set; }
        public List<FieldStation> StationFields { get; set; }
        public List<FieldUtility> UtilityFields { get; set; }
        public List<FieldTax> TaxFields { get; set; }
        public List<FieldChance> ChanceFields { get; set; }
        public List<FieldCommunityChest> CommunityChestFields { get; set; }
        public List<FieldGoToJail> GoToJailFields { get; set; }
        public List<FieldOther> OtherFields { get; set; }

        // Fields which can be bought and traded
        public List<IFieldRentable> BuyableFields
        {
            get
            {
                var fields = new List<IFieldRentable>();

                fields.AddRange(PropertyFields);
                fields.AddRange(StationFields);
                fields.AddRange(UtilityFields);

                return fields;
            }
        }

        // Only fields which can have houses
        public List<IFieldBuildable> BuildableFields
        {
            get
            {
                var fields = new List<IFieldBuildable>();

                fields.AddRange(PropertyFields);

                return fields;
            }
        }
    }
}