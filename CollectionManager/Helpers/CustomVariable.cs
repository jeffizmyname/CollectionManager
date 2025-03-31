using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.Helpers
{
    public class CustomVariable
    {
        public string Name { get; set; } = string.Empty;

        public string? StringValue { get; set; }
        public int? IntValue { get; set; }
        public List<string>? EnumValues { get; set; }

        public static CustomVariable FromString(string value) => new() { StringValue = value };
        public static CustomVariable FromInt(int value) => new() { IntValue = value };

        public static CustomVariable FromEnumOptions<T>(List<T> values) where T : Enum
        {
            return new CustomVariable
            {
                EnumValues = values.Select(e => e.ToString()).ToList()
            };
        }

        public override string ToString()
        {
            if (StringValue != null) return $"String: {StringValue}";
            if (IntValue != null) return $"Int: {IntValue}";
            if (EnumValues != null) return $"Enums: {string.Join(", ", EnumValues)}";
            return "Empty Choice";
        }
    }
}
