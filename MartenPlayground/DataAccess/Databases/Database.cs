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

				// Event Store stuff
				o.Events.DatabaseSchemaName = "events";
				o.Events.AddEventType(typeof(PersonCreated));
				o.Events.AddEventType(typeof(CrimeCommited));

				o.Events.InlineProjections.AggregateStreamsWith<PersonAggregate>();
				o.Events.InlineProjections.TransformEvents(new CrimesCommitedTransform());

				// Document Store stuff
				o.Schema.For<User>();
				o.Schema.For<Crime>();
			});
		}

		public IDocumentStore GetDocumentStore() => this.documentStore;
	}
}