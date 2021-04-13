using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace VodafoneCreditTransfer.Services
{
    public class DapperMySqlService : IDapperService
    {
        public DapperMySqlService(IConfiguration config)
        {
            _config = config;
        }

        private readonly IConfiguration _config;
        private const string Connectionstring = "DefaultConnection";

        public Task<int> ExecuteAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
            return db.ExecuteAsync(sp, parms, commandType: commandType);
        }

        public async Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
            var result = await db.QueryAsync<T>(sp, parms, commandType: commandType);
            return result.FirstOrDefault();
        }

        public Task<IEnumerable<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
            return db.QueryAsync<T>(sp, parms, commandType: commandType);
        }

        public DbConnection GetDbconnection()
        {
            return new MySqlConnection(_config.GetConnectionString(Connectionstring));
        }

        public async Task<T> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            T result;
            using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    var queryResult = await db.QueryAsync<T>(sp, parms, commandType: commandType, transaction: tran);
                    result = queryResult.FirstOrDefault();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public async Task<T> UpdateAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            T result;
            using IDbConnection db = new MySqlConnection(_config.GetConnectionString(Connectionstring));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    var queryResult = await db.QueryAsync<T>(sp, parms, commandType: commandType, transaction: tran);
                    result = queryResult.FirstOrDefault();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }
    }
}
