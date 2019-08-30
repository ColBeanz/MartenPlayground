using System;
using System.Diagnostics;
using MartenPlayground.DataAccess.Databases;
using MartenPlayground.DataAccess.Events;
using MartenPlayground.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using MartenPlayground.Models;

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
				model.People = session.Query<Person>();
			}

			return View(model);
		}

		public IActionResult AddUser()
		{
			var personCreated = new PersonCreated {Id = Guid.NewGuid(), Name = "Some name " + new Random().Next(1, 1000)};

			using (var session = this.database.GetDocumentStore().LightweightSession())
			{
				session.Events.StartStream<Person>(Guid.NewGuid(), personCreated);
				session.SaveChanges();
			}
			return View(new AddUserModel{ Person = new Person{Id = personCreated.Id, Name = personCreated.Name} });
		}

		[HttpPost]
		public IActionResult ChangeName(Guid streamId, string name)
		{
			var changeName = new ChangeName { Id = Guid.NewGuid(), Name = "Some name " + new Random().Next(1, 1000) };

			using (var session = this.database.GetDocumentStore().LightweightSession())
			{
				session.Events.Append(streamId, changeName);
				session.SaveChanges();
			}
			return Redirect("/home/index");
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
