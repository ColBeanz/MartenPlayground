using Marten;
using MartenPlayground.DataAccess.Events;
using MartenPlayground.DataAccess.Models;

namespace MartenPlayground.DataAccess.Databases
{
	sealed class Database : IDatabase
	{
		readonly IDocumentStore documentStore;

		public Database()
		{
			this.documentStore = DocumentStore.For(o =>
			{
				o.Connection("host=localhost;port=4321;database=martenplayground;password=password;username=postgres");
				o.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;

				o.Events.DatabaseSchemaName = "events";
				o.Events.AddEventType(typeof(PersonCreated));
				o.Events.AddEventType(typeof(ChangeName));

				o.Events.InlineProjections.AggregateStreamsWith<PersonAggregate>();
				o.Events.InlineProjections.TransformEvents(new ChangeNameTransform());
			});
		}

		public IDocumentStore GetDocumentStore() => this.documentStore;
	}
}