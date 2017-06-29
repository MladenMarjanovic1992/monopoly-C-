using System.Collections.Generic;

namespace Monopoly
{
    public class FieldBuildableFilter : IFilter<IFieldBuildable>
    {
        public IEnumerable<IFieldBuildable> FilterFields(IEnumerable<IFieldBuildable> fields, ISpecification<IFieldBuildable> spec)
        {
            foreach (var field in fields)
            {
                if (spec.IsSatisfied(field))
                    yield return field;
            }
        }
    }
}