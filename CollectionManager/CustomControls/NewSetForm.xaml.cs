using CollectionManager.Models;
using CollectionManager.ViewModels;
using System.Collections.ObjectModel;

namespace CollectionManager.CustomControls;

public partial class NewSetForm : ContentView
{
    public event EventHandler<Set> SetCreated;
    public ObservableCollection<string> Options { get; set; } = new ObservableCollection<string>();
    public NewSetForm()
	{
		InitializeComponent();
        BindingContext = this;
    }

    private void OnAddOptionButton_Clicked(object sender, EventArgs e)
    {
        string option = SetValueEntry.Text?.Trim();
        if (!string.IsNullOrEmpty(option))
        {
            Options.Add(option);
            SetValueEntry.Text = string.Empty;
        }
    }

    private void OnDeleteOptionButton_Clicked(object sender, EventArgs e)
    {
        Button button = sender as Button;
        string optionToDelete = button?.BindingContext as string;

        if (optionToDelete != null && Options.Contains(optionToDelete))
        {
            Options.Remove(optionToDelete);
        }
    }

    private void CreateSetButton_Clicked(object sender, EventArgs e)
    {
        SetCreated.Invoke(sender, new Set
        (
            SetNameEntry.Text,
            Options.ToList()
        ));
        Options.Clear();
        SetNameEntry.Text = string.Empty;
        SetValueEntry.Text = string.Empty;
    }
}