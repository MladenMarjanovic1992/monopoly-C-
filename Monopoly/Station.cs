using System.Collections.Generic;
using System.Linq;

namespace Monopoly
{
    public class Station
    {
        private readonly List<FieldStation> _fields;

        public Station(List<FieldStation> fields)
        {
            _fields = fields;
        }

        public void OnFieldBought(object sender, FieldBoughtEventArgs e)
        {
            var fields = _fields.Where(f => f.Owner == e.Field.Owner && f.CanMortgage).ToList();

            RentSetter(fields, fields.Count);
        }

        public void OnFieldSold(object sender, FieldSoldEventArgs e)
        {
            var fields = _fields.Where(f => f.Owner == e.FormerOwner && f.CanMortgage).ToList();

            RentSetter(fields, fields.Count);
        }

        public void OnFieldMortgaged(object sender, FieldMortgagedEventArgs e)
        {
            var fields = _fields.Where(f => f.Owner == e.Field.Owner && f.CanMortgage).ToList();

            RentSetter(fields, fields.Count);
        }

        public void OnMortgagePayed(object sender, MortgagePayedEventArgs e)
        {
            var fields = _fields.Where(f => f.Owner == e.Field.Owner && f.CanMortgage).ToList();

            RentSetter(fields, fields.Count);
        }

        private void RentSetter(List<FieldStation> fields, int index)
        {
            foreach (var field in fields)
            {
                field.CurrentRent = field.Rent[index - 1];
            }
        }
    }
}