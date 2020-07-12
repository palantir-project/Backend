namespace Palantir.IO
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Palantir.Domain.Configurations;

    public class IframeConfigurationWriter
    {
        public async Task WriteConfiguration(RestApiConfiguration configuration, string path)
        {
            string result = await IOHelper.ReadFile(path);
            List<WidgetConfiguration> currentConfigList = JsonConvert.DeserializeObject<List<WidgetConfiguration>>(result);

            if (currentConfigList == null || !currentConfigList.Any(item => item.WidgetType == "iframe"))
            {
                List<WidgetConfiguration> updatedConfigList = new List<WidgetConfiguration>();
                WidgetConfiguration config = new WidgetConfiguration()
                {
                    WidgetType = "iframe",
                    ServiceName = configuration.Html,
                };
                updatedConfigList.Add(config);

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
            string result = await IOHelper.ReadFile(path);
            List<WidgetConfiguration> currentConfigList = JsonConvert.DeserializeObject<List<WidgetConfiguration>>(result);

            if (currentConfigList != null && currentConfigList.Any(item => item.WidgetType == "iframe"))
            {
                List<WidgetConfiguration> updatedConfigList = new List<WidgetConfiguration>();
                WidgetConfiguration config = new WidgetConfiguration()
                {
                    WidgetType = "iframe",
                    ServiceName = configuration.Html,
                };
                updatedConfigList.Add(config);

                if (currentConfigList != null)
                {
                    foreach (WidgetConfiguration item in currentConfigList)
                    {
                        if (item.WidgetType != "iframe")
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