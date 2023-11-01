using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FactorySettingsStructure = System.Collections.Generic.Dictionary<string, object>;

using Windows.Storage;

namespace PelotonIDE.Presentation
{
    public sealed partial class MainPage : Page
    {
        private static async Task<FactorySettingsStructure?> GetFactorySettings()
        {
            var globalSettings = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///PelotonIDE\\Presentation\\FactorySettings.json"));
            string globalSettingsString = File.ReadAllText(globalSettings.Path);
            return JsonConvert.DeserializeObject<FactorySettingsStructure>(globalSettingsString);
        }

        private T? GetFactorySettingsWithLocalSettingsOverrideOrDefault<T>(string name, T value, FactorySettingsStructure? factory, ApplicationDataContainer? container)
        {
            T? result = default;
            bool noFactory = false;
            bool noContainer = false;
            if (factory.ContainsKey(name))
            {
                result = (T)factory[name];
            }
            else
            {
                noFactory = true;
            }
            if (container.Values.ContainsKey(name))
            {
                result = (T)container.Values[name];
            }
            else
            {
                noContainer = true;
            }
            if (noFactory && noContainer)
            {
                result = value;
            }
            return result;
        }

        private int GetFactorySettingsWithLocalSettingsOverrideOrDefault(string name, int value, FactorySettingsStructure? factory, ApplicationDataContainer? container)
        {
            int result = default;
            bool noFactory = false;
            bool noContainer = false;
            if (factory.ContainsKey(name))
            {
                result = (int)factory[name];
            }
            else
            {
                noFactory = true;
            }
            if (container.Values.ContainsKey(name))
            {
                result = (int)container.Values[name];
            }
            else
            {
                noContainer = true;
            }
            if (noFactory && noContainer)
            {
                result = value;
            }
            return result;
        }
        private long GetFactorySettingsWithLocalSettingsOverrideOrDefault(string name, long value, FactorySettingsStructure? factory, ApplicationDataContainer? container)
        {
            long result = default;
            bool noFactory = false;
            bool noContainer = false;
            if (factory.ContainsKey(name))
            {
                result = (long)factory[name];
            }
            else
            {
                noFactory = true;
            }
            if (container.Values.ContainsKey(name))
            {
                result = (long)container.Values[name];
            }
            else
            {
                noContainer = true;
            }
            if (noFactory && noContainer)
            {
                result = value;
            }
            return result;
        }
        private bool GetFactorySettingsWithLocalSettingsOverrideOrDefault(string name, bool value, FactorySettingsStructure? factory, ApplicationDataContainer? container)
        {
            bool result = default;
            bool noFactory = false;
            bool noContainer = false;
            if (factory.ContainsKey(name))
            {
                result = (bool)factory[name];
            }
            else
            {
                noFactory = true;
            }
            if (container.Values.ContainsKey(name))
            {
                result = (bool)container.Values[name];
            }
            else
            {
                noContainer = true;
            }
            if (noFactory && noContainer)
            {
                result = value;
            }
            return result;
        }

        private OutputPanelPosition GetFactorySettingsWithLocalSettingsOverrideOrDefault(string name, OutputPanelPosition value, FactorySettingsStructure? factory, ApplicationDataContainer? container)
        {
            OutputPanelPosition result = default;
            bool noFactory = false;
            bool noContainer = false;
            if (factory.ContainsKey(name))
            {
                result = (OutputPanelPosition)Enum.Parse(typeof(OutputPanelPosition), (string)factory[name]);
            }
            else
            {
                noFactory = true;
            }
            if (container.Values.ContainsKey(name))
            {
                result = (OutputPanelPosition)Enum.Parse(typeof(OutputPanelPosition), (string)container.Values[name]);
            }
            else
            {
                noContainer = true;
            }
            if (noFactory && noContainer)
            {
                result = value;
            }
            return result;
        }

        private string? GetFactorySettingsWithLocalSettingsOverrideOrDefault(string name, string value, FactorySettingsStructure? factory, ApplicationDataContainer? container)
        {
            string? result = default;
            bool noFactory = false;
            bool noContainer = false;
            if (factory.ContainsKey(name))
            {
                result = (string)factory[name];
            }
            else
            {
                noFactory = true;
            }
            if (container.Values.ContainsKey(name))
            {
                result = (string)container.Values[name];
            }
            else
            {
                noContainer = true;
            }
            if (noFactory && noContainer)
            {
                result = value;
            }
            return result;
        }

    }
}
