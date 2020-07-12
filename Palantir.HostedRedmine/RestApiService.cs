namespace Palantir.HostedRedmine
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Newtonsoft.Json;
    using Palantir.Domain.Configurations;
    using Palantir.Domain.Services;

    public class RestApiService : IIssueService
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

        public List<Palantir.Domain.Models.Issue> GetIssues()
        {
            IssueAdapter adapter = new IssueAdapter();
            List<Palantir.Domain.Models.Issue> result = new List<Palantir.Domain.Models.Issue>();

            this.client.DefaultRequestHeaders.Add("X-Redmine-API-Key", this.restApiHeader.AuthorizationToken);

            foreach (string item in this.restApiUrls)
            {
                HttpResponseMessage data = this.client.GetAsync(item).Result;

                if (data.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpRequestException($"Error al realizar petición a {item}");
                }

                string dataContent = data.Content.ReadAsStringAsync().Result;
                HostedRedmineIssue hostedRedmineIssue = JsonConvert.DeserializeObject<HostedRedmineIssue>(dataContent);

                foreach (Issue issue in hostedRedmineIssue.Issues)
                {
                    result.Add(adapter.GetIssue(issue));
                }
            }

            return result;
        }
    }
}