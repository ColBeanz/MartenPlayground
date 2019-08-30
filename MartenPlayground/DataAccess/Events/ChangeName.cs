using System;
using Marten.Schema;

namespace MartenPlayground.DataAccess.Events
{
	public class ChangeName
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

		public static void ConfigureMarten(DocumentMapping<ChangeName> mapping)
		{
			mapping.DatabaseSchemaName = "events";
		}
	}
}