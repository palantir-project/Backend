namespace Palantir.TravisCI
{
    using System.Collections.Generic;
    using System.Linq;
    using Palantir.Domain.Adapters;
    using Palantir.Domain.Configurations;
    using Palantir.Exceptions;

    public class WidgetConfigurationAdapter : IWidgetConfigurationAdapter
    {
        private const string TravisCIUrl = "https://api.travis-ci.org/repo";

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
                urls.Add($"{TravisCIUrl}/{item}/builds");
            }

            string userAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";
            RestApiHeader header = new RestApiHeader()
            {
                AuthorizationToken = this.adaptee.Token,
                UserAgent = userAgent,
                ApiVersion = "3",
            };

            return new WidgetConfiguration()
            {
                WidgetType = "bs",
                ServiceName = "Palantir.TravisCI.RestApiService, Palantir.TravisCI",
                RestApiUrls = urls,
                RestApiHeader = header,
            };
        }

        public void ValidateWidgetConfiguration(RestApiConfiguration adaptee)
        {
            if (string.IsNullOrEmpty(adaptee.Token))
            {
                throw new MissingTokenException();
            }

            if (adaptee.Projects == null || !adaptee.Projects.Any() || adaptee.Projects.Contains(string.Empty))
            {
                throw new InvalidFieldException("El nombre o ID de proyecto de Travis CI es obligatorio");
            }
        }
    }
}