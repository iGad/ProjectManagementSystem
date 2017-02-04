using System;
using System.ComponentModel;
using System.Resources;

namespace Common
{
    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private readonly string resourceKey;
        private readonly ResourceManager resource;
        public LocalizedDescriptionAttribute(string resourceKey, Type resourceType)
        {
            this.resource = new ResourceManager(resourceType);
            this.resourceKey = resourceKey;
        }

        public override string Description
        {
            get
            {
                string displayName = this.resource.GetString(this.resourceKey);

                return string.IsNullOrEmpty(displayName)
                    ? string.Format("[[{0}]]", this.resourceKey)
                    : displayName;
            }
        }
    }
}
