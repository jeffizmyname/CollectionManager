using CollectionManager.Helpers;
using CollectionManager.Models;
using CollectionManager.Storage;
using CollectionManager.ViewModels;
using Microsoft.Win32;
using System.Collections;
using System.Diagnostics;
using static CollectionManager.Storage.Manager;

namespace CollectionManager.CustomControls;

public partial class NewCollectionItemForm : ContentView
{
    public event EventHandler<CollectionItem>? CreateCollectionItemButtonClicked_Listener;
    public event EventHandler<string>? OpenNewSetButtonClicked_Listener;
    public string ImagePath { get; set; } = string.Empty;
    public string ImageName { get; set; } = string.Empty;

    Page App = Application.Current.MainPage;

    public SetViewModel _SetViewModel { get; set; }
    public NewCollectionItemForm()
	{
		InitializeComponent();
        _SetViewModel = new SetViewModel();
        SetPicker.BindingContext = _SetViewModel;
        SetValuePicker.BindingContext = _SetViewModel;
        TypePicker.SelectedIndex = 0;
    }

    private async void CreateCollectionButton_Clicked(object sender, EventArgs e)
    {
        if (await Validate())
        {
            CollectionItem Item = new CollectionItem
            {
                Name = ItemNameEntry.Text,
                Description = ItemDescriptionEntry.Text,
                Price = float.Parse(ItemPriceEntry.Text),
                CollectionDate = CollectionDatePicker.Date,
                SellDate = SoldCheckBox.IsChecked ? SoldDatePicker.Date : null,
                Rating = int.Parse(ItemRatingEntry.Text),
                Sold = SoldCheckBox.IsChecked,
                ToSale = ForSaleCheckBox.IsChecked,
                ImageName = CopyImage(ImagePath),
                CollectionName = CurrentCollectionName
            };

            CustomVariable customVariable = new CustomVariable();            

            if (TypePicker.SelectedIndex == 2)
            {
                customVariable.Name = _SetViewModel.SelectedSet.Name;
                customVariable.StringValue = SetValuePicker.SelectedItem.ToString();

            } else
            {
                customVariable.Name = CustomVariableNameEntry.Text;
                if (int.TryParse(CustomVariableValueEntry.Text, out int intValue))
                {
                    customVariable.IntValue = intValue;
                }
                else
                {
                    customVariable.StringValue = CustomVariableValueEntry.Text;
                }
            }

            Item.customVariable = customVariable;
            Debug.WriteLine($"Item: {Item.ToDisplayString()}");

            if(CollectionItem.HasDuplicate(Manager.LoadItems(Manager.CurrentCollectionName), Item))
            {
                bool ans = await App.DisplayAlert("Validation Error", "This item might be duplicate do you want to add It?", "Yes", "No");
                if (!ans)
                {
                    return;
                }
            }

            CreateCollectionItemButtonClicked_Listener.Invoke(sender, Item);
        }
    }

	private async Task<bool> Validate()
	{
        if (App == null) return false;

        // Name validation
        if (string.IsNullOrWhiteSpace(ItemNameEntry.Text))
        {
            await App.DisplayAlert("Validation Error", "Item name is required.", "OK");
            return false;
        }

        // Price validation
        if (!float.TryParse(ItemPriceEntry.Text, out float price) || price < 0)
        {
            await App.DisplayAlert("Validation Error", "Price must be a valid non-negative number.", "OK");
            return false;
        }

        // Rating validation
        if (!string.IsNullOrWhiteSpace(ItemRatingEntry.Text))
        {
            if (!int.TryParse(ItemRatingEntry.Text, out int rating))
            {
                await App.DisplayAlert("Validation Error", "Rating must be a valid number.", "OK");
                return false;
            }

            if (rating < 0 || rating > 10)
            {
                await App.DisplayAlert("Validation Error", "Rating must be between 0 and 10.", "OK");
                return false;
            }
        } else
        {
            await App.DisplayAlert("Validation Error", "Rating mustnt be emmpty", "OK");
            return false;
        }

        if (TypePicker.SelectedIndex == 2)
        {
            //set name
            if(SetPicker.SelectedItem == null)
            {
                await App.DisplayAlert("Validation Error", "Custom set name is required.", "OK");
                return false;
            }


            //set value
            if (SetValuePicker.SelectedItem == null)
            {
                await App.DisplayAlert("Validation Error", "Custom set value is required.", "OK");
                return false;
            }

        } else
        {

            // Custom variable name
            if (string.IsNullOrWhiteSpace(CustomVariableNameEntry.Text))
            {
                await App.DisplayAlert("Validation Error", "Custom variable name is required.", "OK");
                return false;
            }

            // Custom variable value 
            if (string.IsNullOrWhiteSpace(CustomVariableValueEntry.Text))
            {
                await App.DisplayAlert("Validation Error", "Custom variable value is required.", "OK");
                return false;
            }
        }

        //require sell date
        if (SoldCheckBox.IsChecked && SoldDatePicker.Date == null)
        {
            await App.DisplayAlert("Validation Error", "Please select a sell date.", "OK");
            return false;
        }

        //sold newer than coollection
        if (SoldCheckBox.IsChecked && SoldDatePicker.Date < CollectionDatePicker.Date)
        {
            await App.DisplayAlert("Validation Error", "Sold date must be newer than collection date", "OK");
            return false;
        }

        //image
        if(ImageName == string.Empty)
        {
            await App.DisplayAlert("Validation Error", "Please select an image.", "OK");
            return false;
        }

        return true;
    }

    private void SoldCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if(e.Value)
        {
            ForSaleCheckBox.IsEnabled = false;
            ForSaleCheckBox.IsChecked = true;
            SellContainer.IsVisible = true;
        }
        else
        {
            SellContainer.IsVisible = false;
            ForSaleCheckBox.IsEnabled = true;
        }
    }

    private async Task<FileResult> PickImage()
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Please select an image",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                return result;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error picking image: {ex.Message}");
        }

        return null;
    }

    private async void ImagePickerButton_Clicked(object sender, EventArgs e)
    {
        FileResult? file = await PickImage();
        if (file != null)
        {
            ImagePath = file.FullPath;
            ImageName = file.FileName;
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Failed to pick image.", "OK");
        }
    }

    private void CreateNewSet_Clicked(object sender, EventArgs e)
    {
        OpenNewSetButtonClicked_Listener.Invoke(sender, "NewSet");
    }

    private void TypePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(sender is Picker picker)
        {
            if (picker.SelectedItem != null)
            {
                if (picker.SelectedItem.ToString() == "Custom List")
                {
                    SetOptions.IsVisible = true;
                    ValueOptions.IsVisible = false;
                }
                else
                {
                    SetOptions.IsVisible = false;
                    ValueOptions.IsVisible = true;
                }
            }
        }
    }
}