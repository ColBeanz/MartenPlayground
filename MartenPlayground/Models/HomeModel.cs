using System;
using System.Collections.Generic;

namespace MartenPlayground.Models
{
	public sealed class HomeModel
	{
		public IEnumerable<Person> People { get; set; }

		public class Person
		{
			public Guid Id { get; set;}
			public string Name { get; set;}
		}
	}
}