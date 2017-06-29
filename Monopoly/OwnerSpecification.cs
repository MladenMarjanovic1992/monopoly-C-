namespace Monopoly
{
    public class OwnerSpecification : ISpecification<IFieldRentable>
    {
        private readonly Player _owner;

        public OwnerSpecification(Player owner)
        {
            _owner = owner;
        }

        public bool IsSatisfied(IFieldRentable t)
        {
            if (t.Owner != null)
                return t.Owner == _owner && t.CanTrade;
            return false;
        }
    }
}