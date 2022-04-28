using System;
using Marten.Schema;
using Marten.Schema.Identity;

namespace MartenPlayground.DataAccess.Events
{
	public class CrimeCommited
	{
		public Guid Id { get; set; }
		public string Crime { get; set; }

		public static void ConfigureMarten(DocumentMapping<CrimeCommited> mapping)
		{
			mapping.DatabaseSchemaName = "events";
		}
	}
}