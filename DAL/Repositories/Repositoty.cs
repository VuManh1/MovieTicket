using SharedLibrary;

namespace DAL.Repositories
{
	public interface IRepository<T> where T : class
	{
		public T? GetById(string id);
		public IEnumerable<T> GetAll();
		public T? FirstOrDefault(string filter);
		public Result Add(T entity);
		public Result Update(T entity);
		public Result Delete(string id);
	}
}
