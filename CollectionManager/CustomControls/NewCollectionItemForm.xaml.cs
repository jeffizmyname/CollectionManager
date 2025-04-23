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

    public CollectionItem? EditingItem { get; private set; }
    public bool IsEditMode => EditingItem != null;

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
            var item = EditingItem ?? new CollectionItem();

            item.Name = ItemNameEntry.Text;
            item.Description = ItemDescriptionEntry.Text;
            item.Price = float.Parse(ItemPriceEntry.Text);
            item.CollectionDate = CollectionDatePicker.Date;
            item.SellDate = SoldCheckBox.IsChecked ? SoldDatePicker.Date : null;
            item.Rating = int.Parse(ItemRatingEntry.Text);
            item.Sold = SoldCheckBox.IsChecked;
            item.ToSale = ForSaleCheckBox.IsChecked;
            item.ImageName = IsEditMode ? item.ImageName : CopyImage(ImagePath);
            item.CollectionName = CurrentCollectionName;

            CustomVariable customVariable = new CustomVariable();

            if (TypePicker.SelectedIndex == 2)
            {
                customVariable.Name = _SetViewModel.SelectedSet.Name;
                customVariable.StringValue = SetValuePicker.SelectedItem.ToString();
            }
            else
            {
                customVariable.Name = CustomVariableNameEntry.Text;
                if (int.TryParse(CustomVariableValueEntry.Text, out int intValue))
                    customVariable.IntValue = intValue;
                else
                    customVariable.StringValue = CustomVariableValueEntry.Text;
            }

            item.customVariable = customVariable;

            if (!IsEditMode && CollectionItem.HasDuplicate(Manager.LoadItems(Manager.CurrentCollectionName), item))
            {
                bool ans = await App.DisplayAlert("Validation Error", "This item might be a duplicate. Add it anyway?", "Yes", "No");
                if (!ans) return;
            }

            //cos ze albo tworzy albo edycja  idk
            CreateCollectionItemButtonClicked_Listener?.Invoke(sender, item);
            ClearForm();

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
            } else
            {
                if (TypePicker.SelectedIndex == 1) 
                {

                    if (!int.TryParse(CustomVariableValueEntry.Text, out int res))
                    {
                        await App.DisplayAlert("Validation Error", "Value must be an integer", "OK");
                        return false;
                    }
                }
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

    public void LoadItemForEditing(CollectionItem item)
    {
        EditingItem = item;

        ItemNameEntry.Text = item.Name;
        ItemDescriptionEntry.Text = item.Description;
        ItemPriceEntry.Text = item.Price.ToString();
        CollectionDatePicker.Date = item.CollectionDate;
        SoldCheckBox.IsChecked = item.Sold;
        SoldDatePicker.Date = item.SellDate ?? DateTime.Now;
        ForSaleCheckBox.IsChecked = item.ToSale;
        ItemRatingEntry.Text = item.Rating.ToString();
        ImagePath = Manager.GetImagePath(item.ImageName);
        ImageName = item.ImageName;

        if (item.customVariable != null)
        {
            if (!string.IsNullOrWhiteSpace(item.customVariable.Name) &&
                _SetViewModel.Sets.Any(s => s.Name == item.customVariable.Name))
            {
                TypePicker.SelectedIndex = 2; // "Set"
                _SetViewModel.SelectedSet = _SetViewModel.Sets.First(s => s.Name == item.customVariable.Name);
                SetValuePicker.SelectedItem = item.customVariable.StringValue;
            }
            else
            {
                TypePicker.SelectedIndex = 0; // "Custom"
                CustomVariableNameEntry.Text = item.customVariable.Name;
                if (item.customVariable.IntValue != null)
                    CustomVariableValueEntry.Text = item.customVariable.IntValue.ToString();
                else
                    CustomVariableValueEntry.Text = item.customVariable.StringValue;
            }
        }

        CreateCollectionButton.Text = "Save Item";
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

    public void ClearForm()
    {
        EditingItem = null;
        ItemNameEntry.Text = string.Empty;
        ItemDescriptionEntry.Text = string.Empty;
        ItemPriceEntry.Text = string.Empty;
        CollectionDatePicker.Date = DateTime.Today;
        SoldCheckBox.IsChecked = false;
        SoldDatePicker.Date = DateTime.Today;
        ForSaleCheckBox.IsChecked = false;
        ItemRatingEntry.Text = string.Empty;
        ImagePath = string.Empty;
        ImageName = string.Empty;
        CustomVariableNameEntry.Text = string.Empty;
        CustomVariableValueEntry.Text = string.Empty;
        TypePicker.SelectedIndex = 0;
    }

    public void ChangeTitle(string title)
    {
        TitleLabel.Text = title;
    }
}