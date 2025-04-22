using CollectionManager.Models;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static CollectionManager.Storage.Formatter;

namespace CollectionManager.Storage
{
    static class Manager
    {
        private static string StoragePath = Path.Combine(FileSystem.AppDataDirectory, "data");

        public static string CurrentCollectionName { get; set; } = String.Empty;
        public static void InitializeStorage()
        {
            Debug.WriteLine("Storage path: " + $"{FileSystem.AppDataDirectory.ToString()}\\data");
            if (!Directory.Exists(StoragePath))
            {
                Directory.CreateDirectory(StoragePath);
            }
            if (!Directory.Exists(Path.Combine(StoragePath, "Temp")))
            {
                Directory.CreateDirectory(Path.Combine(StoragePath, "Temp"));
            }
            if (!Directory.Exists(Path.Combine(StoragePath, "Images")))
            {
                Directory.CreateDirectory(Path.Combine(StoragePath, "Images"));
            }
            if (!Directory.Exists(Path.Combine(StoragePath, "Sets")))
            {
                Directory.CreateDirectory(Path.Combine(StoragePath, "Sets"));
            }
            if (!File.Exists(Path.Combine(StoragePath, "Sets", "Sets.txt")))
            {
                File.WriteAllTextAsync(Path.Combine(StoragePath, "Sets", "Sets.txt"), "");
            }
        }

