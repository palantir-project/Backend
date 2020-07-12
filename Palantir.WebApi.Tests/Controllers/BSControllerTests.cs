namespace Palantir.WebApi.Tests.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Newtonsoft.Json;
    using Palantir.Domain.Configurations;
    using Xunit;

    [Collection("Controllers Test Collection")]
    public class BSControllerTests
    {
        public BSControllerTests()
        {
            this.Server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            this.TestClient = this.Server.CreateClient();
        }

        public HttpClient TestClient { get; private set; }

        private TestServer Server { get; set; }

        [Fact]
        public void Dispose()
        {
            this.Server = null;
            this.TestClient = null;
        }

        [Fact]
        public async Task PostSavesConfigurationInJson()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/bs");

            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Projects = new List<string>() { "8722522" },
                Service = "Palantir.TravisCI",
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            await this.TestClient.PostAsync("admin/bs", stringContent);

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/bs");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "{\"WidgetType\":\"bs\",\"ServiceName\":\"Palantir.TravisCI.RestApiService, Palantir.TravisCI\",\"RestApiUrls\":[\"https://api.travis-ci.org/repo/8722522/builds\"],\"RestApiHeader\":{\"AuthorizationToken\":\"token\",\"UserAgent\":\"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41\",\"ApiVersion\":\"3\"}}";

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task PostDoesNotSaveConfigurationIfThereIsPreviousConfiguration()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/bs");

            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Projects = new List<string>() { "8722522" },
                Service = "Palantir.TravisCI",
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/bs", stringContent);

            // When
            RestApiConfiguration configuration2 = new RestApiConfiguration
            {
                Projects = new List<string>() { "8722522" },
                Service = "Palantir.TravisCI",
                Token = "another-token",
            };
            string json2 = JsonConvert.SerializeObject(configuration2);
            StringContent stringContent2 = new StringContent(json2, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/bs", stringContent2);

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/bs");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "{\"WidgetType\":\"bs\",\"ServiceName\":\"Palantir.TravisCI.RestApiService, Palantir.TravisCI\",\"RestApiUrls\":[\"https://api.travis-ci.org/repo/8722522/builds\"],\"RestApiHeader\":{\"AuthorizationToken\":\"token\",\"UserAgent\":\"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41\",\"ApiVersion\":\"3\"}}";

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task PostReturnsBadRequestWhenServiceNameIsInvalidOrUnexistent()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "invalid",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PostAsync("admin/bs", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PostReturnsUnauthorizedWhenTravisCITokenIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Projects = new List<string>() { "8722522" },
                Service = "Palantir.TravisCI",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PostAsync("admin/bs", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PostReturnsBadRequestWhenTravisCIProjectIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.TravisCI",
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // WhenGitHub
            HttpResponseMessage response = await this.TestClient.PostAsync("admin/bs", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PutUpdatesConfigurationInJsonIfThereIsPreviousConfiguration()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/bs");

            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Projects = new List<string>() { "0" },
                Service = "Palantir.TravisCI",
                Token = "test-token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/bs", stringContent);

            // When
            RestApiConfiguration configuration2 = new RestApiConfiguration
            {
                Projects = new List<string>() { "8722522" },
                Service = "Palantir.TravisCI",
                Token = "token",
            };
            string json2 = JsonConvert.SerializeObject(configuration2);
            StringContent stringContent2 = new StringContent(json2, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PutAsync("admin/bs", stringContent2);

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/bs");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "{\"WidgetType\":\"bs\",\"ServiceName\":\"Palantir.TravisCI.RestApiService, Palantir.TravisCI\",\"RestApiUrls\":[\"https://api.travis-ci.org/repo/8722522/builds\"],\"RestApiHeader\":{\"AuthorizationToken\":\"token\",\"UserAgent\":\"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41\",\"ApiVersion\":\"3\"}}";

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task PutDoesNotUpdateConfigurationInJsonIfThereIsNotPreviousConfiguration()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/bs");

            // When
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Projects = new List<string>() { "8722522" },
                Service = "Palantir.TravisCI",
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PutAsync("admin/bs", stringContent);

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/bs");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = string.Empty;

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task PutReturnsBadRequestWhenServiceNameIsInvalidOrUnexistent()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "invalid",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PutAsync("admin/bs", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PutReturnsUnauthorizedWhenTravisCITokenIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Projects = new List<string>() { "8722522" },
                Service = "Palantir.TravisCI",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PutAsync("admin/bs", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PutReturnsBadRequestWhenTravisCIProjectIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.TravisCI",
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // WhenGitHub
            HttpResponseMessage response = await this.TestClient.PutAsync("admin/bs", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task DeleteRemovesConfigurationInJson()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Projects = new List<string>() { "8722522" },
                Service = "Palantir.TravisCI",
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/bs", stringContent);

            // When
            await this.TestClient.DeleteAsync("admin/bs");

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/bs");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = string.Empty;

            Assert.Equal(expectedJson, jsonResponse);
        }
    }
}