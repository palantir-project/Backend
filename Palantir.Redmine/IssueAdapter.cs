namespace Palantir.Redmine
{
    public class IssueAdapter
    {
        public Palantir.Domain.Models.Issue GetIssue(Issue adaptee) => new Palantir.Domain.Models.Issue()
        {
            Id = adaptee.Id,
            Project = adaptee.Project.Name,
            Title = adaptee.Subject,
            State = adaptee.Status.Name,
            AuthorName = adaptee.Author.Name,
            AssignedTo = adaptee.AssignedTo != null ? adaptee.AssignedTo.Name : "no one",
            CreatedOn = adaptee.CreatedOn,
            UpdatedOn = adaptee.UpdatedOn,
        };
    }
}
