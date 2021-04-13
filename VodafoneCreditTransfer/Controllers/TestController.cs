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
        public async Task<ActionResult<IEnumerable<Test>>> GetAllTest()
        {
            // Execute query and collect the results
            var query = @$"
                SELECT 
                    `{nameof(Test.TestId)}`, 
                    `{nameof(Test.TestName)}`,
                    `{nameof(Test.TestDate)}`,
                    `{nameof(Test.TestCheck)}`
                FROM `{nameof(Test)}`
            ";
            var results = await _dapperService.GetAllAsync<Test>(query, null);

            // Return an OK (200) response with results
            return Ok(results);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Test>> GetTest(int id)
        {
            // Add parameters to bind
            var dbparams = new DynamicParameters();
            dbparams.Add("Id", id, DbType.Int32);

            // Execute query with bound parameters and collect the result
            var query = @$"
                SELECT 
                    `{nameof(Test.TestId)}`, 
                    `{nameof(Test.TestName)}`,
                    `{nameof(Test.TestDate)}`,
                    `{nameof(Test.TestCheck)}`
                FROM `{nameof(Test)}`
                WHERE `{nameof(Test.TestId)}` = @Id
            "; 
            var result = await _dapperService.GetAsync<Test>(query, null);

            // Return not found if no result
            if (result == default)
                return NotFound();

            // Return an OK response with result
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Test>> PostTest(Test testItem)
        {
            // Add parameters to bind
            var dbparams = new DynamicParameters();
            dbparams.Add("Name", testItem.TestName, DbType.String);
            dbparams.Add("Date", testItem.TestDate, DbType.DateTime);
            dbparams.Add("Check", testItem.TestCheck, DbType.Boolean);

            // Execute the query with bound parameters
            var query = @$"
                INSERT INTO `{nameof(Test)}` (
                    `{nameof(Test.TestName)}`,
                    `{nameof(Test.TestDate)}`,
                    `{nameof(Test.TestCheck)}`
                ) VALUES (@Name, @Date, @Check)
            ";
            var result = await _dapperService.InsertAsync<Test>(query, dbparams);

            // Log the insert becuase it is important
            _logger.LogInformation($"{nameof(Test)} item with {nameof(Test.TestId)} '{result.TestId}' was inserted");

            // Return the created at action for the new item inserted
            return CreatedAtAction(nameof(PostTest), new { id = result.TestId }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutTest(int id, Test testItem)
        {
            // Return bad request if IDs do not match
            if (id != testItem.TestId)
                return BadRequest();

            // Add parameters to bind
            var dbparams = new DynamicParameters();
            dbparams.Add("Id", testItem.TestName, DbType.Int32);
            dbparams.Add("Name", testItem.TestName, DbType.String);
            dbparams.Add("Date", testItem.TestDate, DbType.DateTime);
            dbparams.Add("Check", testItem.TestCheck, DbType.Boolean);

            // Execute the query with bound parameters
            var query = @$"
                UPDATE `{nameof(Test)}` SET
                    `{nameof(Test.TestName)}` = @Name,
                    `{nameof(Test.TestDate)}` = @Date,
                    `{nameof(Test.TestCheck)}` = @Check
                WHERE `{nameof(Test.TestId)}` = @Id
            ";
            var result = await _dapperService.UpdateAsync<Test>(query, dbparams);

            // Return not found if no result
            if (result == default)
                return NotFound();

            // Log the update becuase it is important
            _logger.LogInformation($"{nameof(Test)} item with {nameof(Test.TestId)} '{id}' was updated");

            // Return nothing if successful for update
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTest(int id)
        {
            // Add parameters to bind
            var dbparams = new DynamicParameters();
            dbparams.Add("Id", id, DbType.Int32);

            // Execute the query with bound parameters
            var query = $"DELETE FROM `{nameof(Test)}` WHERE `{nameof(Test.TestId)}` = @Id";
            var result = await _dapperService.ExecuteAsync(query, dbparams);

            // Return not found in no records affected
            if (result == 0)
                return NotFound();

            // Log the delete becuase it is important
            _logger.LogInformation($"{nameof(Test)} item with {nameof(Test.TestId)} '{id}' was deleted");

            // Return nothing if successful for delete
            return NoContent();
        }
    }
}
