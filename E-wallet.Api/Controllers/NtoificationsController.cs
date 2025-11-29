using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Services;
using E_wallet.Domain.Entities;
using E_wallet.Domain.IHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_wallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _NotificationService;

        public NotificationsController(INotificationService NotificationService)
        {
            _NotificationService = NotificationService;
        }

        [HttpGet("GetUserNotifications/{userId}")]
        public async Task<IActionResult> GetUserNotifications([FromRoute] int userId)
        {
            try {
                var result = await _NotificationService.GetUserNotifications(userId);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
           
        }


        [HttpDelete("DeleteNotification/{Id}")]
        public async Task<IActionResult> DeleteNotification([FromRoute]int Id)
        {
            try
            {
                var response = await _NotificationService.DeleteUserNotification(Id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");

            }
        }


    }
}
