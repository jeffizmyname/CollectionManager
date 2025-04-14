using CollectionManager.CustomControls;
using CollectionManager.Helpers;
using CollectionManager.Models;
using System.Diagnostics;
using static CollectionManager.Storage.Manager;

namespace CollectionManager
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            CollectionListPage.newButtonClicked_Listener += (e, data) => Swap(data);
            CollectionListPage.pageChanged_Listener += (e, data) => Reset(data);
            FormCollectionPage.createCollectionButtonClicked_Listener += (s, data) => CollectionListPage._collectionViewModel.AddCollection(new Collection(data[0], data[1]));
        }

        async void Reset(string data)
        {
            if (SidePanel.Content != null)
            {
                await SidePanel.Content.TranslateTo(SidePanel.Width + 100, 0, 500);

                if (data == "CollectionList")
                {
                    InfoBarPage.IsVisible = true;
                    FormCollectionPage.IsVisible = false;
                    FormItemPage.IsVisible = false;
                }
                else if (data == "CollectionItemList")
                {
                    InfoBarPage.IsVisible = true;
                    FormCollectionPage.IsVisible = false;
                    FormItemPage.IsVisible = false;
                }
                InfoBarPage.SetContent(data);

                await SidePanel.Content.TranslateTo(0, 0, 500);
                return;
            }
        }

        async void Swap(string data)
        {
            if(SidePanel.Content != null)
            {
                await SidePanel.Content.TranslateTo(SidePanel.Width + 100, 0, 500);

                if (data == "CollectionList")
                {
                    InfoBarPage.IsVisible = false;
                    FormCollectionPage.IsVisible = true;
                    FormItemPage.IsVisible = false;
                }
                else if(data == "CollectionItemList")
                {
                    InfoBarPage.IsVisible = false;
                    FormCollectionPage.IsVisible = false;
                    FormItemPage.IsVisible = true;
                }
                

                await SidePanel.Content.TranslateTo(0, 0, 500);
                return;
            }
        }

    }

}
