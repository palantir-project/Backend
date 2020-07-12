namespace Palantir.WebApi.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Palantir.Domain.Configurations;
    using Palantir.IO;

    [Route("admin/[controller]")]
    [ApiController]
    public class IframeController : ControllerBase
    {
        private readonly string configFilePath;

        public IframeController(IWebHostEnvironment environment)
        {
            this.configFilePath = environment.ContentRootPath + "/Properties/apiConfig.json";
        }

        // POST admin/iframe
        [HttpPost]
        public async Task<IActionResult> Post(RestApiConfiguration configuration)
        {
            IframeConfigurationWriter configurationWriter = new IframeConfigurationWriter();
            await configurationWriter.WriteConfiguration(configuration, this.configFilePath);

            return this.Ok();
        }

        // GET admin/iframe
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string result = await IOHelper.ReadFile(this.configFilePath);

            List<WidgetConfiguration> configList = JsonConvert.DeserializeObject<List<WidgetConfiguration>>(result);
            WidgetConfiguration iframeConfiguration = null;

            foreach (WidgetConfiguration item in configList)
            {
                if (item.WidgetType == "iframe")
                {
                    iframeConfiguration = item;
                }
            }

            return this.Ok(iframeConfiguration);
        }

        // PUT admin/iframe
        [HttpPut]
        public async Task<IActionResult> Put(RestApiConfiguration configuration)
        {
            IframeConfigurationWriter configurationWriter = new IframeConfigurationWriter();
            await configurationWriter.UpdateConfiguration(configuration, this.configFilePath);

            return this.Ok();
        }

        // DELETE admin/iframe
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            string result = await IOHelper.ReadFile(this.configFilePath);
            List<WidgetConfiguration> configList = JsonConvert.DeserializeObject<List<WidgetConfiguration>>(result);
            List<WidgetConfiguration> newConfigList = new List<WidgetConfiguration>();

            if (configList != null)
            {
                foreach (WidgetConfiguration item in configList)
                {
                    if (item.WidgetType != "iframe")
                    {
                        newConfigList.Add(item);
                    }
                }
            }

            string newConfigJson = JsonConvert.SerializeObject(newConfigList);
            await IOHelper.WriteFile(this.configFilePath, newConfigJson);

            return this.Ok();
        }
    }
}
