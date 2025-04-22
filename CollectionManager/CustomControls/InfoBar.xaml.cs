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

    public void UpdateCollectionInfo(int collectionCount, int allItemCount)
    {
        CollectionCountLabel.Text = $"Collections: {collectionCount}";
        AllItemsCountLabel.Text = $"All Items: {allItemCount}";
    }

    public void UpdateItemsInfo(int ItemCount, int SoldItems, int ForSale)
    {
        ItemCountLabel.Text = $"Items: {ItemCount}";
        SoldItemsCountLabel.Text = $"Sold Items: {SoldItems}";
        ForSaleItemsLabel.Text = $"For Sale: {ForSale}";
    }
} 