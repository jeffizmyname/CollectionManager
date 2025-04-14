namespace CollectionManager.CustomControls;

public partial class InfoBar : ContentView
{
    public InfoBar()
    {
        InitializeComponent();
    }

    public void SetContent(string page)
    {
        if (page == "CollectionList")
        {
            CollectionsInfo.IsVisible = true;
            CollectionInfo.IsVisible = false;
        }
        else if (page == "CollectionItemList")
        {
            CollectionsInfo.IsVisible = false;
            CollectionInfo.IsVisible = true;
        }
    }
}