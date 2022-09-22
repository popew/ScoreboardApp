using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreboardApp.Domain.Commons
{
    // See more: https://github.com/vkhorikov/CSharpFunctionalExtensions/blob/master/CSharpFunctionalExtensions/ValueObject/SimpleValueObject.cs

    [Serializable]
    public abstract class ValueObjectOf<T> : ValueObject
    {
        public T Value { get; }

        protected ValueObjectOf(T value)
        {
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString()
        {
            return Value?.ToString();
        }

        public static implicit operator T(ValueObjectOf<T> valueObject)
        {
            return valueObject == null ? default : valueObject.Value;
        }
    }
}
