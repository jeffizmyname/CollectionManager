using CollectionManager.Models;
using CollectionManager.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CollectionManager.Storage.Manager;

namespace CollectionManager.ViewModels
{
    public class CollectionItemViewModel
    {
        public ObservableCollection<CollectionItem> Items { get; set; }

        public CollectionItemViewModel()
        {
            Items = new ObservableCollection<CollectionItem>();
        }

        public void SetCollection(string collectionName)
        {
            Items.Clear();
            foreach (var item in LoadItems(collectionName))
            {
                Items.Add(item);
            }
            PutSoldAtEnd();
        }

        public void AddItem(CollectionItem item)
        {
            item.Id = Manager.GetMaxItemId(item.CollectionName) + 1;
            Items.Add(item);
            SaveItem(item);
            PutSoldAtEnd();
        }

        public void EditItem(CollectionItem item)
        {
            var index = Items.ToList().FindIndex(i => i.Id == item.Id);
            if (index != -1)
            {
                Items.RemoveAt(index);
                Items.Insert(index, item);
                Manager.UpdateItems(Items.ToList(), CurrentCollectionName);
            }
            PutSoldAtEnd();
        }

        public void RemoveItem(CollectionItem item)
        {
            var index = Items.ToList().FindIndex(i => i.Id == item.Id);
            if (index != -1)
            {
                Items.RemoveAt(index);
                Manager.UpdateItems(Items.ToList(), CurrentCollectionName);
            }
            PutSoldAtEnd();
        }

        public void PutSoldAtEnd()
        {
            var items = Items.ToList();
            foreach (var item in items)
            {
                if (item.Sold)
                {
                    Items.Remove(item);
                    Items.Add(item);
                }
            }
        }
    }
}
