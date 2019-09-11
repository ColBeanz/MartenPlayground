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
	public class HomeController : Controller
	{
		readonly IDatabase database;

		public HomeController(IDatabase database)
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
					});
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
			var personCreated = new PersonCreated();
			var nameChanged = new ChangeName { Name = name };

			using (var session = this.database.GetDocumentStore().LightweightSession())
			{
				session.Events.StartStream<Person>(Guid.NewGuid(), personCreated, nameChanged);
				session.SaveChanges();
			}
			return Redirect("/home/index");
		}

		[HttpPost]
		public IActionResult ChangeName(Guid streamId, string name)
		{
			var changeName = new ChangeName { Id = Guid.NewGuid(), Name = name };

			using (var session = this.database.GetDocumentStore().LightweightSession())
			{
				session.Events.Append(streamId, changeName);
				session.SaveChanges();
			}
			return Redirect("/home/index");
		}

		public IActionResult PreviousNames(Guid streamId)
		{
			var model = new PreviousNamesModel();

			using (var session = this.database.GetDocumentStore().LightweightSession())
			{
				model.PreviousNames = session.Query<PreviousName>().Where(x => x.StreamId == streamId).ToList();
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
