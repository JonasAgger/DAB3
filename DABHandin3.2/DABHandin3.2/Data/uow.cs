using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DABHandin3._2.Models;

namespace DABHandin3._2.Data
{
    public class UnitOfWork<T> where T : Entity
    {
        public Repository<T> Repo;

        private Context _db;

        public UnitOfWork(Context db)
        {
            _db = db;
            Repo = new Repository<T>(_db);
        }

        public void Commit()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

    }
}