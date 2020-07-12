namespace Palantir.TravisCI.Tests
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
            this.adaptee = new RestApiConfiguration()
            {
                Projects = new List<string>() { "8722522" },
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
            Assert.Equal("bs", widgetType);
        }

        [Fact]
        public void ReturnsServiceName()
        {
            // When
            WidgetConfiguration configuration = this.adapter.GetWidgetConfiguration();
            string serviceName = configuration.ServiceName;

            // Then
            Assert.Equal("Palantir.TravisCI.RestApiService, Palantir.TravisCI", serviceName);
        }

        [Fact]
        public void ReturnsAdaptedUrl()
        {
            // When
            WidgetConfiguration configuration = this.adapter.GetWidgetConfiguration();
            string adaptedUrl = configuration.RestApiUrls.FirstOrDefault();

            // Then
            Assert.Equal("https://api.travis-ci.org/repo/8722522/builds", adaptedUrl.ToString());
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