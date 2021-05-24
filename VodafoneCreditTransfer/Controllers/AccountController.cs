using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using VodafoneCreditTransfer.Models;
using VodafoneCreditTransfer.Services;

namespace VodafoneCreditTransfer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        public AccountController(ILogger<AccountController> logger, IDapperService dapperService)
        {
            _logger = logger;
            _dapperService = dapperService;
        }

        private readonly ILogger<AccountController> _logger;
        private readonly IDapperService _dapperService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account_Details_TBL>>> GetAllAccounts()
        {
            // Execute query and collect the results
            var query = @$"
                SELECT 
                    `{nameof(Account_Details_TBL.CustomerID)}`, 
                    `{nameof(Account_Details_TBL.Mobile_or_Landline_Number_)}`,
                    `{nameof(Account_Details_TBL.Account_Password_)}`,
                    `{nameof(Account_Details_TBL.Credit_Balance_)}`,
                    `{nameof(Account_Details_TBL.Minutes_Balance)}`,
                    `{nameof(Account_Details_TBL.Data_Balance_In_MB)}`,
                    `{nameof(Account_Details_TBL.Allow_Credit_transfer_request)}`,
                    `{nameof(Account_Details_TBL.On_contract)}`,
                    `{nameof(Account_Details_TBL.Allow_Notifications)}`,
                    `{nameof(Account_Details_TBL.Current_Notification_Count)}`,
                    `{nameof(Account_Details_TBL.Is_Transfer_Verified)}`
                FROM `{nameof(Account_Details_TBL)}`
            ";
            var results = await _dapperService.GetAllAsync<Account_Details_TBL>(query, null);

            // Return an OK (200) response with results
            return Ok(results);
        }
    }
}
