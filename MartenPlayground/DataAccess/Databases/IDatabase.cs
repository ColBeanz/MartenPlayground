using Marten;

namespace MartenPlayground.DataAccess.Databases
{
	public interface IDatabase
	{
		IDocumentStore GetDocumentStore();
	}
}