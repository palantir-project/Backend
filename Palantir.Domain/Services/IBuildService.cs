namespace Palantir.Domain.Services
{
    using System.Collections.Generic;
    using Palantir.Domain.Models;

    public interface IBuildService
    {
        List<Build> GetBuilds();
    }
}