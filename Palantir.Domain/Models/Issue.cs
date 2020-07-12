namespace Palantir.Domain.Models
{
    using System;

    public class Issue
    {
        public long Id { get; set; }

        public Uri Url { get; set; }

        public string Project { get; set; }

        public string Title { get; set; }

        public string State { get; set; }

        public string AuthorName { get; set; }

        public string AssignedTo { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }
    }
}