using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        /*
         * Test users: 
         * 532543 1monica423
         * 1234567 1chandler123
         */

        [HttpGet("{number}")]
        public async Task<ActionResult<Account>> GetAccount(int number)
        {
            // Add parameters to bind
            var dbparams = new DynamicParameters();
            dbparams.Add("Number", number, DbType.Int32);

            // Execute query with bound parameters and collect the result
            var query = @$"
                SELECT
                    CONCAT(Customer_TBL.FName, ' ', Customer_TBL.LName) AS Name,
                    Customer_TBL.Address AS Address,
                    Account_Details_TBL.Mobile_or_Landline_Number_ AS Number,
                    Account_Details_TBL.Credit_Balance_ AS Balance
                FROM Account_Details_TBL
                    JOIN Customer_TBL ON Customer_TBL.CustomerID = Account_Details_TBL.CustomerID
                WHERE Mobile_or_Landline_Number_ = @Number
            ";
            var result = await _dapperService.GetAsync<Account>(query, dbparams);

            // Return not found if no result
            if (result == default)
                return NotFound();

            // Return an OK response with result
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<bool>> Login([FromBody] LoginDetails body)
        {
            // Add parameters to bind
            var dbparams = new DynamicParameters();
            dbparams.Add("Number", body.Number, DbType.Int32);
            dbparams.Add("Password", body.Password, DbType.String);

            // Execute query with bound parameters and collect the result
            var query = @$"
                SELECT CustomerID
                FROM Account_Details_TBL
                WHERE Mobile_or_Landline_Number_ = @Number
                AND Account_Password_ = @Password
            ";
            var result = await _dapperService.GetAsync<int?>(query, dbparams);

            var resultString = result == default ? "unsuccessfully" : "successfully";
            _logger.LogInformation($"User logged in {resultString} with number: {body.Number}");

            // Return not found if no result
            if (result == default)
                return Ok(false);

            // Return an OK response with result
            return Ok(true);
        }
    }
}
