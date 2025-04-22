using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.Models
{
    public class Set
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Values { get; set; }

        public Set(string name, List<string> values)
        {
            Name = name;
            Values = values;
        }

        public static bool AreSetsEqual(Set a, Set b)
        {
            if (a.Name != b.Name) return false;

            if (a.Values.Count != b.Values.Count)
                return false;

            for (int i = 0; i < a.Values.Count; i++)
            {
                if (a.Values[i] != b.Values[i])
                    return false;
            }

            return true;
        }

        public static List<Set> RemoveDuplicateSets(List<Set> sets)
        {
            List<Set> uniqueSets = new List<Set>();

            foreach (var set in sets)
            {
                bool isDuplicate = uniqueSets.Any(existing => AreSetsEqual(existing, set));
                if (!isDuplicate)
                {
                    uniqueSets.Add(set);
                }
            }

            return uniqueSets;
        }
    }
}
