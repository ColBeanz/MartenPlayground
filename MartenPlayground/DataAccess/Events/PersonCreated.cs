﻿using System;
using Marten.Schema;
using Marten.Schema.Identity;

namespace MartenPlayground.DataAccess.Events
{
	public class PersonCreated
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public static void ConfigureMarten(DocumentMapping<PersonCreated> mapping)
		{
			mapping.DatabaseSchemaName = "events";
			mapping.IdStrategy = new CombGuidIdGeneration();
		}
	}
}