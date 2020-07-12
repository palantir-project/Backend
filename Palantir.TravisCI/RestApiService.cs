namespace Palantir.TravisCI
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Newtonsoft.Json;
    using Palantir.Domain.Configurations;
    using Palantir.Domain.Services;

    public class RestApiService : IBuildService
    {
        private readonly HttpClient client;

        private readonly List<string> restApiUrls;

        private readonly RestApiHeader restApiHeader;

        public RestApiService(List<string> urls, RestApiHeader header)
        {
            this.client = new HttpClient();
            this.restApiUrls = urls;
            this.restApiHeader = header;
        }

        public List<Domain.Models.Build> GetBuilds()
        {
            BuildAdapter adapter = new BuildAdapter();
            List<Domain.Models.Build> result = new List<Domain.Models.Build>();

            this.client.DefaultRequestHeaders.Add("Authorization", $"token {this.restApiHeader.AuthorizationToken}");
            this.client.DefaultRequestHeaders.Add("User-Agent", this.restApiHeader.UserAgent);
            this.client.DefaultRequestHeaders.Add("Travis-API-Version", this.restApiHeader.ApiVersion);

            foreach (string url in this.restApiUrls)
            {
                HttpResponseMessage data = this.client.GetAsync(url).Result;

                if (data.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpRequestException($"Error al realizar petición a {url}");
                }

                string dataContent = data.Content.ReadAsStringAsync().Result;
                TravisCIBuild travisCIBuilds = JsonConvert.DeserializeObject<TravisCIBuild>(dataContent);

                IEnumerable<Build> lastBuildByBranch = travisCIBuilds.Builds.GroupBy(x => x.Branch.Name).Select(x => x.FirstOrDefault());

                foreach (Build build in lastBuildByBranch)
                {
                    result.Add(adapter.GetBuild(build));
                }
            }

            return result;
        }
    }
}