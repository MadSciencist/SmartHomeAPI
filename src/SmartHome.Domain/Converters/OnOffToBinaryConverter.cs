using System;

namespace SmartHome.Core.Domain.Converters
{
    public class OnOffToBinaryConverter : IDataConverter
    {
        public string Convert(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException(nameof(input));
            }

            switch (input.ToLowerInvariant())
            {
                case "on": return "1";
                case "off": return "0";

                default: throw new ArgumentException($"Argument ${input} has to be 'on' or 'off");
            }
        }

        public string ReverseConvert(string input)
        {
            throw new NotImplementedException();
        }
    }
}
