namespace Palantir.TravisCI
{
    using System;
    using Newtonsoft.Json;

    public partial class TravisCIBuild
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("@href")]
        public string Href { get; set; }

        [JsonProperty("@representation")]
        public string Representation { get; set; }

        [JsonProperty("@pagination")]
        public Pagination Pagination { get; set; }

        [JsonProperty("builds")]
        public Build[] Builds { get; set; }
    }

    public partial class Build
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("@href")]
        public string Href { get; set; }

        [JsonProperty("@representation")]
        public string Representation { get; set; }

        [JsonProperty("@permissions")]
        public Permissions Permissions { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("event_type")]
        public string EventType { get; set; }

        [JsonProperty("previous_state")]
        public string PreviousState { get; set; }

        [JsonProperty("pull_request_title")]
        public string PullRequestTitle { get; set; }

        [JsonProperty("pull_request_number")]
        public long? PullRequestNumber { get; set; }

        [JsonProperty("started_at")]
        public DateTimeOffset StartedAt { get; set; }

        [JsonProperty("finished_at")]
        public DateTimeOffset FinishedAt { get; set; }

        [JsonProperty("private")]
        public bool? Private { get; set; }

        [JsonProperty("repository")]
        public Repository Repository { get; set; }

        [JsonProperty("branch")]
        public Branch Branch { get; set; }

        [JsonProperty("tag")]
        public Tag Tag { get; set; }

        [JsonProperty("commit")]
        public Commit Commit { get; set; }

        [JsonProperty("jobs")]
        public CreatedBy[] Jobs { get; set; }

        [JsonProperty("stages")]
        public object[] Stages { get; set; }

        [JsonProperty("created_by")]
        public CreatedBy CreatedBy { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }
    }

    public partial class Branch
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("@href")]
        public string Href { get; set; }

        [JsonProperty("@representation")]
        public string Representation { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Commit
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("@representation")]
        public string Representation { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("sha")]
        public string Sha { get; set; }

        [JsonProperty("ref")]
        public string Ref { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("compare_url")]
        public Uri CompareUrl { get; set; }

        [JsonProperty("committed_at")]
        public DateTimeOffset CommittedAt { get; set; }
    }

    public partial class CreatedBy
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("@href")]
        public string Href { get; set; }

        [JsonProperty("@representation")]
        public string Representation { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }
    }

    public partial class Permissions
    {
        [JsonProperty("read")]
        public bool Read { get; set; }

        [JsonProperty("cancel")]
        public bool Cancel { get; set; }

        [JsonProperty("restart")]
        public bool Restart { get; set; }
    }

    public partial class Repository
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("@href")]
        public string Href { get; set; }

        [JsonProperty("@representation")]
        public string Representation { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }
    }

    public partial class Tag
    {
        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("@representation")]
        public string Representation { get; set; }

        [JsonProperty("repository_id")]
        public long RepositoryId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("last_build_id")]
        public long LastBuildId { get; set; }
    }

    public partial class Pagination
    {
        [JsonProperty("limit")]
        public long Limit { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("is_first")]
        public bool IsFirst { get; set; }

        [JsonProperty("is_last")]
        public bool IsLast { get; set; }

        [JsonProperty("next")]
        public Next Next { get; set; }

        [JsonProperty("prev")]
        public object Prev { get; set; }

        [JsonProperty("first")]
        public Next First { get; set; }

        [JsonProperty("last")]
        public Next Last { get; set; }
    }

    public partial class Next
    {
        [JsonProperty("@href")]
        public string Href { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }
    }
}
