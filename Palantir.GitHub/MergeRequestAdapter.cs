namespace Palantir.GitHub
{
    using Palantir.Domain.Models;

    public class MergeRequestAdapter
    {
        public MergeRequest GetMergeRequest(GitHubPullRequest adaptee) => new MergeRequest()
        {
            Id = adaptee.Id,
            Title = adaptee.Title,
            AuthorName = adaptee.User.Login,
            CreationDate = adaptee.CreatedAt,
            LastUpdate = adaptee.UpdatedAt,
            State = adaptee.State,
            Url = adaptee.HtmlUrl,
        };
    }
}