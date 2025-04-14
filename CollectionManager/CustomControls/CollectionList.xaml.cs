using CollectionManager.Models;
using CollectionManager.ViewModels;
using Microsoft.Maui.Controls.Handlers.Items;


namespace CollectionManager.CustomControls;

public partial class CollectionList : ContentView
{
    public event EventHandler<string>? newButtonClicked_Listener;
    public event EventHandler<string>? pageChanged_Listener;
    public CollectionViewModel? _collectionViewModel { get; set; }
    public CollectionItemViewModel? _collectionItemViewModel { get; set; }

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

    private void AddNewButton_Clicked(object sender, EventArgs e)
    {
        newButtonClicked_Listener.Invoke(e, currentPage);
    }

    private async void BackButton_Tapped(object sender, TappedEventArgs e)
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

        currentPage = "CollectionList";
        pageChanged_Listener.Invoke(e, currentPage);

        await Task.WhenAll(
            TitleLabel.TranslateTo(0, 0, 2000),
            BackButton.TranslateTo(0, 0, 2000)
        );
    }
}
