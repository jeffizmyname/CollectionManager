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
            CollectionListPage.newCollectionButtonClicked_Listener += Swap;
            FormPage.createCollectionButtonClicked_Listener +=
        }

        async void Swap(object sender, EventArgs e)
        {
            if(SidePanel.Content != null)
            {
                await SidePanel.Content.TranslateTo(SidePanel.Width+100, 0, 500);
                InfoBarPage.IsVisible = false;
                FormPage.IsVisible = true;
                await SidePanel.Content.TranslateTo(0, 0, 500);
                return;
            }
        }

    }

}
