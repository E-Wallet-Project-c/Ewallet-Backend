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

        [HttpGet("GetUserNotifications/{userId}/{PageNumber}/{MaxItem}")]
        public async Task<IActionResult> GetUserNotifications([FromRoute] int userId, [FromRoute] int PageNumber, [FromRoute] int MaxItems, CancellationToken ct)
        {
            try {
                var result = await _NotificationService.GetUserNotifications(userId,PageNumber,MaxItems,ct);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
           
        }


        [HttpDelete("DeleteNotification/{Id}")]
        public async Task<IActionResult> DeleteNotification([FromRoute]int Id, CancellationToken ct)
        {
            try
            {
                var response = await _NotificationService.DeleteUserNotification(Id,ct);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");

            }
        }


        [HttpGet("UserUnReadCount/{userId}")]
        public async Task<IActionResult> GetUserUnReadCount([FromRoute] int userId,CancellationToken ct)
        {
            try
            {
                var response = await _NotificationService.UnReadNotificationCount(userId, ct);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

        [HttpGet("SetASRead/{Id}")]
        public async Task<IActionResult> SetAsRead([FromRoute] int Id, CancellationToken ct)
        {
            try
            {
                var response = await _NotificationService.SetAsRead(Id, ct);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
        }

    }
}
