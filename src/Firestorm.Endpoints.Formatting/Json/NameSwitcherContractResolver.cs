﻿using Newtonsoft.Json.Serialization;

namespace Firestorm.Endpoints.Formatting.Json
{
    public class NameSwitcherContractResolver : DefaultContractResolver
    {
        private readonly INamingConventionSwitcher _namingConventionSwitcher;

        public NameSwitcherContractResolver(INamingConventionSwitcher namingConventionSwitcher)
        {
            _namingConventionSwitcher = namingConventionSwitcher;
        }

        protected override string ResolvePropertyName(string propertyName)
        {
            return _namingConventionSwitcher.ConvertCodedToDefault(propertyName);
        }
    }
}