using System;
using Marten.Events;
using Marten.Events.Projections;
using Marten.Schema;
using MartenPlayground.DataAccess.Events;

public class ChangeNameTransform : ITransform<ChangeName, PreviousName>
{
    public PreviousName Transform(EventStream stream, Event<ChangeName> input)
    {
        return new PreviousName
        {
            Id = input.Id,
            Name = input.Data.Name,
            StreamId = stream.Id
        };
    }
}

public class PreviousName
{
    public Guid Id { get; set; }
    public Guid StreamId { get; set; }
    public string Name { get; set; }

    public static void ConfigureMarten(DocumentMapping<PreviousName> mapping)
    {
        mapping.DatabaseSchemaName = "events";
        mapping.Index(x => x.StreamId);
    }
}