namespace Palantir.TravisCI
{
    using System;

    public class BuildAdapter
    {
        public Palantir.Domain.Models.Build GetBuild(Build adaptee) => new Palantir.Domain.Models.Build()
        {
            Id = adaptee.Id,
            Url = new Uri($"https://travis-ci.org/{adaptee.Repository.Slug}/builds/{adaptee.Id}"),
            Number = adaptee.Number,
            State = adaptee.State,
            Repository = adaptee.Repository.Name,
            Branch = adaptee.Branch.Name,
            Commit = adaptee.Commit.Message,
            StartedOn = adaptee.StartedAt,
            FinishedOn = adaptee.FinishedAt,
        };
    }
}
