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
    public class TestController : ControllerBase
    {
        public TestController(ILogger<TestController> logger, IDapperService dapperService)
        {
            _logger = logger;
            _dapperService = dapperService;
        }

        private readonly ILogger<TestController> _logger;
        private readonly IDapperService _dapperService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Test_Table>>> GetAllTest()
        {
            // Execute query and collect the results
            var query = @$"
                SELECT 
                    `{nameof(Test_Table.One1)}`, 
                    `{nameof(Test_Table.Two2)}`,
                    `{nameof(Test_Table.Three3)}`,
                    `{nameof(Test_Table.Four4)}`
                FROM `{nameof(Test_Table)}`
            ";
            var results = await _dapperService.GetAllAsync<Test_Table>(query, null);

            // Return an OK (200) response with results
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Test_Table>> GetTest(string id)
        {
            // Add parameters to bind
            var dbparams = new DynamicParameters();
            dbparams.Add("Id", id, DbType.String);

            // Execute query with bound parameters and collect the result
            var query = @$"
                SELECT 
                    `{nameof(Test_Table.One1)}`, 
                    `{nameof(Test_Table.Two2)}`,
                    `{nameof(Test_Table.Three3)}`,
                    `{nameof(Test_Table.Four4)}`
                FROM `{nameof(Test_Table)}`
                WHERE `{nameof(Test_Table.One1)}` = @Id
            "; 
            var result = await _dapperService.GetAsync<Test_Table>(query, dbparams);

            // Return not found if no result
            if (result == default)
                return NotFound();

            // Return an OK response with result
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Test_Table>> PostTest([FromBody] Test_Table testItem)
        {
            // Add parameters to bind
            var dbparams = new DynamicParameters();
            dbparams.Add("One", testItem.One1, DbType.String);
            dbparams.Add("Two", testItem.Two2, DbType.String);
            dbparams.Add("Three", testItem.Three3, DbType.String);
            dbparams.Add("Four", testItem.Four4, DbType.String);

            // Execute the query with bound parameters
            var query = @$"
                INSERT INTO `{nameof(Test_Table)}` (
                    `{nameof(Test_Table.One1)}`,
                    `{nameof(Test_Table.Two2)}`,
                    `{nameof(Test_Table.Three3)}`,
                    `{nameof(Test_Table.Four4)}`
                ) VALUES (@One, @Two, @Three, @Four);
            ";
            await _dapperService.InsertAsync<int>(query, dbparams);

            // Log the insert becuase it is important
            _logger.LogInformation($"{nameof(Test_Table)} item with {nameof(Test_Table.One1)} '{testItem.One1}' was inserted");

            // Return the created at action for the new item inserted
            return CreatedAtAction(nameof(PostTest), new { id = testItem.One1 }, testItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTest(string id, [FromBody] Test_Table testItem)
        {
            // Return bad request if IDs do not match
            if (id != testItem.One1)
                return BadRequest();

            // Add parameters to bind
            var dbparams = new DynamicParameters();
            dbparams.Add("One", testItem.One1, DbType.String);
            dbparams.Add("Two", testItem.Two2, DbType.String);
            dbparams.Add("Three", testItem.Three3, DbType.String);
            dbparams.Add("Four", testItem.Four4, DbType.String);

            // Execute the query with bound parameters
            var query = @$"
                UPDATE `{nameof(Test_Table)}` SET
                    `{nameof(Test_Table.One1)}` = @One,
                    `{nameof(Test_Table.Two2)}` = @Two,
                    `{nameof(Test_Table.Three3)}` = @Three,
                    `{nameof(Test_Table.Four4)}` = @Four
                WHERE `{nameof(Test_Table.One1)}` = @One
            ";
            var result = await _dapperService.ExecuteAsync(query, dbparams);

            // Return not found if no result
            if (result == 0)
                return NotFound();

            // Log the update becuase it is important
            _logger.LogInformation($"{nameof(Test_Table)} item with {nameof(Test_Table.One1)} '{id}' was updated");

            // Return nothing if successful for update
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest(string id)
        {
            // Add parameters to bind
            var dbparams = new DynamicParameters();
            dbparams.Add("Id", id, DbType.String);

            // Execute the query with bound parameters
            var query = $"DELETE FROM `{nameof(Test_Table)}` WHERE `{nameof(Test_Table.One1)}` = @Id";
            var result = await _dapperService.ExecuteAsync(query, dbparams);

            // Return not found in no records affected
            if (result == 0)
                return NotFound();

            // Log the delete becuase it is important
            _logger.LogInformation($"{nameof(Test_Table)} item with {nameof(Test_Table.One1)} '{id}' was deleted");

            // Return nothing if successful for delete
            return NoContent();
        }
    }
}
