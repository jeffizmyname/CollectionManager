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


    }
}
