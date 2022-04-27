using System;
using System.Diagnostics;
using MartenPlayground.DataAccess.Databases;
using Microsoft.AspNetCore.Mvc;
using MartenPlayground.Models;
using MartenPlayground.DataAccess.Models;
using System.Linq;

namespace MartenPlayground.Controllers
{
	public class DocumentStoreController : Controller
	{
		readonly IDatabase database;

		public DocumentStoreController(IDatabase database)
		{
			this.database = database;
		}
		public IActionResult Index()
		{
			var model = new HomeModel();
			
			using (var session = this.database.GetDocumentStore().LightweightSession())
			{
				model.People = session.Query<User>()
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
			using (var session = this.database.GetDocumentStore().LightweightSession())
			{
				session.Store<User>(new User(){Name = name});
				session.SaveChanges();
			}
			return Redirect("/documentstore/index");
		}

		[HttpPost]
		public IActionResult CommitCrime(Guid userId, string crime)
		{

			using (var session = this.database.GetDocumentStore().LightweightSession())
			{
				session.Store<Crime>(new Crime{ UserId = userId, CrimeName = crime});
				session.SaveChanges();
			}
			return Redirect("/documentstore/index");
		}

		public IActionResult PreviousCrimes(Guid userId)
		{
			var model = new PreviousCrimesModel();
			using (var session = this.database.GetDocumentStore().LightweightSession())
			{
				model.PreviousCrimes = session.Query<Crime>().Where(x => x.UserId == userId)
				.Select(x => new CommitedCrime() {Id = x.Id, Crime = x.CrimeName}).ToList();
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
