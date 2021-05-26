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
    public class TransferController : Controller
    {
        public TransferController(ILogger<TransferController> logger, IDapperService dapperService)
        {
            _logger = logger;
            _dapperService = dapperService;
        }

        private readonly ILogger<TransferController> _logger;
        private readonly IDapperService _dapperService;

        [HttpPost("{number}")]
        public async Task<ActionResult<bool>> Login(int number, [FromBody] Transfer body)
        {
            // Remove amount from user
            var removeDbparams = new DynamicParameters();
            removeDbparams.Add("Amount", body.Amount, DbType.Decimal);
            removeDbparams.Add("Number", body.FromNumber, DbType.Int32);
            var removeQuery = @$"
                UPDATE Account_Details_TBL
                SET Credit_Balance_ = Credit_Balance_ - @Amount
                WHERE Mobile_or_Landline_Number_ = @Number
            ";
            var removeResult = await _dapperService.ExecuteAsync(removeQuery, removeDbparams);
            if (removeResult == 0)
            {
                _logger.LogInformation($"Credit failed to transfer ${body.Amount} from {body.FromNumber} to {number}");
                return Ok(false);
            }

            // Add amount from user
            var addDbparams = new DynamicParameters();
            addDbparams.Add("Amount", body.Amount, DbType.Decimal);
            addDbparams.Add("Number", number, DbType.Int32);
            var addQuery = @$"
                UPDATE Account_Details_TBL
                SET Credit_Balance_ = Credit_Balance_ + @Amount
                WHERE Mobile_or_Landline_Number_ = @Number
            ";
            var addResult = await _dapperService.ExecuteAsync(addQuery, addDbparams);
            if (addResult == 0)
            {
                _logger.LogInformation($"Credit failed to transfer ${body.Amount} from {body.FromNumber} to {number}");
                return Ok(false);
            }

            _logger.LogInformation($"Credit of ${body.Amount} transfered from {body.FromNumber} to {number}");

            // Return result
            return Ok(true);
        }
    }
}
