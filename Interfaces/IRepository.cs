using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyTeam.Interfaces
{
	public interface IRepository<TEntity, TKey> where TEntity : class
	{
		Task<IEnumerable<TEntity>> GetAllAsync();
		Task<TEntity> GetByKeyAsync(TKey key);
		Task<TEntity> CreateAsync(TEntity entity);
		Task UpdateAsync(TEntity entity);
		Task DeleteAsync(TKey key);
		Task SaveAsync();
	}
}