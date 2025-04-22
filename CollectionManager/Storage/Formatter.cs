using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectionManager.Helpers;
using CollectionManager.Models;

namespace CollectionManager.Storage
{
    public static class Formatter
    {
        public static string SerializeCollection(Collection collection)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[Collection]");
            sb.AppendLine($"Id={collection.Id}");
            sb.AppendLine($"Name={collection.Name}");
            sb.AppendLine($"Description={collection.Description}");
            return sb.ToString();
        }

        public static string SerializeCollections(List<Collection> collections)
        {
            StringBuilder sb = new StringBuilder();
            collections.ForEach(coll =>
            {
                sb.AppendLine(SerializeCollection(coll));
            });

            return sb.ToString();
        }

        public static string SerializeCollectionItems(Collection collection, string path)
        {
            StringBuilder sb = new StringBuilder();
            collection.Items.ForEach(item => {
                sb.AppendLine(SerializeCollectionItem(item));
            });

            return sb.ToString();
        }

        public static string SerializeCollectionItem(CollectionItem item)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[Item]\n");
            sb.Append($"Id={item.Id}\n");
            sb.Append($"Name={item.Name}\n");
            sb.Append($"CollectionName={item.CollectionName}\n");
            sb.Append($"Description={item.Description}\n");
            sb.Append($"Price={item.Price}\n");
            sb.Append($"CollectionDate={item.CollectionDate}\n");
            sb.Append($"SellDate={item.SellDate}\n");
            sb.Append($"Rating={item.Rating}\n");
            sb.Append($"Sold={item.Sold}\n");
            sb.Append($"ToSale={item.ToSale}\n");
            sb.Append($"ImageName={item.ImageName}\n");
            sb.Append($"CustomVariable={item.customVariable}\n");
            return sb.ToString();
        }

        public static List<CollectionItem> DeserializeItems(string data)
        {
            List<CollectionItem> parsedItems = new List<CollectionItem>();
            List<string> items = data.Split(new string[] { "[Item]" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            items.ForEach(item =>
            {
                item = item.Replace("[Item]", "");
                List<string> attributes = item.ReplaceLineEndings().Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();

                Dictionary<string, string> attributesSplit = new Dictionary<string, string>();
                for (int i = 0; i < attributes.Count; i++)
                {
                    int index = attributes[i].IndexOf("=");
                    attributesSplit.Add(attributes[i].Substring(0, index), attributes[i].Substring(index + 1));
                }

                CollectionItem newItem = new CollectionItem();
                foreach (KeyValuePair<string, string> entry in attributesSplit)
                {
                    string key = entry.Key;
                    string value = entry.Value;

                    Debug.WriteLine($"{key}: {value}");

                    switch (key)
                    {
                        case "Id":
                            newItem.Id = int.Parse(value);
                            break;
                        case "Name":
                            newItem.Name = value;
                            break;
                        case "Description":
                            newItem.Description = value;
                            break;
                        case "Price":
                            newItem.Price = float.Parse(value);
                            break;
                        case "CollectionDate":
                            newItem.CollectionDate = DateTime.Parse(value);
                            break;
                        case "SellDate":
                            newItem.SellDate = DateTime.Parse(value);
                            break;
                        case "Rating":
                            newItem.Rating = int.Parse(value);
                            break;
                        case "Sold":
                            newItem.Sold = bool.Parse(value);
                            break;
                        case "ToSale":
                            newItem.ToSale = bool.Parse(value);
                            break;
                        case "ImageName":
                            newItem.ImageName = value;
                            break;
                        case "CustomVariable":
                            newItem.customVariable = CustomVariable.Parse(value);
                            break;
                        case "CollectionName":
                            newItem.CollectionName = value;
                            break;
                    }
                }
                parsedItems.Add(newItem);
            });
            return parsedItems;
        }

        public static List<Collection> DeserializeCollections(string data)
        {
            List<Collection> parsedCollections = new List<Collection>();
            List<string> collections = data.Split(new string[] { "[Collection]" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            collections.ForEach(collection =>
            {
                collection = collection.Replace("[Collection]", "");
                List<string> attributes = collection.ReplaceLineEndings().Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();
                Dictionary<string, string> attributesSplit = new Dictionary<string, string>();
                for (int i = 0; i < attributes.Count; i++)
                {
                    int index = attributes[i].IndexOf("=");
                    attributesSplit.Add(attributes[i].Substring(0, index), attributes[i].Substring(index + 1));
                }
                Collection newCollection = new Collection();
                foreach (KeyValuePair<string, string> entry in attributesSplit)
                {
                    string key = entry.Key;
                    string value = entry.Value;
                    Debug.WriteLine($"{key}: {value}");
                    switch (key)
                    {
                        case "Id":
                            newCollection.Id = int.Parse(value);
                            break;
                        case "Name":
                            newCollection.Name = value;
                            break;
                        case "Description":
                            newCollection.Description = value;
                            break;
                    }
                }
                parsedCollections.Add(newCollection);
            });
            return parsedCollections;
        }

        public static string SerializeSet(Set set)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[Set]\n");
            sb.Append($"Id={set.Id}\n");
            sb.Append($"Name={set.Name}\n");
            sb.Append($"Values={string.Join(",", set.Values)}\n");
            return sb.ToString();
        }

        public static List<Set> DeserializeSets(string data)
        {
            List<Set> parsedSets = new List<Set>();
            List<string> sets = data.Split(new string[] { "[Set]" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            sets.ForEach( set => {
                set = set.Replace("[Set]", "");
                List<string> attributes = set.ReplaceLineEndings().Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();
                Dictionary<string, string> attributesSplit = new Dictionary<string, string>();
                for (int i = 0; i < attributes.Count; i++)
                {
                    int index = attributes[i].IndexOf("=");
                    attributesSplit.Add(attributes[i].Substring(0, index), attributes[i].Substring(index + 1));
                }
                Set newSet = new Set("", new List<string>());
                foreach (KeyValuePair<string, string> entry in attributesSplit)
                {
                    string key = entry.Key;
                    string value = entry.Value;
                    Debug.WriteLine($"{key}: {value}");
                    switch (key)
                    {
                        case "Id":
                            newSet.Id = int.Parse(value);
                            break;
                        case "Name":
                            newSet.Name = value;
                            break;
                        case "Values":
                            newSet.Values = value.Split(",").ToList();
                            break;
                    }
                }
                parsedSets.Add(newSet);
            });
            return parsedSets;
        }
    }
}
