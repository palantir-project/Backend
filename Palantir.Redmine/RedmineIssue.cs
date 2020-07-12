namespace Palantir.Redmine
{
    using System;
    using Newtonsoft.Json;

    public partial class RedmineIssue
    {
        [JsonProperty("issues")]
        public Issue[] Issues { get; set; }

        [JsonProperty("total_count")]
        public long TotalCount { get; set; }

        [JsonProperty("offset")]
        public long Offset { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }
    }

    public partial class Issue
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("project")]
        public AssignedTo Project { get; set; }

        [JsonProperty("tracker")]
        public AssignedTo Tracker { get; set; }

        [JsonProperty("status")]
        public AssignedTo Status { get; set; }

        [JsonProperty("priority")]
        public AssignedTo Priority { get; set; }

        [JsonProperty("author")]
        public AssignedTo Author { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("start_date")]
        public DateTimeOffset? StartDate { get; set; }

        [JsonProperty("due_date")]
        public DateTimeOffset? DueDate { get; set; }

        [JsonProperty("done_ratio")]
        public long DoneRatio { get; set; }

        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }

        [JsonProperty("estimated_hours")]
        public long? EstimatedHours { get; set; }

        [JsonProperty("created_on")]
        public DateTimeOffset CreatedOn { get; set; }

        [JsonProperty("updated_on")]
        public DateTimeOffset UpdatedOn { get; set; }

        [JsonProperty("closed_on")]
        public object ClosedOn { get; set; }

        [JsonProperty("assigned_to", NullValueHandling = NullValueHandling.Ignore)]
        public AssignedTo AssignedTo { get; set; }
    }

    public partial class AssignedTo
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}