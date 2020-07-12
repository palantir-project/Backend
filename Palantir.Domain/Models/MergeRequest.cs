namespace Palantir.Domain.Models
{
    using System;

    public class MergeRequest
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string AuthorName { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset LastUpdate { get; set; }

        public string State { get; set; }

        public Uri Url { get; set; }
    }
}