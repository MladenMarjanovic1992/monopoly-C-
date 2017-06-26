using System.Collections.Generic;

namespace Monopoly
{
    public interface IFilter<TFieldType>
    {
        IEnumerable<TFieldType> FilterFields(IEnumerable<TFieldType> fields, ISpecification<TFieldType> spec);
    }
}