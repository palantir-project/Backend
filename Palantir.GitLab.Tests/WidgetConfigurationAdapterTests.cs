namespace Palantir.GitLab.Tests
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
                Projects = new List<string>() { "0123456789" },
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
            Assert.Equal("Palantir.GitLab.RestApiService, Palantir.GitLab", serviceName);
        }

        [Fact]
        public void ReturnsAdaptedUrl()
        {
            // When
            WidgetConfiguration configuration = this.adapter.GetWidgetConfiguration();
            string adaptedUrl = configuration.RestApiUrls.FirstOrDefault();

            // Then
            Assert.Equal("https://gitlab.com/api/v4/projects/0123456789/merge_requests", adaptedUrl.ToString());
        }

        [Fact]
        public void ReturnsAdaptedHeaderWithEmptyUserAgent()
        {
            // When
            WidgetConfiguration configuration = this.adapter.GetWidgetConfiguration();
            string userAgent = configuration.RestApiHeader.UserAgent;

            // Then
            Assert.Equal(string.Empty, userAgent);
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