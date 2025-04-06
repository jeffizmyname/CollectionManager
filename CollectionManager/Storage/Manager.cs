using CollectionManager.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionManager.Storage
{
    static class Manager
    {
        private static string StoragePath = Path.Combine(FileSystem.AppDataDirectory, "/data");
        public static void Initialize()
        {
            Debug.WriteLine("Storage path: " + StoragePath);
            if (!Directory.Exists(StoragePath))
            {
                Directory.CreateDirectory(StoragePath);
            }
        }

        public static string SaveItem(CollectionItem item)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(StoragePath, item.CollectionName + ".txt"), true))
            {
                sw.Write(Formatter.SerializeCollectionItem(item));
            }
        }


    }
}
