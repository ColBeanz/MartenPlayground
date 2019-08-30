using System;
using Marten.Schema;

namespace MartenPlayground.DataAccess.Models
{
	public class Person
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

		public static void ConfigureMarten(DocumentMapping<Person> mapping)
		{
			mapping.DatabaseSchemaName = "newschema";
		}
	}
}