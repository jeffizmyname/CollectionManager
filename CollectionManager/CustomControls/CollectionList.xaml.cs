using CollectionManager.Models;
using CollectionManager.ViewModels;
using Microsoft.Maui.Controls.Handlers.Items;
using CollectionManager.Storage;
using System.Diagnostics;

namespace CollectionManager.CustomControls;

public partial class CollectionList : ContentView
{
    public event EventHandler<string>? newButtonClicked_Listener;
    public event EventHandler<string>? pageChanged_Listener;
    public event EventHandler<CollectionItem>? editButtonCliked_Listener;
    public CollectionViewModel? _collectionViewModel { get; set; }
    public CollectionItemViewModel? _collectionItemViewModel { get; set; }

    public CollectionItem? SelectedItem { get; set; } = null;
    public string currentPage { get; set; } = "CollectionList";
    public CollectionList()
    {
        InitializeComponent();
        _collectionViewModel = new CollectionViewModel();
        _collectionItemViewModel = new CollectionItemViewModel();
        CollectionListView.BindingContext = _collectionViewModel;
        CollectionItemsListView.BindingContext = _collectionItemViewModel;
    }


    private async void CollectionListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            Collection selectedItem = e.CurrentSelection[0] as Collection;
            if (selectedItem != null)
            {
                await Task.WhenAll(
                    TitleLabel.TranslateTo(0, -100, 500),
                    BackButton.TranslateTo(0, -100, 500)
                );

                TitleLabel.Text = selectedItem.Name;

                AddNewButton.Text = "New Item";

                _collectionItemViewModel.SetCollection(selectedItem.Name);

                BackButton.IsVisible = true;
                CollectionListView.IsVisible = false;
                CollectionItemsListView.IsVisible = true;
                ItemDetailsPage.IsVisible = false;
                ExportButton.IsVisible = true;
                ImportButton.IsVisible = true;

                Manager.CurrentCollectionName = selectedItem.Name;

                currentPage = "CollectionItemList";
                pageChanged_Listener.Invoke(e, currentPage);

                await Task.WhenAll(
                    TitleLabel.TranslateTo(0, 0, 2000),
                    BackButton.TranslateTo(0, 0, 2000)
                );


            }
            ((CollectionView)sender).SelectedItem = null;
        }

    }
    private void CollectionItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            var selectedItem = e.CurrentSelection[0] as CollectionItem;
            if (selectedItem != null)
            {
                Debug.WriteLine($"Clicked item");
                var template = (DataTemplate)this.Resources["ItemPageTemplate"];
                var content = (View)template.CreateContent();
                content.BindingContext = selectedItem;
                SelectedItem = selectedItem;

                AddNewButton.Text = "Delete Item";

                ItemDetailsPage.Content = content;
                ItemDetailsPage.IsVisible = true;
                CollectionListView.IsVisible = false;
                CollectionItemsListView.IsVisible = false;
                ExportButton.IsVisible = false;
                ImportButton.IsVisible = false;
                EditButton.IsVisible = true;

                currentPage = "ItemDetailsPage";
                pageChanged_Listener?.Invoke(e, currentPage);
            }

            ((CollectionView)sender).SelectedItem = null;
            Debug.WriteLine($"sel item {((CollectionView)sender).SelectedItem}");
        }
    }

    private void AddNewButton_Clicked(object sender, EventArgs e)
    {
        if(currentPage == "ItemDetailsPage")
        {
            _collectionItemViewModel.RemoveItem(SelectedItem);
            BackButton_Tapped(sender, null);
            return;
        }
        newButtonClicked_Listener.Invoke(e, currentPage);
    }

    private async void BackButton_Tapped(object sender, TappedEventArgs e)
    {
        if (currentPage == "ItemDetailsPage")
        {
            AddNewButton.Text = "New Item";

            ItemDetailsPage.IsVisible = false;
            CollectionItemsListView.IsVisible = true;
            ExportButton.IsVisible = false;
            ImportButton.IsVisible = false;
            EditButton.IsVisible = false;

            Debug.WriteLine($"sel item {CollectionItemsListView.SelectedItem}");


            currentPage = "CollectionItemList";
            pageChanged_Listener?.Invoke(e, currentPage);
            CollectionItemsListView.SelectedItem = null;
            return;
        }

        if (currentPage == "CollectionItemList")
        {
            await Task.WhenAll(
                TitleLabel.TranslateTo(0, -100, 500),
                BackButton.TranslateTo(0, -100, 500)
            );

            TitleLabel.Text = "Your Collections";
            AddNewButton.Text = "New Collection";

            BackButton.IsVisible = false;
            CollectionListView.IsVisible = true;
            CollectionItemsListView.IsVisible = false;
            ItemDetailsPage.IsVisible = false;
            ExportButton.IsVisible = false;
            ImportButton.IsVisible = false; 
            EditButton.IsVisible = false;

            currentPage = "CollectionList";
            pageChanged_Listener?.Invoke(e, currentPage);

            await Task.WhenAll(
                TitleLabel.TranslateTo(0, 0, 2000),
                BackButton.TranslateTo(0, 0, 2000)
            );
        }
    }

    private async void ImportButton_Clicked(object sender, EventArgs e)
    {
        await Manager.ImportItems(Manager.CurrentCollectionName);
        _collectionItemViewModel.SetCollection(Manager.CurrentCollectionName);
        pageChanged_Listener?.Invoke(e, currentPage);
    }

    private void ExportButton_Clicked(object sender, EventArgs e)
    {
        Manager.ExportItems(Manager.CurrentCollectionName);
    }

    private void EditButton_Clicked(object sender, EventArgs e)
    {
        editButtonCliked_Listener.Invoke(sender, SelectedItem);
    }
}
