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
    public class CustomerController : Controller
    {
        public CustomerController(ILogger<CustomerController> logger, IDapperService dapperService)
        {
            _logger = logger;
            _dapperService = dapperService;
        }

        private readonly ILogger<CustomerController> _logger;
        private readonly IDapperService _dapperService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer_TBL>>> GetAllCustomers()
        {
            // Execute query and collect the results
            var query = @$"
                SELECT 
                    `{nameof(Customer_TBL.CustomerID)}`, 
                    `{nameof(Customer_TBL.FName)}`,
                    `{nameof(Customer_TBL.LName)}`,
                    `{nameof(Customer_TBL.Address)}`
                FROM `{nameof(Customer_TBL)}`
            ";
            var results = await _dapperService.GetAllAsync<Customer_TBL>(query, null);

            // Return an OK (200) response with results
            return Ok(results);
        }
    }
}
