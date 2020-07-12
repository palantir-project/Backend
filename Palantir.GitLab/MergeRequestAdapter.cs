namespace Palantir.GitLab
{
    using Palantir.Domain.Models;

    public class MergeRequestAdapter
    {
        public MergeRequest GetMergeRequest(GitLabMergeRequest adaptee) => new MergeRequest()
        {
            Id = adaptee.Id,
            Title = adaptee.Title,
            AuthorName = adaptee.Author.Username,
            CreationDate = adaptee.CreatedAt,
            LastUpdate = adaptee.UpdatedAt,
            State = adaptee.State,
            Url = adaptee.WebUrl,
        };
    }
}
