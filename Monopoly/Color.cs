using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Color // changes the state of fields belonging to the same color depending on the circumstances
    {
        public string ColorName { get; set; }
        public Player Owner { get; set; }
        public List<FieldProperty> Fields { get; set; }

        public Color(string colorName, List<FieldProperty> fields)
        {
            ColorName = colorName;
            Fields = fields;
        }

        // Rules for building: A player can only build houses on a field if certain conditions are met:
        //  1) He owns all fields of the same color
        //  2) None of the fields of that color is under mortgage
        //  3) None of the fields of that color have fewer houses on them than that field
        //  3) The field has fewer than 5 houses

        // Rule for selling houses: A house can't be sold on fields which have fewer houses than other fields of the same color

        // Rules for selling/mortgaging a field:
        //  1) A field can only be sold/mortgaged if none of the fields of that color have houses build

        public void OnFieldBought(object sender, FieldBoughtEventArgs e)
        {
            if (e.Field.Color == ColorName && Fields.TrueForAll(f => f.Owner == e.NewOwner && !f.UnderMortgage))
            {
                Owner = e.NewOwner;

                foreach (var field in Fields)
                {
                    field.CanBuild = true;
                    field.CurrentRent = field.Rent[1];
                }
            }
        }

        public void OnFieldSold(object sender, FieldSoldEventArgs e)
        {
            if (e.Field.Color == ColorName && !Fields.TrueForAll(f => f.Owner == e.FormerOwner))
            {
                foreach (var field in Fields)
                {
                    field.CanBuild = false;
                    field.CurrentRent = field.Rent[0];
                }
            }
        }

        public void OnHouseBuilt(object sender, HouseBuiltEventArgs e)
        {
            if (e.Field.Color == ColorName)
            {
                e.Field.CanBuild = false;
                e.Field.CanRemoveHouse = true;
                foreach (var field in Fields)
                {
                    field.CanMortgage = false;
                    field.CanTrade = false;
                }

                if (Fields.TrueForAll(f => f.Houses == e.Field.Houses) && Fields.TrueForAll(f => f.Houses < 5))
                {
                    foreach (var field in Fields)
                    {
                        field.CanBuild = true;
                        field.CanRemoveHouse = true;
                    }
                }
                else
                {
                    var buildableFields = Fields.Where(f => f.Houses < e.Field.Houses);

                    foreach (var field in buildableFields)
                    {
                        field.CanBuild = true;
                        field.CanRemoveHouse = false;
                    }
                }
            }
        }

        public void OnHouseSold(object sender, HouseSoldEventArgs e)
        {
            if (e.Field.Color == ColorName)
            {
                e.Field.CanBuild = true;
                e.Field.CanRemoveHouse = false;

                if (Fields.TrueForAll(f => f.Houses == e.Field.Houses) && Fields.TrueForAll(f => f.Houses > 0))
                {
                    e.Field.CanRemoveHouse = true;
                }
                else
                {
                    var notBuildableFields = Fields.Where(f => f.Houses > e.Field.Houses);

                    foreach (var field in notBuildableFields)
                    {
                        field.CanBuild = false;
                        field.CanRemoveHouse = true;
                    }
                }

                if (Fields.TrueForAll(f => f.Houses == 0))
                {
                    foreach (var field in Fields)
                    {
                        field.CanMortgage = true;
                        field.CanTrade = true;
                    }
                }
            }
            
        }

        public void OnFieldMortgaged(object sender, FieldMortgagedEventArgs e)
        {
            if (e.Field.Color == ColorName)
            {
                foreach (var field in Fields)
                {
                    field.CanBuild = false;
                    field.CurrentRent = field.Rent[0];
                }
            }
        }

        public void OnMortgagePayed(object sender, MortgagePayedEventArgs e)
        {
            if (e.Field.Color == ColorName && e.Field.Owner == Owner && Fields.TrueForAll(f => !f.UnderMortgage))
            {
                foreach (var field in Fields)
                {
                    field.CanBuild = true;
                    field.CurrentRent = field.Rent[1];
                }
            }
        }
    }
}