using Firestorm.Engine.Fields;
using Firestorm.Stems.Definitions;

namespace Firestorm.Stems.Essentials.Factories.Factories
{
    internal class AttributeFieldDescription : IFieldDescription
    {
        private readonly FieldDescription _description;

        internal AttributeFieldDescription(FieldDescription description)
        {
            _description = description;
        }

        public string Description => _description.Text;

        public object Example => _description.Example;
    }
}