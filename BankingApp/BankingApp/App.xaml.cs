using BankingApp.UI.Views;

namespace BankingApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell())
        {
            MinimumHeight = 800,
            MaximumHeight = 800,
            MinimumWidth = 650,
            MaximumWidth = 650
        };
    }
}