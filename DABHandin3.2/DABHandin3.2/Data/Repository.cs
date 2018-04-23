using System.Data.Entity;
using DABHandin3._2.Models;

namespace DABHandin3._2.Data
{
    public class Entity { } //Base class for repository

    public interface IRepository<T> where T : Entity
    {
        void Create(T t);
        T Read(int id);
        DbSet<T> ReadAll();
        void Update(int id, T t);
        void Delete(T t);
    }


    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly Context _context;

        public Repository(Context context)
        {
            _context = context;
        }

        public void Create(T t)
        {
            _context.Entry<T>(t).State = EntityState.Added;
        }

        public T Read(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public DbSet<T> ReadAll()
        {
            return _context.Set<T>();
        }

        public void Update(int id, T t)
        {
            _context.Entry<T>(t).State = EntityState.Modified;
        }

        public void Delete(T t)
        {
            _context.Entry<T>(t).State = EntityState.Deleted;
        }
    }
}