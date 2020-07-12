namespace Palantir.WebApi.Tests.Controllers
{
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Newtonsoft.Json;
    using Palantir.Domain.Models;
    using Xunit;

    [Collection("Controllers Test Collection")]
    public class IframeControllerTests
    {
        public IframeControllerTests()
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
            await this.TestClient.DeleteAsync("admin/iframe");

            Iframe configuration = new Iframe
            {
                Html = "<iframe></iframe>",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            // When
            await this.TestClient.PostAsync("admin/iframe ", stringContent);

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/iframe");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "{\"WidgetType\":\"iframe\",\"ServiceName\":\"<iframe></iframe>\",\"RestApiUrls\":null,\"RestApiHeader\":null}";

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task PostDoesNotSaveConfigurationIfThereIsPreviousConfiguration()
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
            Iframe configuration2 = new Iframe
            {
                Html = "<iframe>new-iframe</iframe>",
            };
            string json2 = JsonConvert.SerializeObject(configuration2);
            StringContent stringContent2 = new StringContent(json2, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/iframe", stringContent2);

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/iframe");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "{\"WidgetType\":\"iframe\",\"ServiceName\":\"<iframe></iframe>\",\"RestApiUrls\":null,\"RestApiHeader\":null}";

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task PutUpdatesConfigurationInJsonIfThereIsPreviousConfiguration()
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
            Iframe configuration2 = new Iframe
            {
                Html = "<iframe>new-iframe</iframe>",
            };
            string json2 = JsonConvert.SerializeObject(configuration2);
            StringContent stringContent2 = new StringContent(json2, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PutAsync("admin/iframe", stringContent2);

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/iframe");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = "{\"WidgetType\":\"iframe\",\"ServiceName\":\"<iframe>new-iframe</iframe>\",\"RestApiUrls\":null,\"RestApiHeader\":null}";

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task PutDoesNotUpdateConfigurationInJsonIfThereIsNotPreviousConfiguration()
        {
            // Given
            await this.TestClient.DeleteAsync("admin/iframe");

            // When
            Iframe configuration = new Iframe
            {
                Html = "<iframe></iframe>",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PutAsync("admin/iframe", stringContent);
            
            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/iframe");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = string.Empty;

            Assert.Equal(expectedJson, jsonResponse);
        }

        [Fact]
        public async Task DeleteRemovesConfigurationInJson()
        {
            // Given
            Iframe configuration = new Iframe
            {
                Html = "<iframe></iframe>",
            };
            string json = JsonConvert.SerializeObject(configuration);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            await this.TestClient.PostAsync("admin/iframe ", stringContent);

            // When
            await this.TestClient.DeleteAsync("admin/iframe");

            // Then
            HttpResponseMessage response = await this.TestClient.GetAsync("admin/iframe");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            string expectedJson = string.Empty;

            Assert.Equal(expectedJson, jsonResponse);
        }
    }
}