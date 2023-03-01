using SharedLibrary;

namespace DAL.Repositories
{
	public interface IRepository<T> where T : class
	{
		public T? GetById(int id);
		public T? FirstOrDefault(string filter);
		public IEnumerable<T> Find(string filter);
        public IEnumerable<T> GetAll();
		public Result Add(T entity);
		public Result Update(T entity);
		public Result Delete(T entity);
	}
}
