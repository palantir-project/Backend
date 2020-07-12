namespace Palantir.Domain.Models
{
    using System;
    
    public class Build
    {
        public long Id { get; set; }

        public Uri Url { get; set; }

        public string Number { get; set; }

        public string State { get; set; }

        public string Repository { get; set; }

        public string Branch { get; set; }

        public string Commit { get; set; }

        public DateTimeOffset StartedOn { get; set; }

        public DateTimeOffset FinishedOn { get; set; }
    }
}