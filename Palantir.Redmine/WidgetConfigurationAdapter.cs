namespace Palantir.Redmine
{
    using System.Collections.Generic;
    using System.Linq;
    using Palantir.Domain.Adapters;
    using Palantir.Domain.Configurations;

    public class WidgetConfigurationAdapter : IWidgetConfigurationAdapter
    {
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
                urls.Add($"http://localhost:{this.adaptee.Port}/projects/{item}/issues.json?status_id=8&limit=100");
            }

            RestApiHeader header = new RestApiHeader()
            {
                AuthorizationToken = this.adaptee.Token,
            };

            return new WidgetConfiguration()
            {
                WidgetType = "it",
                ServiceName = "Palantir.Redmine.RestApiService, Palantir.Redmine",
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

            if (string.IsNullOrEmpty(adaptee.Port))
            {
                throw new Palantir.Exceptions.InvalidFieldException("El número de puerto local de Redmine es obligatorio");
            }

            if (adaptee.Projects == null || !adaptee.Projects.Any() || adaptee.Projects.Contains(string.Empty))
            {
                throw new Palantir.Exceptions.InvalidFieldException("El nombre de proyecto de Redmine es obligatorio");
            }
        }
    }
}