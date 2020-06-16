using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.UI.Services
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IList<T>> Get(string url);
        Task<T> Get(string url, int id);
        Task<bool> Create(string url, T obj);
        Task<bool> Update(string url, T obj, int id);
        Task<bool> Delete(string url, int id);
    }
}
