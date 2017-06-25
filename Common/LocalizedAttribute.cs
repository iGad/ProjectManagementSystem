using System;
using System.ComponentModel;
using System.Resources;

namespace Common
{
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string _resourceKey;
        private readonly ResourceManager _resource;
        public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
        {
            this._resource = new ResourceManager(resourceType);
            this._resourceKey = resourceKey;
        }

        public override string Description
        {
            get
            {
                string displayName = this._resource.GetString(this._resourceKey);

                return string.IsNullOrEmpty(displayName)
                    ? string.Format("[[{0}]]", this._resourceKey)
                    : displayName;
            }
        }
    }
}
