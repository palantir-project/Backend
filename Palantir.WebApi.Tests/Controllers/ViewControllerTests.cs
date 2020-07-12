namespace Palantir.WebApi.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Newtonsoft.Json;
    using Palantir.Domain.Configurations;
    using Palantir.Domain.Models;
    using Xunit;

    [Collection("Controllers Test Collection")]
    public class ViewControllerTests : IDisposable
    {
        public ViewControllerTests()
        {
            this.Server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            this.TestClient = this.Server.CreateClient();
        }

        public HttpClient TestClient { get; private set; }

        private TestServer Server { get; set; }

        public void Dispose()
        {
            this.Server = null;
            this.TestClient = null;
        }

        [Fact]
        public async Task ScmReturnsExpectedGitLabMergeRequestResponse()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/scm");

            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitLab",
                Projects = new List<string>() { "15722380" },
                Token = "qAxa6BEzME7miLNQzmKD",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/scm", stringContent);

            // When
            HttpResponseMessage response = await this.TestClient.GetAsync("view/scm");

            // Then
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "[{\"id\":44126460,\"title\":\"Readme updated\",\"authorName\":\"nicopaez\",\"creationDate\":\"2019-12-06T12:55:01.316+00:00\",\"lastUpdate\":\"2019-12-06T12:55:01.316+00:00\",\"state\":\"opened\",\"url\":\"https://gitlab.com/untref-ingsoft/tfi-cozzi/sample-repo1/-/merge_requests/1\"}]";

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task ScmReturnsExpectedGitHubPullRequestResponse()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/scm");

            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitHub",
                User = "gonzalocozzi",
                Projects = new List<string>() { "rest-api-test" },
                Token = "86a48616a377f0a29df4f086f085451afb044b53",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/scm", stringContent);

            // When
            HttpResponseMessage response = await this.TestClient.GetAsync("view/scm");

            // Then
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "[{\"id\":315229289,\"title\":\"Test\",\"authorName\":\"gonzalocozzi\",\"creationDate\":\"2019-09-07T22:26:59+00:00\",\"lastUpdate\":\"2019-09-23T23:46:10+00:00\",\"state\":\"open\",\"url\":\"https://github.com/gonzalocozzi/rest-api-test/pull/1\"}]";

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task ITReturnsExpectedHostedRedmineIssuesResponse()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/it");

            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.HostedRedmine",
                Projects = new List<string>() { "test-credit-system" },
                Token = "0c93671c0f32595a533d91ad4001206f08cb9f7a",
            };

            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/it", stringContent);

            // When
            HttpResponseMessage response = await this.TestClient.GetAsync("view/it");

            // Then
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "[{\"id\":851196,\"url\":\"http://www.hostedredmine.com/issues/851196\",\"project\":\"TestCreditSystem\",\"title\":\"[Front][Back] Export Data table\",\"state\":\"In Progress\",\"authorName\":\"Eric Chau\",\"assignedTo\":\"surendra k\",\"createdOn\":\"2019-12-05T17:59:24+00:00\",\"updatedOn\":\"2019-12-11T20:09:08+00:00\"},{\"id\":833318,\"url\":\"http://www.hostedredmine.com/issues/833318\",\"project\":\"TestCreditSystem\",\"title\":\"Angleški logo\",\"state\":\"In Progress\",\"authorName\":\"Klemen Urbančnik\",\"assignedTo\":\"Klemen Urbančnik\",\"createdOn\":\"2019-08-29T07:21:25+00:00\",\"updatedOn\":\"2019-09-09T20:51:53+00:00\"},{\"id\":680102,\"url\":\"http://www.hostedredmine.com/issues/680102\",\"project\":\"TestCreditSystem\",\"title\":\"Dlaczego my?\",\"state\":\"In Progress\",\"authorName\":\"Rafał Hejnowicz\",\"assignedTo\":\"Mateusz Zabój\",\"createdOn\":\"2017-06-29T23:31:06+00:00\",\"updatedOn\":\"2017-07-27T19:53:37+00:00\"},{\"id\":678528,\"url\":\"http://www.hostedredmine.com/issues/678528\",\"project\":\"TestCreditSystem\",\"title\":\"abonnement bevriezen\",\"state\":\"In Progress\",\"authorName\":\"Ritchie Peperkoorn\",\"assignedTo\":\"Ritchie Peperkoorn\",\"createdOn\":\"2017-06-21T06:18:52+00:00\",\"updatedOn\":\"2017-07-06T09:52:41+00:00\"},{\"id\":635939,\"url\":\"http://www.hostedredmine.com/issues/635939\",\"project\":\"TestCreditSystem\",\"title\":\"GoGo JAPAN Tier 2 flyer\",\"state\":\"In Progress\",\"authorName\":\"Joyce Gorsuch\",\"assignedTo\":\"Geraldine Matias\",\"createdOn\":\"2017-02-10T03:54:45+00:00\",\"updatedOn\":\"2017-02-14T19:37:01+00:00\"},{\"id\":612685,\"url\":\"http://www.hostedredmine.com/issues/612685\",\"project\":\"TestCreditSystem\",\"title\":\"学习git\",\"state\":\"In Progress\",\"authorName\":\"浩然 白\",\"assignedTo\":\"no one\",\"createdOn\":\"2016-11-18T11:23:43+00:00\",\"updatedOn\":\"2016-11-21T01:39:44+00:00\"},{\"id\":568419,\"url\":\"http://www.hostedredmine.com/issues/568419\",\"project\":\"TestCreditSystem\",\"title\":\"Licentie capability inbouwen\",\"state\":\"In Progress\",\"authorName\":\"Hans de Bue\",\"assignedTo\":\"no one\",\"createdOn\":\"2016-06-22T05:45:40+00:00\",\"updatedOn\":\"2016-12-05T20:55:45+00:00\"}]";

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task BSReturnsExpectedTravisCIBuildsResponse()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/bs");

            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.TravisCI",
                Projects = new List<string>() { "8722522" },
                Token = "o5EDKkh872Ws00tnILXAjg",
            };

            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/bs", stringContent);

            // When
            HttpResponseMessage response = await this.TestClient.GetAsync("view/bs");

            // Then
            string expectedJson = "[{\"id\":631066056,\"url\":\"https://travis-ci.org/gonzalocozzi/aydoo-2016-e4/builds/631066056\",\"number\":\"297\",\"state\":\"errored\",\"repository\":\"aydoo-2016-e4\",\"branch\":\"master\",\"commit\":\"Bump mustache from 2.1.3 to 3.2.1 in /archivos de ejemplo/plantilla\\n\\nBumps [mustache](https://github.com/janl/mustache.js) from 2.1.3 to 3.2.1.\\n- [Release notes](https://github.com/janl/mustache.js/releases)\\n- [Changelog](https://github.com/janl/mustache.js/blob/master/CHANGELOG.md)\\n- [Commits](https://github.com/janl/mustache.js/compare/v2.1.3...v3.2.1)\\n\\nSigned-off-by: dependabot[bot] <support@github.com>\",\"startedOn\":\"2019-12-30T20:23:06+00:00\",\"finishedOn\":\"2019-12-30T20:23:23+00:00\"},{\"id\":631065999,\"url\":\"https://travis-ci.org/gonzalocozzi/aydoo-2016-e4/builds/631065999\",\"number\":\"296\",\"state\":\"errored\",\"repository\":\"aydoo-2016-e4\",\"branch\":\"dependabot/npm_and_yarn/archivos-de-ejemplo/plantilla/mustache-3.2.1\",\"commit\":\"Bump mustache from 2.1.3 to 3.2.1 in /archivos de ejemplo/plantilla\\n\\nBumps [mustache](https://github.com/janl/mustache.js) from 2.1.3 to 3.2.1.\\n- [Release notes](https://github.com/janl/mustache.js/releases)\\n- [Changelog](https://github.com/janl/mustache.js/blob/master/CHANGELOG.md)\\n- [Commits](https://github.com/janl/mustache.js/compare/v2.1.3...v3.2.1)\\n\\nSigned-off-by: dependabot[bot] <support@github.com>\",\"startedOn\":\"2019-12-30T20:23:01+00:00\",\"finishedOn\":\"2019-12-30T20:23:19+00:00\"},{\"id\":138963890,\"url\":\"https://travis-ci.org/gonzalocozzi/aydoo-2016-e4/builds/138963890\",\"number\":\"295\",\"state\":\"passed\",\"repository\":\"aydoo-2016-e4\",\"branch\":\"v2\",\"commit\":\"se hizo un cambio chiquito de redaccion en el Readme\",\"startedOn\":\"2016-06-20T17:06:09+00:00\",\"finishedOn\":\"2016-06-20T17:07:19+00:00\"}]";
            string jsonResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task IframeReturnsExpectedIframeResponse()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/iframe");

            Iframe configuration = new Iframe
            {
                Html = "<iframe></iframe>",
            };

            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/iframe", stringContent);

            // When
            HttpResponseMessage response = await this.TestClient.GetAsync("view/iframe");

            // Then
            string expectedJson = "[{\"html\":\"<iframe></iframe>\"}]";
            string jsonResponse = await response.Content.ReadAsStringAsync();

            Assert.Equal(expectedJson, jsonResponse);
        }
    }
}