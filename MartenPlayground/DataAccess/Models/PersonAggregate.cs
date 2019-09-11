using System;
using Marten.Schema;
using MartenPlayground.DataAccess.Events;

namespace MartenPlayground.DataAccess.Models
{
	public class PersonAggregate
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

		public void Apply(ChangeName nameChanged)
		{
			this.Name = nameChanged.Name;
		}

		public override string ToString()
		{
			return $"Person named - {this.Name}";
		}

		public static void ConfigureMarten(DocumentMapping<PersonAggregate> mapping)
		{
			mapping.DatabaseSchemaName = "events";
		}
	}
}