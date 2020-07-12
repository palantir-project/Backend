namespace Palantir.GitLab
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Newtonsoft.Json;
    using Palantir.Domain.Configurations;
    using Palantir.Domain.Models;
    using Palantir.Domain.Services;

    public class RestApiService : IMergeRequestService
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

        public List<MergeRequest> GetMergeRequests()
        {
            MergeRequestAdapter adapter = new MergeRequestAdapter();
            List<MergeRequest> result = new List<MergeRequest>();

            this.client.DefaultRequestHeaders.Add("Private-Token", this.restApiHeader.AuthorizationToken);

            foreach (string item in this.restApiUrls)
            {
                HttpResponseMessage data = this.client.GetAsync(item).Result;

                if (data.StatusCode != HttpStatusCode.OK)
                {
                    throw new HttpRequestException($"Error al realizar petición a {item}");
                }

                string dataContent = data.Content.ReadAsStringAsync().Result;
                IEnumerable<GitLabMergeRequest> gitLabMergeRequests = JsonConvert.DeserializeObject<IEnumerable<GitLabMergeRequest>>(dataContent);

                foreach (GitLabMergeRequest gitLabMergeRequest in gitLabMergeRequests)
                {
                    result.Add(adapter.GetMergeRequest(gitLabMergeRequest));
                }
            }

            return result;
        }
    }
}
