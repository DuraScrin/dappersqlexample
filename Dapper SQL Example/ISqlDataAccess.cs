using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_SQL_Example
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connStringName);
        Task SaveData<T>(string storedProcedure, T parameters, string connStringName);
    }
}
