using System.Collections.Generic;
using MartenPlayground.DataAccess.Models;

namespace MartenPlayground.Models
{
	public sealed class HomeModel
	{
		public IEnumerable<Person> People { get; set; }
	}
}