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

        private T? GetFactorySettingsWithLocalSettingsOverrideOrDefault<T>(string name, T otherwise,
                                                                           FactorySettingsStructure? factory,
                                                                           ApplicationDataContainer? container)
        {
            T? result = default;
            bool noFactory = false;
            bool noContainer = false;
            if (factory.TryGetValue(name, out object? valu))
            {
                result = (T)valu;
            }
            else
            {
                noFactory = true;
            }
            if (container.Values.TryGetValue(name, out object? val))
            {
                result = (T)val;
            }
            else
            {
                noContainer = true;
            }
            if (noFactory && noContainer)
            {
                result = otherwise;
            }
            container.Values[name] = result;
            return result;
        }

        private int GetFactorySettingsWithLocalSettingsOverrideOrDefault(string name, int otherwise, FactorySettingsStructure? factory, ApplicationDataContainer? container)
        {
            int result = default;
            bool noFactory = false;
            bool noContainer = false;
            if (factory.TryGetValue(name, out object? valu))
            {
                result = (int)valu;
            }
            else
            {
                noFactory = true;
            }
            if (container.Values.TryGetValue(name, out object? val))
            {
                result = (int)val;
            }
            else
            {
                noContainer = true;
            }
            if (noFactory && noContainer)
            {
                result = otherwise;
            }
            container.Values[name] = result;
            return result;
        }
        private long GetFactorySettingsWithLocalSettingsOverrideOrDefault(string name, long otherwise, FactorySettingsStructure? factory, ApplicationDataContainer? container)
        {
            long result = default;
            bool noFactory = false;
            bool noContainer = false;
            if (factory.TryGetValue(name, out object? valu))
            {
                result = (long)valu;
            }
            else
            {
                noFactory = true;
            }
            if (container.Values.TryGetValue(name, out object? val))
            {
                result = (long)val;
            }
            else
            {
                noContainer = true;
            }
            if (noFactory && noContainer)
            {
                result = otherwise;
            }
            container.Values[name] = result;
            return result;
        }
        private bool GetFactorySettingsWithLocalSettingsOverrideOrDefault(string name, bool otherwise, FactorySettingsStructure? factory, ApplicationDataContainer? container)
        {
            bool result = default;
            bool noFactory = false;
            bool noContainer = false;
            if (factory.TryGetValue(name, out object? valu))
            {
                result = (bool)valu;
            }
            else
            {
                noFactory = true;
            }
            if (container.Values.TryGetValue(name, out object? val))
            {
                result = (bool)val;
            }
            else
            {
                noContainer = true;
            }
            if (noFactory && noContainer)
            {
                result = otherwise;
            }
            container.Values[name] = result;
            return result;
        }

        private OutputPanelPosition GetFactorySettingsWithLocalSettingsOverrideOrDefault(string name, OutputPanelPosition otherwise, FactorySettingsStructure? factory, ApplicationDataContainer? container)
        {
            OutputPanelPosition result = default;
            bool noFactory = false;
            bool noContainer = false;
            if (factory.TryGetValue(name, out object? valu))
            {
                result = (OutputPanelPosition)Enum.Parse(typeof(OutputPanelPosition), (string)valu);
            }
            else
            {
                noFactory = true;
            }
            if (container.Values.TryGetValue(name, out object? val))
            {
                result = (OutputPanelPosition)Enum.Parse(typeof(OutputPanelPosition), (string)val);
            }
            else
            {
                noContainer = true;
            }
            if (noFactory && noContainer)
            {
                result = otherwise;
            }
            container.Values[name] = result.ToString();
            return result;
        }

        private string? GetFactorySettingsWithLocalSettingsOverrideOrDefault(string name, string otherwise, FactorySettingsStructure? factory, ApplicationDataContainer? container)
        {
            string? result = default;
            bool noFactory = false;
            bool noContainer = false;
            if (factory.TryGetValue(name, out object? valu))
            {
                result = (string)valu;
            }
            else
            {
                noFactory = true;
            }
            if (container.Values.TryGetValue(name, out object? val))
            {
                result = (string)val;
            }
            else
            {
                noContainer = true;
            }
            if (noFactory && noContainer)
            {
                result = otherwise;
            }
            container.Values[name] = result;
            return result;
        }
    }
}
