using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Data;
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

        [HttpGet("{number}")]
        public async Task<ActionResult<Account_Details_TBL>> GetAccount(string number)
        {
            // Add parameters to bind
            var dbparams = new DynamicParameters();
            dbparams.Add("Number", number, DbType.String);

            // Execute query with bound parameters and collect the result
            var query = @$"
                SELECT 
                    `{nameof(Account_Details_TBL.CustomerID)}`, 
                    `{nameof(Account_Details_TBL.Mobile_or_Landline_Number_)}`,
                    `{nameof(Account_Details_TBL.Credit_Balance_)}`,
                    `{nameof(Account_Details_TBL.Minutes_Balance)}`,
                    `{nameof(Account_Details_TBL.Data_Balance_In_MB)}`,
                    `{nameof(Account_Details_TBL.Allow_Credit_transfer_request)}`,
                    `{nameof(Account_Details_TBL.On_contract)}`,
                    `{nameof(Account_Details_TBL.Allow_Notifications)}`,
                    `{nameof(Account_Details_TBL.Current_Notification_Count)}`,
                    `{nameof(Account_Details_TBL.Is_Transfer_Verified)}`
                FROM `{nameof(Account_Details_TBL)}`
                WHERE `{nameof(Account_Details_TBL.Mobile_or_Landline_Number_)}` = @Number
            ";
            var result = await _dapperService.GetAsync<Account_Details_TBL>(query, dbparams);

            // Return not found if no result
            if (result == default)
                return NotFound();

            // Return an OK response with result
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<bool>> Login([FromBody] Login body)
        {
            // Add parameters to bind
            var dbparams = new DynamicParameters();
            dbparams.Add("Number", body.Number, DbType.Int32);
            dbparams.Add("Password", body.Password, DbType.String);

            // Execute query with bound parameters and collect the result
            var query = @$"
                SELECT `{nameof(Account_Details_TBL.CustomerID)}`
                FROM `{nameof(Account_Details_TBL)}`
                WHERE `{nameof(Account_Details_TBL.Mobile_or_Landline_Number_)}` = @Number
                AND `{nameof(Account_Details_TBL.Account_Password_)}` = @Password
            ";
            var result = await _dapperService.GetAsync<int?>(query, dbparams);

            // Return not found if no result
            if (result == default)
                return Ok(false);

            // Return an OK response with result
            return Ok(true);
        }
    }
}
