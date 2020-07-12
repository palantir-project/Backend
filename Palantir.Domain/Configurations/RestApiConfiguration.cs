namespace Palantir.Domain.Configurations
{
    using System.Collections.Generic;

    public class RestApiConfiguration
    {
        public string Service { get; set; }

        public string Token { get; set; }

        public List<string> Projects { get; set; }
        
        public string User { get; set; }

        public string Port { get; set; }

        public string Html { get; set; }
    }
}