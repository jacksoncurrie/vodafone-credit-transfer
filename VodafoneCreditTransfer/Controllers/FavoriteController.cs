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
    [Route("[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        public FavoriteController(ILogger<FavoriteController> logger, IDapperService dapperService)
        {
            _logger = logger;
            _dapperService = dapperService;
        }

        private readonly ILogger<FavoriteController> _logger;
        private readonly IDapperService _dapperService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Favorite_Transfers>>> GetAllFavorites()
        {
            // Execute query and collect the results
            var query = @$"
                SELECT 
                    `{nameof(Favorite_Transfers.My_Number_)}`, 
                    `{nameof(Favorite_Transfers.Transfer_Recievers_Number_)}`
                FROM `{nameof(Favorite_Transfers)}`
            ";
            var results = await _dapperService.GetAllAsync<Favorite_Transfers>(query, null);

            // Return an OK (200) response with results
            return Ok(results);
        }

        [HttpGet("{number}")]
        public async Task<ActionResult<Favorite_Transfers>> GetMyFavorites(string number)
        {
            // Add parameters to bind
            var dbparams = new DynamicParameters();
            dbparams.Add("Number", number, DbType.String);

            // Execute query with bound parameters and collect the result
            var query = @$"
                SELECT 
                    `{nameof(Favorite_Transfers.My_Number_)}`, 
                    `{nameof(Favorite_Transfers.Transfer_Recievers_Number_)}`
                FROM `{nameof(Favorite_Transfers)}`
                WHERE `{nameof(Favorite_Transfers.My_Number_)}` = @Number
            ";
            var result = await _dapperService.GetAllAsync<Favorite_Transfers>(query, dbparams);

            // Return an OK response with result
            return Ok(result);
        }
    }
}
