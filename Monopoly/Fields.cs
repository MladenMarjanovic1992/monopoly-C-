using System;
using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Fields
    {
        public List<PropertyField> PropertyFields { get; set; }
        public List<StationField> StationFields { get; set; }
        public List<UtilityField> UtilityFields { get; set; }
        public List<TaxField> TaxFields { get; set; }
        public List<ChanceField> ChanceFields { get; set; }
        public List<CommunityChestField> CommunityChestFields { get; set; }
        public List<GoToJailField> GoToJailFields { get; set; }
        public List<OtherFields> OtherFields { get; set; }

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
    }
}