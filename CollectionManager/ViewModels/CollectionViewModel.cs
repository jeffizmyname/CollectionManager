﻿using CollectionManager.Models;
using CollectionManager.Storage;
using System.Collections.ObjectModel;
using static CollectionManager.Storage.Manager;


namespace CollectionManager.ViewModels
{
    public class CollectionViewModel
    {
        public ObservableCollection<Collection> Collections { get; set; } =  new ObservableCollection<Collection>();

        public CollectionViewModel()
        {
            Collections = new ObservableCollection<Collection>(LoadCollections());
        }

        public void AddCollection(Collection collection)
        {
            collection.Id = Manager.GetMaxCollectionId() + 1;
            Collections.Add(collection);
            SaveCollection(collection);
        }

    }
}
