namespace Palantir.WebApi.Tests.Controllers
{
    using System;
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
    public class ITControllerTests : IDisposable
    {
        public ITControllerTests()
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
        public async Task PostSavesConfigurationInJson()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/it");

            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.HostedRedmine",
                Projects = new List<string>() { "test-credit-system" },
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            await this.TestClient.PostAsync("admin/it", stringContent);

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/it");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "{\"WidgetType\":\"it\",\"ServiceName\":\"Palantir.HostedRedmine.RestApiService, Palantir.HostedRedmine\",\"RestApiUrls\":[\"http://www.hostedredmine.com/projects/test-credit-system/issues.json?status_id=8&limit=100\"],\"RestApiHeader\":{\"AuthorizationToken\":\"token\",\"UserAgent\":\"\",\"ApiVersion\":\"\"}}";

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task PostDoesNotSaveConfigurationIfThereIsPreviousConfiguration()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/it");

            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.HostedRedmine",
                Projects = new List<string>() { "test-credit-system" },
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/it", stringContent);

            // When
            RestApiConfiguration configuration2 = new RestApiConfiguration
            {
                Service = "Palantir.HostedRedmine",
                Projects = new List<string>() { "another-project" },
                Token = "another-token",
            };
            string json2 = JsonConvert.SerializeObject(configuration2);
            StringContent stringContent2 = new StringContent(json2, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/it", stringContent2);

            HttpResponseMessage response = await this.TestClient.GetAsync("admin/it");

            // Then
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "{\"WidgetType\":\"it\",\"ServiceName\":\"Palantir.HostedRedmine.RestApiService, Palantir.HostedRedmine\",\"RestApiUrls\":[\"http://www.hostedredmine.com/projects/test-credit-system/issues.json?status_id=8&limit=100\"],\"RestApiHeader\":{\"AuthorizationToken\":\"token\",\"UserAgent\":\"\",\"ApiVersion\":\"\"}}";

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
            HttpResponseMessage response = await this.TestClient.PostAsync("admin/it", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PostReturnsUnauthorizedWhenHostedRedmineTokenIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.HostedRedmine",
                Projects = new List<string>() { "test-credit-system" },
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PostAsync("admin/it", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PostReturnsBadRequestWhenHostedRedmineProjectIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.HostedRedmine",
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PostAsync("admin/it", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PutUpdatesConfigurationInJsonIfThereIsPreviousConfiguration()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/it");

            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.HostedRedmine",
                Projects = new List<string>() { "project" },
                Token = "test-token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/it", stringContent);

            // When
            RestApiConfiguration configuration2 = new RestApiConfiguration
            {
                Service = "Palantir.HostedRedmine",
                Projects = new List<string>() { "test-credit-system" },
                Token = "token",
            };
            string json2 = JsonConvert.SerializeObject(configuration2);
            StringContent stringContent2 = new StringContent(json2, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PutAsync("admin/it", stringContent2);

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/it");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "{\"WidgetType\":\"it\",\"ServiceName\":\"Palantir.HostedRedmine.RestApiService, Palantir.HostedRedmine\",\"RestApiUrls\":[\"http://www.hostedredmine.com/projects/test-credit-system/issues.json?status_id=8&limit=100\"],\"RestApiHeader\":{\"AuthorizationToken\":\"token\",\"UserAgent\":\"\",\"ApiVersion\":\"\"}}";

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task PutDoesNotUpdateConfigurationInJsonIfThereIsNotPreviousConfiguration()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/it");

            // When
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.HostedRedmine",
                Projects = new List<string>() { "test-credit-system" },
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PutAsync("admin/it", stringContent);

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/it");
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
            HttpResponseMessage response = await this.TestClient.PutAsync("admin/it", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PutReturnsUnauthorizedWhenHostedRedmineTokenIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.HostedRedmine",
                Projects = new List<string>() { "test-credit-system" },
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PutAsync("admin/it", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PutReturnsBadRequestWhenHostedRedmineProjectIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.HostedRedmine",
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PutAsync("admin/it", stringContent);

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
                Service = "Palantir.HostedRedmine",
                Projects = new List<string>() { "test-credit-system" },
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/it", stringContent);

            // When
            await this.TestClient.DeleteAsync("admin/it");

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/it");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = string.Empty;

            Assert.Equal(expectedJson, jsonResponse);
        }
    }
}