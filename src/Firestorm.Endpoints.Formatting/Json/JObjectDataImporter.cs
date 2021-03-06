using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace Firestorm.Endpoints.Formatting.Json
{
    public class JObjectDataImporter : IRestItemDataImporter
    {
        private readonly JObjectToDictionaryTranslator<ExpandoObject> _dictionaryTranslator;

        public JObjectDataImporter()
        {
            _dictionaryTranslator = new JObjectToDictionaryTranslator<ExpandoObject>(null);
        }

        public bool CanImport(object obj)
        {
            return obj is JObject;
        }

        public Type GetType(object obj)
        {
            return typeof(object);
            //switch (((JObject)obj).ResourceType)
        }

        public IEnumerable<KeyValuePair<string, object>> GetValues(object obj)
        {
            var jObj = (JObject) obj;
            foreach (KeyValuePair<string, JToken> pair in jObj)
            {
                yield return new KeyValuePair<string, object>(pair.Key, _dictionaryTranslator.Convert(pair.Value));
            }
        }
    }
}