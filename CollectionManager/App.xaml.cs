using static CollectionManager.Storage.Manager;

namespace CollectionManager
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InitializeStorage();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}