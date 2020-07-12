namespace Palantir.Domain.Services
{
    using System.Collections.Generic;
    using Palantir.Domain.Models;

    public interface IIssueService
    {
        List<Issue> GetIssues();
    }
}