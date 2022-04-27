using System;
using System.Collections.Generic;

namespace MartenPlayground.Models
{
	public sealed class GetAllEventsModel
	{
		public IEnumerable<GetAllEventModel> Events { get; set; }

		public class GetAllEventModel
		{
			public Guid EventId { get; set; }
			public Guid StreamId { get; set;}
			public string EventName { get; set;}
			public long SequenceNumber { get; set; }
		}
	}
}