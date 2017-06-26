namespace Monopoly
{
    public class MortgageSpecification : ISpecification<IFieldRentable>
    {
        private readonly Player _owner;
        private readonly bool _canMortgage;

        public MortgageSpecification(Player owner, bool canMortgage)
        {
            _owner = owner;
            _canMortgage = canMortgage;
        }

        public bool IsSatisfied(IFieldRentable t)
        {
            return t.Owner == _owner && t.CanMortgage == _canMortgage;
        }
    }
}