namespace Palantir.HostedRedmine
{
    using System.Collections.Generic;
    using System.Linq;
    using Palantir.Domain.Adapters;
    using Palantir.Domain.Configurations;

    public class WidgetConfigurationAdapter : IWidgetConfigurationAdapter
    {
        private const string HostedRedmineUrl = "http://www.hostedredmine.com/projects";

        private readonly RestApiConfiguration adaptee;

        public WidgetConfigurationAdapter(RestApiConfiguration adaptee)
        {
            this.adaptee = adaptee;
        }

        public WidgetConfiguration GetWidgetConfiguration()
        {
            List<string> urls = new List<string>();
            foreach (string item in this.adaptee.Projects)
            {
                urls.Add($"{HostedRedmineUrl}/{item}/issues.json?status_id=8&limit=100");
            }

            RestApiHeader header = new RestApiHeader()
            {
                AuthorizationToken = this.adaptee.Token,
            };

            return new WidgetConfiguration()
            {
                WidgetType = "it",
                ServiceName = "Palantir.HostedRedmine.RestApiService, Palantir.HostedRedmine",
                RestApiUrls = urls,
                RestApiHeader = header,
            };
        }

        public void ValidateWidgetConfiguration(RestApiConfiguration adaptee)
        {
            if (string.IsNullOrEmpty(adaptee.Token))
            {
                throw new Palantir.Exceptions.MissingTokenException();
            }

            if (adaptee.Projects == null || !adaptee.Projects.Any() || adaptee.Projects.Contains(string.Empty))
            {
                throw new Palantir.Exceptions.InvalidFieldException("El nombre de proyecto de HostedRedmine es obligatorio");
            }
        }
    }
}