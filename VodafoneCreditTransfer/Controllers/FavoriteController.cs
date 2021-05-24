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
    }
}
