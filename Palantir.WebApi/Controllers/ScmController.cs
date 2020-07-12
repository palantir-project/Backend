namespace Palantir.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Palantir.Domain.Configurations;
    using Palantir.Exceptions;
    using Palantir.IO;

    [Route("admin/[controller]")]
    [ApiController]
    public class ScmController : ControllerBase
    {
        private readonly string configFilePath;

        public ScmController(IWebHostEnvironment environment)
        {
            this.configFilePath = environment.ContentRootPath + "/Properties/apiConfig.json";
        }

        // POST admin/scm
        [HttpPost]
        public async Task<IActionResult> Post(RestApiConfiguration configuration)
        {
            SourceControlManagerConfigurationWriter configurationWriter = new SourceControlManagerConfigurationWriter();

            try
            {
                await configurationWriter.WriteConfiguration(configuration, this.configFilePath);
            }
            catch (Exception exception)
            {
                if (exception is TargetInvocationException || exception is ArgumentNullException || exception is InvalidServiceNameException || exception is InvalidFieldException)
                {
                    return this.BadRequest(exception.Message);
                }
                else if (exception is MissingTokenException)
                {
                    return this.Unauthorized(exception.Message);
                }
            }

            return this.Ok();
        }

        // GET admin/scm
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string result = await IOHelper.ReadFile(this.configFilePath);

            List<WidgetConfiguration> configList = JsonConvert.DeserializeObject<List<WidgetConfiguration>>(result);
            WidgetConfiguration widgetConfiguration = null;

            foreach (WidgetConfiguration item in configList)
            {
                if (item.WidgetType == "scm")
                {
                    widgetConfiguration = item;
                }
            }

            return this.Ok(widgetConfiguration);
        }

        // PUT admin/scm
        [HttpPut]
        public async Task<IActionResult> Put(RestApiConfiguration configuration)
        {
            SourceControlManagerConfigurationWriter configurationWriter = new SourceControlManagerConfigurationWriter();

            try
            {
                await configurationWriter.UpdateConfiguration(configuration, this.configFilePath);
            }
            catch (Exception exception)
            {
                if (exception is TargetInvocationException || exception is ArgumentNullException || exception is InvalidServiceNameException || exception is InvalidFieldException)
                {
                    return this.BadRequest(exception.Message);
                }
                else if (exception is MissingTokenException)
                {
                    return this.Unauthorized(exception.Message);
                }
            }

            return this.Ok();
        }

        // DELETE admin/scm
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
                    if (item.WidgetType != "scm")
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