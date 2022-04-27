using System;
using Marten.Schema;
using Marten.Schema.Identity;

namespace MartenPlayground.DataAccess.Models
{
    public class Crime
	{
		public Guid Id { get; set; }
		public string CrimeName { get; set; }
		public Guid UserId { get; set; }

		public static void ConfigureMarten(DocumentMapping<Crime> mapping)
		{
			mapping.DatabaseSchemaName = "newschema";
			mapping.IdStrategy = new CombGuidIdGeneration();
			mapping.Index(x =>x.UserId);
		}
	}
}