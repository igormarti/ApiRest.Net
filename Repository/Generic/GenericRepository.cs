using Microsoft.EntityFrameworkCore;
using RestApiWithDontNet.Models;
using RestApiWithDontNet.Models.Base;
using RestApiWithDontNet.Models.Context;

namespace RestApiWithDontNet.Repository.Generic
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {

        private readonly MySqlContext _context;
        private readonly DbSet<T> _entity;

        public GenericRepository(MySqlContext context) { 
            _context = context;
            _entity = _context.Set<T>();
        }

        public T Create(T item)
        {
            _entity.Add(item);
            _context.SaveChanges();
            return item;
        }

        public void Delete(long id)
        {
            var result = FindById(id);
            _entity.Remove(result);
            _context.SaveChanges();
        }

        public bool Exists(long id)
        {
            return _entity.Any(u => u.Id.Equals(id));
        }

        public List<T> FindAll()
        {
            return _entity.ToList();
        }

        public T FindById(long id)
        {
            return _entity.SingleOrDefault(u => u.Id.Equals(id));
        }

        public T Update(long id, T newItem, T oldItem)
        {
            newItem.Id = oldItem.Id;
            _entity.Entry(oldItem).CurrentValues.SetValues(newItem);
            _context.SaveChanges();
            return newItem;
        }

        public List<T> findWithPageSearch(string query)
        {
            return _entity.FromSqlRaw<T>(query).ToList();
        }

        public int GetCount(string query)
        {
            var result = "";
            using (var connection = _context.Database.GetDbConnection())
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    result = command.ExecuteScalar().ToString();
                }
            }
            return int.Parse(result);
        }

    }
}
