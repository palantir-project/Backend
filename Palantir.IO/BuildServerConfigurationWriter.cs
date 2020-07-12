namespace Palantir.IO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Palantir.Domain.Adapters;
    using Palantir.Domain.Configurations;

    public class BuildServerConfigurationWriter
    {
        private WidgetConfiguration widgetConfiguration;

        public async Task WriteConfiguration(RestApiConfiguration configuration, string path)
        {
            string fullQualifiedName = $"{configuration.Service}.WidgetConfigurationAdapter, {configuration.Service}";
            IWidgetConfigurationAdapter widgetConfigurationAdapter = (IWidgetConfigurationAdapter)Activator.CreateInstance(Type.GetType(fullQualifiedName), configuration);
            widgetConfigurationAdapter.ValidateWidgetConfiguration(configuration);

            string result = await IOHelper.ReadFile(path);
            List<WidgetConfiguration> currentConfigList = JsonConvert.DeserializeObject<List<WidgetConfiguration>>(result);

            if (currentConfigList == null || !currentConfigList.Any(item => item.WidgetType == "bs"))
            {
                this.widgetConfiguration = widgetConfigurationAdapter.GetWidgetConfiguration();

                List<WidgetConfiguration> updatedConfigList = new List<WidgetConfiguration>
                {
                    this.widgetConfiguration,
                };

                if (currentConfigList != null)
                {
                    updatedConfigList.AddRange(currentConfigList);
                }

                string configJson = JsonConvert.SerializeObject(updatedConfigList);
                await IOHelper.WriteFile(path, configJson);
            }
        }

        public async Task UpdateConfiguration(RestApiConfiguration configuration, string path)
        {
            string fullQualifiedName = $"{configuration.Service}.WidgetConfigurationAdapter, {configuration.Service}";
            IWidgetConfigurationAdapter widgetConfigurationAdapter = (IWidgetConfigurationAdapter)Activator.CreateInstance(Type.GetType(fullQualifiedName), configuration);
            widgetConfigurationAdapter.ValidateWidgetConfiguration(configuration);

            string result = await IOHelper.ReadFile(path);
            List<WidgetConfiguration> currentConfigList = JsonConvert.DeserializeObject<List<WidgetConfiguration>>(result);

            if (currentConfigList != null && currentConfigList.Any(item => item.WidgetType == "bs"))
            {
                this.widgetConfiguration = widgetConfigurationAdapter.GetWidgetConfiguration();

                List<WidgetConfiguration> updatedConfigList = new List<WidgetConfiguration>
                {
                    this.widgetConfiguration,
                };

                if (currentConfigList != null)
                {
                    foreach (WidgetConfiguration item in currentConfigList)
                    {
                        if (item.WidgetType != "bs")
                        {
                            updatedConfigList.Add(item);
                        }
                    }
                }

                string configJson = JsonConvert.SerializeObject(updatedConfigList);
                await IOHelper.WriteFile(path, configJson);
            }
        }
    }
}