namespace Monopoly
{
    public interface ISpecification<TFieldType>
    {
        bool IsSatisfied(TFieldType t);
    }
}