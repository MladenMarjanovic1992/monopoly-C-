using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Color
    {
        public string ColorName { get; set; }
        public Player Owner { get; set; }
        public List<PropertyField> Fields { get; set; }

        public Color(string colorName, List<PropertyField> fields)
        {
            ColorName = colorName;
            Fields = fields;
        }

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