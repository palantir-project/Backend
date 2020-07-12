namespace Palantir.GitHub
{
    using System.Collections.Generic;
    using System.Linq;
    using Palantir.Domain.Adapters;
    using Palantir.Domain.Configurations;
    using Palantir.Exceptions;

    public class WidgetConfigurationAdapter : IWidgetConfigurationAdapter
    {
        private const string GitHubUrl = "https://api.github.com/repos";

        private const string UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41";

        private readonly RestApiConfiguration adaptee;

        public WidgetConfigurationAdapter(RestApiConfiguration adaptee)
        {
            this.adaptee = adaptee;
        }

        public WidgetConfiguration Configuration { get; set; }

        public WidgetConfiguration GetWidgetConfiguration()
        {
            List<string> urls = new List<string>();
            foreach (string item in this.adaptee.Projects)
            {
                urls.Add($"{GitHubUrl}/{this.adaptee.User}/{item}/pulls");
            }

            string userAgent = UserAgent;
            RestApiHeader header = new RestApiHeader()
            {
                AuthorizationToken = this.adaptee.Token,
                UserAgent = userAgent,
            };

            return new WidgetConfiguration()
            {
                WidgetType = "scm",
                ServiceName = "Palantir.GitHub.RestApiService, Palantir.GitHub",
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
                throw new InvalidFieldException("El nombre de proyecto de GitHub es obligatorio");
            }

            if (string.IsNullOrEmpty(adaptee.User))
            {
                throw new InvalidFieldException("El nombre de usuario de GitHub es obligatorio");
            }
        }
    }
}