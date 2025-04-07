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
            if (StringValue != null) return $"[String][{Name}][{StringValue}]";
            if (IntValue != null) return $"[Int][{Name}][{IntValue}]";
            if (EnumValues != null) return $"[Enums][{Name}][{string.Join(",", EnumValues)}]";
            return "Empty Choice";
        }


        public static CustomVariable Parse(string input)
        {
            //[type][name][value]
            CustomVariable parsed = new CustomVariable();
            string[] parts = input.Trim('[', ']').Split(new[] { "][" }, StringSplitOptions.None);

            if (parts.Length == 3)
            {
                string type = parts[0];
                string name = parts[1];
                string value = parts[2];

                parsed.Name = name;

                if (type == "String")
                {
                    parsed.StringValue = value;
                }
                else if (type == "Int")
                {
                    parsed.IntValue = int.Parse(value);
                }
                else if (type == "Enums")
                {
                    parsed.EnumValues = value.Split(",", StringSplitOptions.None).ToList();
                }
            }
            else
            {
                Console.WriteLine("Invalid input format");
            }
            return parsed;
        }
    }
}
