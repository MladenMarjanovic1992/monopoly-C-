﻿using System.Collections.Generic;

namespace Monopoly
{
    public class Utility // changes the state of Utility fields depending on the circumstances
    {
        private readonly List<FieldUtility> _fields;

        public Utility(List<FieldUtility> fields)
        {
            _fields = fields;
        }

        public void OnFieldBought(object sender, FieldBoughtEventArgs e)
        {
            RentSetter(_fields.TrueForAll(f => f.Owner == e.Field.Owner) ? 1 : 0);
        }

        public void OnFieldSold(object sender, FieldSoldEventArgs e)
        {
            RentSetter(0);
        }

        public void OnMortgagePayed(object sender, MortgagePayedEventArgs e)
        {
            RentSetter(_fields.TrueForAll(f => f.Owner == e.Field.Owner) ? 1 : 0);
        }

        public void OnFieldMortgaged(object sender, FieldMortgagedEventArgs e)
        {
            RentSetter(0);
        }

        private void RentSetter(int index)
        {
            foreach (var field in _fields)
            {
                field.CurrentRent = field.Rent[index];
            }
        }
    }
}