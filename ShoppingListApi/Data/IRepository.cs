namespace ShoppingListApi.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByName(string name);
        Task Save(T t);
        Task Delete(string name);
    }
}