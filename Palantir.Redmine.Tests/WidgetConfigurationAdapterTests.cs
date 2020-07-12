namespace Palantir.Redmine.Tests
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
                Port = "5000",
                Projects = new List<string>() { "test-credit-system" },
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
            Assert.Equal("it", widgetType);
        }

        [Fact]
        public void ReturnsServiceName()
        {
            // When
            WidgetConfiguration configuration = this.adapter.GetWidgetConfiguration();
            string serviceName = configuration.ServiceName;

            // Then
            Assert.Equal("Palantir.Redmine.RestApiService, Palantir.Redmine", serviceName);
        }

        [Fact]
        public void ReturnsAdaptedUrl()
        {
            // When
            WidgetConfiguration configuration = this.adapter.GetWidgetConfiguration();
            string adaptedUrl = configuration.RestApiUrls.FirstOrDefault();

            // Then
            Assert.Equal("http://localhost:5000/projects/test-credit-system/issues.json?status_id=8&limit=100", adaptedUrl.ToString());
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