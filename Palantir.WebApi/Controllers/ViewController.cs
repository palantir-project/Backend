namespace Palantir.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using Palantir.Domain.Configurations;
    using Palantir.Domain.Models;
    using Palantir.Domain.Services;
    using Palantir.IO;

    [Route("[controller]")]
    [ApiController]
    public class ViewController : ControllerBase
    {
        private readonly string configFilePath;

        public ViewController(IWebHostEnvironment environment)
        {
            this.configFilePath = environment.ContentRootPath + "/Properties/apiConfig.json";
        }

        // GET view/scm
        [HttpGet("scm")]
        public async Task<IActionResult> SourceControlManager()
        {
            IMergeRequestService fetchingService = null;

            string configJson = await IOHelper.ReadFile(this.configFilePath);
            List<WidgetConfiguration> widgetConfiguration = JsonConvert.DeserializeObject<List<WidgetConfiguration>>(configJson);

            if (widgetConfiguration != null && widgetConfiguration.Count > 0)
            {
                foreach (WidgetConfiguration item in widgetConfiguration)
                {
                    if (item.WidgetType == "scm")
                    {
                        object[] args = new object[] { item.RestApiUrls, item.RestApiHeader, };
                        fetchingService = (IMergeRequestService)Activator.CreateInstance(Type.GetType(item.ServiceName), args);
                    }
                }
            }

            List<MergeRequest> result = new List<MergeRequest>();
            if (fetchingService != null)
            {
                result = fetchingService.GetMergeRequests();
            }

            return this.Ok(result);
        }

        // GET view/bs
        [HttpGet("bs")]
        public async Task<IActionResult> BuildServer()
        {
            IBuildService fetchingService = null;

            string configJson = await IOHelper.ReadFile(this.configFilePath);
            List<WidgetConfiguration> widgetConfiguration = JsonConvert.DeserializeObject<List<WidgetConfiguration>>(configJson);

            if (widgetConfiguration != null && widgetConfiguration.Count > 0)
            {
                foreach (WidgetConfiguration item in widgetConfiguration)
                {
                    if (item.WidgetType == "bs")
                    {
                        object[] args = new object[] { item.RestApiUrls, item.RestApiHeader, };
                        fetchingService = (IBuildService)Activator.CreateInstance(Type.GetType(item.ServiceName), args);
                    }
                }
            }

            List<Build> result = new List<Build>();
            if (fetchingService != null)
            {
                result = fetchingService.GetBuilds();
            }

            return this.Ok(result);
        }

        // GET view/it
        [HttpGet("it")]
        public async Task<IActionResult> IssueTracker()
        {
            IIssueService fetchingService = null;

            string configJson = await IOHelper.ReadFile(this.configFilePath);
            List<WidgetConfiguration> widgetConfiguration = JsonConvert.DeserializeObject<List<WidgetConfiguration>>(configJson);

            if (widgetConfiguration != null && widgetConfiguration.Count > 0)
            {
                foreach (WidgetConfiguration item in widgetConfiguration)
                {
                    if (item.WidgetType == "it")
                    {
                        object[] args = new object[] { item.RestApiUrls, item.RestApiHeader, };
                        fetchingService = (IIssueService)Activator.CreateInstance(Type.GetType(item.ServiceName), args);
                    }
                }
            }

            List<Issue> result = new List<Issue>();
            if (fetchingService != null)
            {
                result = fetchingService.GetIssues();
            }

            return this.Ok(result);
        }

        // GET view/iframe
        [HttpGet("iframe")]
        public async Task<IActionResult> Iframe()
        {
            List<Iframe> result = new List<Iframe>();
            string configJson = await IOHelper.ReadFile(this.configFilePath);
            List<WidgetConfiguration> widgetConfiguration = JsonConvert.DeserializeObject<List<WidgetConfiguration>>(configJson);

            if (widgetConfiguration != null && widgetConfiguration.Count > 0)
            {
                foreach (WidgetConfiguration item in widgetConfiguration)
                {
                    if (item.WidgetType == "iframe")
                    {
                        result.Add(new Iframe() { Html = item.ServiceName });
                    }
                }
            }

            return this.Ok(result);
        }
    }
}