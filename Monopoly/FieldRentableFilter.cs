using System.Collections.Generic;

namespace Monopoly
{
    public class FieldRentableFilter : IFilter<IFieldRentable>
    {
        public IEnumerable<IFieldRentable> FilterFields(IEnumerable<IFieldRentable> fields, ISpecification<IFieldRentable> spec)
        {
            foreach (var field in fields)
            {
                if (spec.IsSatisfied(field))
                    yield return field;
            }
        }
    }
}