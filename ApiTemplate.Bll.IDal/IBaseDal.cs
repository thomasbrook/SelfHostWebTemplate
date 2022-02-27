using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTemplate.Bll.IDal
{
    public interface IBaseDal
    {
        T Get<T>(string id) where T : class;
        IEnumerable<T> GetAll<T>() where T : class;
        bool Insert<T>(T t) where T : class;
        bool Insert<T>(List<T> t) where T : class;
        bool Update<T>(T t) where T : class;
        bool Update<T>(List<T> t) where T : class;
        bool Delete<T>(T t) where T : class;
        bool Delete<T>(List<T> t) where T : class;
    }
}
