using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;

using Newtonsoft.Json;

using System.Diagnostics;
using System.Text;

using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;

namespace PelotonIDE.Presentation
{
    public sealed partial class IDEConfigPage : Page
    {
        public IDEConfigPage()
        {
            this.InitializeComponent();
        }

        private async void interpreterLocationBtn_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker open = new()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            open.FileTypeFilter.Add(".exe");

            // For Uno.WinUI-based apps
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App._window);
            WinRT.Interop.InitializeWithWindow.Initialize(open, hwnd);

            StorageFile pickedFile = await open.PickSingleFileAsync();
            if (pickedFile != null)
            {
                interpreterTextBox.Text = pickedFile.Path;
            }
        }

        private async void sourceDirectoryBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker folderPicker = new()
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary
            };
            folderPicker.FileTypeFilter.Add("*");

            // For Uno.WinUI-based apps
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App._window);
            WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd);

            StorageFolder pickedFolder = await folderPicker.PickSingleFolderAsync();
            if (pickedFolder != null)
            {
                sourceTextBox.Text = pickedFolder.Path;
            }
        }
    }
}