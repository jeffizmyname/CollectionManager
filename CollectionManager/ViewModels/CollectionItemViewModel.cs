using CollectionManager.Models;
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
