using System;
using Marten.Events;
using Marten.Events.Projections;
using Marten.Schema;
using MartenPlayground.DataAccess.Events;

public class CrimesCommitedTransform : ITransform<CrimeCommited, CommitedCrime>
{
    public CommitedCrime Transform(EventStream stream, Event<CrimeCommited> input)
    {
        return new CommitedCrime
        {
            Id = input.Id,
            Crime = input.Data.Crime,
            StreamId = stream.Id
        };
    }
}

public class CommitedCrime
{
    public Guid Id { get; set; }
    public Guid StreamId { get; set; }
    public string Crime { get; set; }

    public static void ConfigureMarten(DocumentMapping<CommitedCrime> mapping)
    {
        mapping.DatabaseSchemaName = "events";
        //mapping.Index(x => x.StreamId);
    }
}