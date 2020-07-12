namespace Palantir.Domain.Services
{
    using System.Collections.Generic;
    using Palantir.Domain.Models;

    public interface IMergeRequestService
    {
        List<MergeRequest> GetMergeRequests();
    }
}