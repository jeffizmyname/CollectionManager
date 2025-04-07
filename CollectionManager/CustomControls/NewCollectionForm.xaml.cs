namespace CollectionManager.CustomControls;

public partial class NewCollectionForm : ContentView
{
    public event EventHandler<string>? createCollectionButtonClicked_Listener;

    public NewCollectionForm()
	{
		InitializeComponent();
	}

    private void CreateCollectionButton_Clicked(object sender, EventArgs e)
    {
		if(!string.IsNullOrEmpty(CollectionNameEntry.Text) && !string.IsNullOrEmpty(CollectionDescriptionEntry.Text))
		{
            createCollectionButtonClicked_Listener.Invoke(sender, new List<string> { CollectionNameEntry.Text, CollectionDescriptionEntry.Text });
        }
    }
}