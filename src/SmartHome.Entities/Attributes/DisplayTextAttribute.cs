using System;

namespace SmartHome.Core.Entities.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DisplayTextAttribute : Attribute
    {
        public string Text { get; set; }

        public DisplayTextAttribute(string text)
        {
            Text = text;
        }
    }
}
