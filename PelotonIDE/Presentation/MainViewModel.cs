using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace PelotonIDE.Presentation
{
    public partial class MainViewModel : ObservableObject
    {
        public string? Title { get; }

        [ObservableProperty]
        private string? name;

        public ICommand GoToIDEConfig { get; }

        public MainViewModel(
            INavigator navigator,
            IStringLocalizer localizer)
        {
            _navigator = navigator;
            Title = $"Main - {localizer["ApplicationName"]}";
            GoToIDEConfig = new AsyncRelayCommand(GoToIDEConfigView);
        }

        private async Task GoToIDEConfigView()
        {
            await _navigator.NavigateViewModelAsync<IDEConfigViewModel>(this, data: new Entity(Name!));
        }

        private INavigator _navigator;
    }
}