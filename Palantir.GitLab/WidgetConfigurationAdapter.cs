namespace Palantir.GitLab
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Palantir.Domain.Adapters;
    using Palantir.Domain.Configurations;
    using Palantir.Exceptions;

    public class WidgetConfigurationAdapter : IWidgetConfigurationAdapter
    {
        private const string GitLabUrl = "https://gitlab.com/api/v4/projects";

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
                urls.Add($"{GitLabUrl}/{item}/merge_requests");
            }

            RestApiHeader header = new RestApiHeader()
            {
                AuthorizationToken = this.adaptee.Token,
            };
            
            return new WidgetConfiguration()
            {
                WidgetType = "scm",
                ServiceName = "Palantir.GitLab.RestApiService, Palantir.GitLab",
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
                throw new InvalidFieldException("El ID de proyecto de GitLab es obligatorio");
            }

            foreach (string item in adaptee.Projects)
            {
                if (!Regex.IsMatch(item, @"^\d+$"))
                {
                    throw new InvalidFieldException("El ID de proyecto de GitLab debe componerse únicamente de caracteres numéricos");
                }
            }
        }
    }
}
