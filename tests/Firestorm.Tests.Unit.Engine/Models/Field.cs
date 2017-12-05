using Firestorm.Engine.Fields;

namespace Firestorm.Tests.Unit.Engine.Models
{
    public class Field<T>
        where T : class
    {
        public IFieldReader<T> Reader { get; set; }

        public IFieldWriter<T> Writer { get; set; }
    }
}