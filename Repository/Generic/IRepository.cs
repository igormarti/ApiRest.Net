using RestApiWithDontNet.Models;
using RestApiWithDontNet.Models.Base;

namespace RestApiWithDontNet.Repository.Generic
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Create(T item);

        T Update(long id, T newItem, T oldItem);

        T FindById(long id);

        List<T> FindAll();

        void Delete(long id);

        bool Exists(long id);
    }
}
