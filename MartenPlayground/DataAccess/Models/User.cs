using System;
using Marten.Schema;
using Marten.Schema.Identity;
using MartenPlayground.DataAccess.Events;

namespace MartenPlayground.DataAccess.Models
{
    public class User
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

		public static void ConfigureMarten(DocumentMapping<User> mapping)
		{
			mapping.DatabaseSchemaName = "newschema";
			mapping.IdStrategy = new CombGuidIdGeneration();
		}
	}
}