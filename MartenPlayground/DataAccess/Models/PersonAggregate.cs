using System;
using System.Collections.Generic;
using Marten.Schema;
using MartenPlayground.DataAccess.Events;

namespace MartenPlayground.DataAccess.Models
{
    public class PersonAggregate
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public List<string> CriminalHistory { get; set; } = new List<string>();

		public void Apply(PersonCreated personCreated)
		{
			this.Name = personCreated.Name;
		}
		public void Apply(CrimeCommited crimeCommited)
		{
			this.CriminalHistory.Add(crimeCommited.Crime);
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