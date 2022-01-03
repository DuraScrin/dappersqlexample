using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Dapper_SQL_Example
{
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        public SqlDataAccess(IConfiguration config, ILogger<ISqlDataAccess> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connStringName)
        {
            string connString = _config.GetConnectionString(connStringName);

            using (IDbConnection connection = new SqlConnection(connString))
            {
                var rows = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return rows.ToList();
            }
        }
        public async Task SaveData<T>(string storedProcedure, T parameters, string connStringName)
        {
            string connString = _config.GetConnectionString(connStringName);

            using (IDbConnection connection = new SqlConnection(connString))
            {
                try
                {
                    await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                }
                catch (SqlException ex)
                {
                    _logger.LogError(ex, "SqlDataAccess: " + ex.Message);
                }
            }
        }
    }
}
