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
    public class ScmControllerTests : IDisposable
    {
        public ScmControllerTests()
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
            await this.TestClient.DeleteAsync("admin/scm");

            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitLab",
                Projects = new List<string>() { "15722380" },
                User = string.Empty,
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            await this.TestClient.PostAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/scm");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "{\"WidgetType\":\"scm\",\"ServiceName\":\"Palantir.GitLab.RestApiService, Palantir.GitLab\",\"RestApiUrls\":[\"https://gitlab.com/api/v4/projects/15722380/merge_requests\"],\"RestApiHeader\":{\"AuthorizationToken\":\"token\",\"UserAgent\":\"\",\"ApiVersion\":\"\"}}";

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task PostDoesNotSaveConfigurationIfThereIsPreviousConfiguration()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/scm");

            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitLab",
                Projects = new List<string>() { "15722380" },
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/scm", stringContent);

            // When
            RestApiConfiguration configuration2 = new RestApiConfiguration
            {
                Service = "Palantir.GitHub",
                User = "gonzalocozzi",
                Projects = new List<string>() { "rest-api-test" },
                Token = "another-token",
            };
            string json2 = JsonConvert.SerializeObject(configuration2);
            StringContent stringContent2 = new StringContent(json2, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/scm", stringContent2);

            HttpResponseMessage response = await this.TestClient.GetAsync("admin/scm");

            // Then
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "{\"WidgetType\":\"scm\",\"ServiceName\":\"Palantir.GitLab.RestApiService, Palantir.GitLab\",\"RestApiUrls\":[\"https://gitlab.com/api/v4/projects/15722380/merge_requests\"],\"RestApiHeader\":{\"AuthorizationToken\":\"token\",\"UserAgent\":\"\",\"ApiVersion\":\"\"}}";

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
            HttpResponseMessage response = await this.TestClient.PostAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PostReturnsUnauthorizedWhenGitLabTokenIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitLab",
                Projects = new List<string>() { "15722380" },
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PostAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PostReturnsUnauthorizedWhenGitHubTokenIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitHub",
                User = "gonzalocozzi",
                Projects = new List<string>() { "rest-api-test" },
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PostAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PostReturnsBadRequestWhenGitHubProjectIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitHub",
                User = "gonzalocozzi",
                Token = "86a48616a377f0a29df4f086f085451afb044b53",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PostAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PostReturnsBadRequestWhenGitLabProjectIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitLab",
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PostAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PostReturnsBadRequestWhenGitHubUserIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitHub",
                Projects = new List<string>() { "rest-api-test" },
                Token = "86a48616a377f0a29df4f086f085451afb044b53",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PostAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PostReturnsUnauthorizedWhenGitLabProjectIsNotNumeric()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitLab",
                Projects = new List<string>() { "15722w380" },
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PostAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PutUpdatesConfigurationInJsonIfThereIsPreviousConfiguration()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/scm");

            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitHub",
                User = "gonzalocozzi",
                Projects = new List<string>() { "rest-api-test" },
                Token = "another-token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/scm", stringContent);

            // When
            RestApiConfiguration configuration2 = new RestApiConfiguration
            {
                Service = "Palantir.GitLab",
                Projects = new List<string>() { "15722380" },
                Token = "token",
            };
            string json2 = JsonConvert.SerializeObject(configuration2);
            StringContent stringContent2 = new StringContent(json2, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PutAsync("admin/scm", stringContent2);

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/scm");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "{\"WidgetType\":\"scm\",\"ServiceName\":\"Palantir.GitLab.RestApiService, Palantir.GitLab\",\"RestApiUrls\":[\"https://gitlab.com/api/v4/projects/15722380/merge_requests\"],\"RestApiHeader\":{\"AuthorizationToken\":\"token\",\"UserAgent\":\"\",\"ApiVersion\":\"\"}}";

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task PutDoesNotUpdateConfigurationInJsonIfThereIsNotPreviousConfiguration()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/scm");

            // When
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitLab",
                Projects = new List<string>() { "15722380" },
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PutAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/scm");
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
            HttpResponseMessage response = await this.TestClient.PutAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PutReturnsUnauthorizedWhenGitLabTokenIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitLab",
                Projects = new List<string>() { "15722380" },
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PutAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PutReturnsUnauthorizedWhenGitHubTokenIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitHub",
                User = "gonzalocozzi",
                Projects = new List<string>() { "rest-api-test" },
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PutAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PutReturnsBadRequestWhenGitHubProjectIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitHub",
                User = "gonzalocozzi",
                Token = "86a48616a377f0a29df4f086f085451afb044b53",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PutAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PutReturnsBadRequestWhenGitLabProjectIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitLab",
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PutAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PutReturnsBadRequestWhenGitHubUserIsNull()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitHub",
                Projects = new List<string>() { "rest-api-test" },
                Token = "86a48616a377f0a29df4f086f085451afb044b53",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PutAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task PutReturnsUnauthorizedWhenGitLabProjectIsNotNumeric()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitLab",
                Projects = new List<string>() { "15722w380" },
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            HttpResponseMessage response = await this.TestClient.PutAsync("admin/scm", stringContent);

            // Then
            HttpResponseMessage expectedResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            Assert.Equal(expectedResponse.StatusCode, response.StatusCode);
        }

        [Fact]
        public async Task DeleteRemovesConfigurationInJson()
        {
            // Given
            RestApiConfiguration configuration = new RestApiConfiguration
            {
                Service = "Palantir.GitLab",
                Projects = new List<string>() { "15722380" },
                Token = "token",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/scm", stringContent);

            // When
            await this.TestClient.DeleteAsync("admin/scm");

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/scm");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = string.Empty;

            Assert.Equal(expectedJson, jsonResponse);
        }
    }
}