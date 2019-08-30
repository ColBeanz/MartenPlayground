using System;
using Marten.Schema;

namespace MartenPlayground.DataAccess.Events
{
	public class PersonCreated
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public static void ConfigureMarten(DocumentMapping<PersonCreated> mapping)
		{
			mapping.DatabaseSchemaName = "events";
		}
	}
}