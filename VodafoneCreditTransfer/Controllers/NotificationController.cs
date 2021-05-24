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
    }
}
