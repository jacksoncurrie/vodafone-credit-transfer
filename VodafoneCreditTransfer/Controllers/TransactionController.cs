using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using VodafoneCreditTransfer.Models;
using VodafoneCreditTransfer.Services;

namespace VodafoneCreditTransfer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        public TransactionController(ILogger<TransactionController> logger, IDapperService dapperService)
        {
            _logger = logger;
            _dapperService = dapperService;
        }

        private readonly ILogger<TransactionController> _logger;
        private readonly IDapperService _dapperService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sharing_Transactions_>>> GetAllAccounts()
        {
            // Execute query and collect the results
            var query = @$"
                SELECT 
                    `{nameof(Sharing_Transactions_.Transaction_No_)}`, 
                    `{nameof(Sharing_Transactions_.Transaction_Type)}`,
                    `{nameof(Sharing_Transactions_.From_Number_)}`,
                    `{nameof(Sharing_Transactions_.To_Number_)}`,
                    `{nameof(Sharing_Transactions_.Credit_Transfered)}`,
                    `{nameof(Sharing_Transactions_.Minutes_Transfered)}`,
                    `{nameof(Sharing_Transactions_.Data_Transferred_MB)}`,
                    `{nameof(Sharing_Transactions_.Date_and_Time)}`,
                    `{nameof(Sharing_Transactions_.Requested)}?`,
                    `{nameof(Sharing_Transactions_.Request_accepted)}?`
                FROM `{nameof(Sharing_Transactions_)}`
            ";
            var results = await _dapperService.GetAllAsync<Sharing_Transactions_>(query, null);

            // Return an OK (200) response with results
            return Ok(results);
        }
    }
}
