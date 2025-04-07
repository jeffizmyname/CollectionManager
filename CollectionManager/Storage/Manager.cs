using CollectionManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CollectionManager.Storage.Formatter;

namespace CollectionManager.Storage
{
    static class Manager
    {
        private static string StoragePath = Path.Combine(FileSystem.AppDataDirectory, "data");
        public static void InitializeStorage()
        {
            Debug.WriteLine("Storage path: " + $"{FileSystem.AppDataDirectory.ToString()}\\data");
            if (!Directory.Exists(StoragePath))
            {
                Directory.CreateDirectory(StoragePath);
            }
        }

        public static void SaveItem(CollectionItem item)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(StoragePath, item.CollectionName + ".txt"), true))
            {
                sw.Write(SerializeCollectionItem(item));
            }
        }

        //public static string SaveItems(List<CollectionItem> items)
        //{
        //    using (StreamWriter sw = new StreamWriter(Path.Combine(StoragePath, item.CollectionName + ".txt"), true))
        //    {
        //        sw.Write(Formatter.SerializeCollectionItems(items));
        //    }
        //}

        public static List<CollectionItem> LoadItems(string collectionName)
        {
            using (StreamReader sr = new StreamReader(Path.Combine(StoragePath, collectionName + ".txt")))
            {
                string data = String.Empty;
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    data += line + "\r\n";
                }
                return DeserializeItems(data);
            }
        }

        public static void SaveCollection(Collection collection)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(StoragePath, "collections.txt"), true))
            {
                sw.Write(SerializeCollection(collection));
            }
        }

        public static List<Collection> LoadCollections()
        {
            using(StreamReader sr = new StreamReader(Path.Combine(StoragePath, "collections.txt")))
            {
                string data = String.Empty;
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    data += line + "\r\n";
                }
                return DeserializeCollections(data);
            }

        }
    }
}
