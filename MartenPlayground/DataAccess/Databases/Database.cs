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
				o.RegisterDocumentType<Person>();

				o.Events.DatabaseSchemaName = "events";
				o.Events.AddEventType(typeof(PersonCreated));
				o.Events.AddEventType(typeof(ChangeName));
			});
		}

		public IDocumentStore GetDocumentStore() => this.documentStore;
	}
}