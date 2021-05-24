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
    public class NotificationController : ControllerBase
    {
        public NotificationController(ILogger<NotificationController> logger, IDapperService dapperService)
        {
            _logger = logger;
            _dapperService = dapperService;
        }

        private readonly ILogger<NotificationController> _logger;
        private readonly IDapperService _dapperService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notifications_and_Requests>>> GetAllNotifications()
        {
            // Execute query and collect the results
            var query = @$"
                SELECT 
                    `{nameof(Notifications_and_Requests.RQ_Number_PK)}`, 
                    `{nameof(Notifications_and_Requests.Mobile_or_Landline_Number_)}`,
                    `{nameof(Notifications_and_Requests.Request_or_notifycation)}?`,
                    `{nameof(Notifications_and_Requests.Requested_From)}`,
                    `{nameof(Notifications_and_Requests.Notification_MSG)}`,
                    `{nameof(Notifications_and_Requests.Amount_Requested)}`,
                    `{nameof(Notifications_and_Requests.Credit_Type_Requested)}`,
                    `{nameof(Notifications_and_Requests.Notification_Recived)}?`
                FROM `{nameof(Notifications_and_Requests)}`
            ";
            var results = await _dapperService.GetAllAsync<Notifications_and_Requests>(query, null);

            // Return an OK (200) response with results
            return Ok(results);
        }

        [HttpGet("{number}")]
        public async Task<ActionResult<Notifications_and_Requests>> GetMyNotifications(string number)
        {
            // Add parameters to bind
            var dbparams = new DynamicParameters();
            dbparams.Add("Number", number, DbType.String);

            // Execute query with bound parameters and collect the result
            var query = @$"
                 SELECT 
                    `{nameof(Notifications_and_Requests.RQ_Number_PK)}`, 
                    `{nameof(Notifications_and_Requests.Mobile_or_Landline_Number_)}`,
                    `{nameof(Notifications_and_Requests.Request_or_notifycation)}?`,
                    `{nameof(Notifications_and_Requests.Requested_From)}`,
                    `{nameof(Notifications_and_Requests.Notification_MSG)}`,
                    `{nameof(Notifications_and_Requests.Amount_Requested)}`,
                    `{nameof(Notifications_and_Requests.Credit_Type_Requested)}`,
                    `{nameof(Notifications_and_Requests.Notification_Recived)}?`
                FROM `{nameof(Notifications_and_Requests)}`
                WHERE `{nameof(Notifications_and_Requests.Mobile_or_Landline_Number_)}` = @Number
            ";
            var result = await _dapperService.GetAllAsync<Notifications_and_Requests>(query, dbparams);

            // Return an OK response with result
            return Ok(result);
        }
    }
}
