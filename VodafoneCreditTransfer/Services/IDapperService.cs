using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace VodafoneCreditTransfer.Services
{
    public interface IDapperService
    {
        DbConnection GetDbconnection();
        Task<T> GetAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);
        Task<IEnumerable<T>> GetAllAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);
        Task<int> ExecuteAsync(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);
        Task<T> InsertAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);
        Task<T> UpdateAsync<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text);
    }
}
