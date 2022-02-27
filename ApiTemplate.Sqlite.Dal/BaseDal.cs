using ApiTemplate.Bll.IDal;
using Bing.NetFramework.DbConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Dapper;

namespace ApiTemplate.Sqlite.Dal
{
    public class BaseDal : BaSQLiteConnection, IBaseDal
    {
        public bool Delete<T>(T t) where T : class
        {
            using (var conn = GetConnectionForBA())
            {
                return conn.Delete<T>(t);
            }
        }

        public bool Delete<T>(List<T> t) where T : class
        {
            using (var conn = GetConnectionForBA())
            {
                return conn.Delete<List<T>>(t);
            }
        }

        public T Get<T>(string id) where T : class
        {
            using (var conn = GetConnectionForBA())
            {
                return conn.Get<T>(id);
            }
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            using (var conn = GetConnectionForBA())
            {
                return conn.GetAll<T>();
            }
        }

        public bool Insert<T>(T t) where T : class
        {
            using (var conn = GetConnectionForBA())
            {
                return conn.Insert<T>(t) > 0;
            }
        }

        public bool Insert<T>(List<T> t) where T : class
        {
            using (var conn = GetConnectionForBA())
            {
                return conn.Insert<List<T>>(t) > 0;
            }
        }

        public bool Update<T>(T t) where T : class
        {
            using (var conn = GetConnectionForBA())
            {
                return conn.Update<T>(t);
            }
        }

        public bool Update<T>(List<T> t) where T : class
        {
            using (var conn = GetConnectionForBA())
            {
                return conn.Update<List<T>>(t);
            }
        }
    }
}
