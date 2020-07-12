namespace Palantir.GitHub.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Palantir.Domain.Configurations;
    using Xunit;

    public class WidgetConfigurationAdapterTests : IDisposable
    {
        private readonly WidgetConfigurationAdapter adapter;

        private RestApiConfiguration adaptee;

        public WidgetConfigurationAdapterTests()
        {
            // Given
            this.adaptee = new RestApiConfiguration
            {
                User = "user",
                Projects = new List<string>() { "project" },
                Token = "token",
            };

            this.adapter = new WidgetConfigurationAdapter(this.adaptee);
        }

        public void Dispose()
        {
            this.adaptee = null;
        }

        [Fact]
        public void ReturnsWidgetType()
        {
            // When
            WidgetConfiguration configuration = this.adapter.GetWidgetConfiguration();
            string widgetType = configuration.WidgetType;

            // Then
            Assert.Equal("scm", widgetType);
        }

        [Fact]
        public void ReturnsServiceName()
        {
            // When
            WidgetConfiguration configuration = this.adapter.GetWidgetConfiguration();
            string serviceName = configuration.ServiceName;

            // Then
            Assert.Equal("Palantir.GitHub.RestApiService, Palantir.GitHub", serviceName);
        }

        [Fact]
        public void ReturnsAdaptedUrl()
        {
            // When
            WidgetConfiguration configuration = this.adapter.GetWidgetConfiguration();
            string adaptedUrl = configuration.RestApiUrls.FirstOrDefault();

            // Then
            Assert.Equal("https://api.github.com/repos/user/project/pulls", adaptedUrl.ToString());
        }

        [Fact]
        public void ReturnsAdaptedHeaderWithUserAgent()
        {
            // When
            WidgetConfiguration configuration = this.adapter.GetWidgetConfiguration();
            string userAgent = configuration.RestApiHeader.UserAgent;

            // Then
            Assert.Equal("Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41", userAgent);
        }

        [Fact]
        public void ReturnsAdaptedHeaderWithAuthorizationToken()
        {
            // When
            WidgetConfiguration configuration = this.adapter.GetWidgetConfiguration();
            string token = configuration.RestApiHeader.AuthorizationToken;

            // Then
            Assert.Equal("token", token);
        }
    }
}