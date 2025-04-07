using CollectionManager.Models;
using CollectionManager.ViewModels;


namespace CollectionManager.CustomControls;

public partial class CollectionList : ContentView
{
    public event EventHandler? newCollectionButtonClicked_Listener;
    public CollectionViewModel? _collectionViewModel { get; set; }
    public CollectionList()
    {
        InitializeComponent();
        _collectionViewModel = new CollectionViewModel();
        CollectionListView.BindingContext = _collectionViewModel;
    }


    private async void CollectionListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count > 0)
        {
            Collection selectedItem = e.CurrentSelection[0] as Collection;
            if (selectedItem != null)
            {
                await TitleLabel.TranslateTo(0, -100, 500);
                TitleLabel.Text = selectedItem.Name;
                await TitleLabel.TranslateTo(0, 0, 2000);
                //Navigation.PushAsync(new CollectionDetailPage(selectedItem));
            }
        }

    }

    private void NewCollectionButton_Clicked(object sender, EventArgs e)
    {
        newCollectionButtonClicked_Listener.Invoke(e, e);
    }
}
