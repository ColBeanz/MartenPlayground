using System;
using System.Diagnostics;
using MartenPlayground.DataAccess.Databases;
using MartenPlayground.DataAccess.Events;
using MartenPlayground.DataAccess.Streams;
using Microsoft.AspNetCore.Mvc;
using MartenPlayground.Models;
using MartenPlayground.DataAccess.Models;
using System.Linq;

namespace MartenPlayground.Controllers
{
	public class EventSourceController : Controller
	{
		readonly IDatabase database;

		public EventSourceController(IDatabase database)
		{
			this.database = database;
		}
		public IActionResult Index()
		{
			var model = new HomeModel();
			
			using (var session = this.database.GetDocumentStore().LightweightSession())
			{
				model.People = session.Query<PersonAggregate>()
					.Select(x => new HomeModel.Person 
					{
						Id = x.Id,
						Name = x.Name
					}).ToList();
			}

			return View(model);
		}

		public IActionResult AddUser()
		{
			return View(new AddUserModel());
		}

		[HttpPost]
		public IActionResult AddUser(string name)
		{
			var personCreated = new PersonCreated(){ Name = name};

			using (var session = this.database.GetDocumentStore().LightweightSession())
			{
				session.Events.StartStream<Person>(personCreated);
				session.SaveChanges();
			}
			return Redirect("/eventsource/index");
		}

		[HttpPost]
		public IActionResult CommitCrime(Guid streamId, string crime)
		{
			var crimeCommited = new CrimeCommited { Crime = crime };

			using (var session = this.database.GetDocumentStore().LightweightSession())
			{
				session.Events.Append(streamId, crimeCommited);
				session.SaveChanges();
			}
			return Redirect("/eventsource/index");
		}

		public IActionResult PreviousCrimes(Guid streamId)
		{
			var model = new PreviousCrimesModel();

			using (var session = this.database.GetDocumentStore().LightweightSession())
			{
				model.PreviousCrimes = session.Query<CommitedCrime>().Where(x => x.StreamId == streamId).ToList();
			}
			return View(model);
		}

		public IActionResult GetAllEvents(Guid streamId)
		{
			var model = new GetAllEventsModel();

			using (var session = this.database.GetDocumentStore().LightweightSession())
			{
				model.Events = session.Events.FetchStream(streamId).Select(x => new Models.GetAllEventsModel.GetAllEventModel
				{
					EventId = x.Id,
					StreamId = x.StreamId,
					SequenceNumber = x.Sequence,
					EventName = x.ToString()
				});
			}
			return View(model);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