        public static void SaveItem(CollectionItem item)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(StoragePath, item.CollectionName + ".txt"), true))
            {
                sw.Write(SerializeCollectionItem(item));
            }
        }

        public static void UpdateItems(List<CollectionItem> items, string collectionName)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(StoragePath, collectionName + ".txt"), false))
            {
                sw.Write(SerializeCollectionItems(items, collectionName));
            }
        }

        public static List<CollectionItem> LoadItems(string collectionName)
        {
            if (!File.Exists(Path.Combine(StoragePath, collectionName + ".txt")))
            {
                File.WriteAllTextAsync(Path.Combine(StoragePath, collectionName + ".txt"), "");
            }
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

        public static void SaveSet(Set set)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(StoragePath, "Sets", "Sets.txt"), true))
            {
                sw.Write(SerializeSet(set));
            }
        }

        public static List<Set> LoadSets()
        {
            using (StreamReader sr = new StreamReader(Path.Combine(StoragePath, "Sets", "Sets.txt")))
            {
                string data = String.Empty;
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    data += line + "\r\n";
                }
                return DeserializeSets(data);
            }
        }

        public static string CopyImage(string path)
        {
            Directory.CreateDirectory(Path.Combine(StoragePath, "Temp"));
            Directory.CreateDirectory(Path.Combine(StoragePath, "Images"));

            string extension = Path.GetExtension(path);
            string newFileName = $"{Guid.NewGuid()}{extension}";

            string tempPath = Path.Combine(StoragePath, "Temp", newFileName);
            string finalPath = Path.Combine(StoragePath, "Images", newFileName);

            File.Copy(path, tempPath);

            File.Move(tempPath, finalPath);

            return newFileName;
        }

        public static async void ExportItems(string collectionName)
        {
            File.Copy(Path.Combine(StoragePath, $"{collectionName}.txt"), Path.Combine(StoragePath, "Temp", $"items.txt"));

            Directory.CreateDirectory(Path.Combine(StoragePath, "Temp", "Sets"));
            File.Copy(Path.Combine(StoragePath, "Sets", "Sets.txt"), Path.Combine(StoragePath, "Temp", "Sets", "Sets.txt"));

            Directory.CreateDirectory(Path.Combine(StoragePath, "Temp", "Images"));
            ListImageNames(collectionName).ForEach(imageName =>
            {
                File.Copy(Path.Combine(StoragePath, "Images", imageName), Path.Combine(StoragePath, "Temp", "Images", imageName));
            });

            var result = await FolderPicker.Default.PickAsync(default);
            if (result.IsSuccessful)
            {
                ZipFile.CreateFromDirectory(Path.Combine(StoragePath, "Temp"), Path.Combine(result.Folder.Path, "exportData.zip"));
            }
            else
            {
                Page App = Microsoft.Maui.Controls.Application.Current.MainPage;
                await App.DisplayAlert("Validation Error", "Error picking folder", "OK");
            }
            Directory.Delete(Path.Combine(StoragePath, "Temp"), true);
            Directory.CreateDirectory(Path.Combine(StoragePath, "Temp"));
            //rem duplicate
        }

        public static async Task ImportItems(string collectionName)
        {
            FileResult? result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Please select a zip file",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { ".zip" } },
                    { DevicePlatform.iOS, new[] { ".zip" } },
                    { DevicePlatform.WinUI, new[] { ".zip" } },
                })
            });

            if (result != null)
            {
                string tempPath = Path.Combine(StoragePath, "Temp");

                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }
                Directory.CreateDirectory(tempPath);

                ZipFile.ExtractToDirectory(result.FullPath, tempPath);

                string[] files = Directory.GetFiles(Path.Combine(tempPath, "Images"));
                foreach (string fullPath in files)
                {
                    File.Copy(fullPath, Path.Combine(StoragePath, "Images", Path.GetFileName(fullPath)), true);
                }

                string contentToAppend = File.ReadAllText(Path.Combine(tempPath, $"items.txt"));
                File.AppendAllText(Path.Combine(StoragePath, $"{CurrentCollectionName}.txt"), contentToAppend);

                string contentToAppend2 = File.ReadAllText(Path.Combine(tempPath, "Sets", "Sets.txt"));
                File.AppendAllText(Path.Combine(StoragePath, "Sets", "Sets.txt"), contentToAppend2);

                //rem duplicate

                List<Set> sets = LoadSets();
                sets = Set.RemoveDuplicateSets(sets);

                using (StreamWriter sw = new StreamWriter(Path.Combine(StoragePath, "Sets", "Sets.txt"), false))
                {
                    foreach (var set in sets)
                    {
                        sw.Write(SerializeSet(set));
                    }
                }

                Directory.Delete(Path.Combine(StoragePath, "Temp"), true);
                Directory.CreateDirectory(Path.Combine(StoragePath, "Temp"));
            }
            else
            {
                Page App = Microsoft.Maui.Controls.Application.Current.MainPage;
                await App.DisplayAlert("Validation Error", "Error picking file", "OK");
            }
        }

        public static List<string> ListImageNames(string collectionName)
        {
            if (CurrentCollectionName == String.Empty) return new List<string>();
            return LoadItems(CurrentCollectionName).DistinctBy(item => item.ImageName).Select(i => i.ImageName).ToList();
        }

        public static int CountAllItems()
        {
            int count = 0;
            foreach (var file in Directory.GetFiles(StoragePath, "*.txt"))
            {
                if (file == Path.Combine(StoragePath, "collections.txt"))
                    continue;
                using (StreamReader sr = new StreamReader(file))
                {
                    string data = String.Empty;
                    string? line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        data += line + "\r\n";
                    }
                    count += DeserializeItems(data).Count;
                }
            }
            return count;
        }

        public static int CountCollections()
        {
            int count = 0;
            foreach (var file in Directory.GetFiles(StoragePath, "*.txt"))
            {
                if (file == Path.Combine(StoragePath, "collections.txt"))
                    continue;
                count++;
            }
            return count;
        }

        public static int CountCurrentCollectionItems()
        {
            if (CurrentCollectionName == String.Empty) return 0;
            return LoadItems(CurrentCollectionName).Count;
        }

        public static int CountCurrentCollectionSoldItems()
        {
            if (CurrentCollectionName == String.Empty) return 0;
            return LoadItems(CurrentCollectionName).Count(e => e.Sold);
        }

        public static int CountCurrentCollectionForSaleItems()
        {
            if (CurrentCollectionName == String.Empty) return 0;
            return LoadItems(CurrentCollectionName).Count(e => e.ToSale && !e.Sold);
        }

        public static int GetMaxCollectionId()
        {
            return LoadCollections().Count == 0 ? 0 : LoadCollections().Max(e => e.Id);
        }

        public static int GetMaxItemId(string collectionName)
        {
            return LoadItems(collectionName).Count == 0 ? 0 : LoadItems(collectionName).Max(e => e.Id);
        }
    }
}
