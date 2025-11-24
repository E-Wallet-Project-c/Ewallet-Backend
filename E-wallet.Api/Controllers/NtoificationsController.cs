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

        [HttpPost("AddNotification")]
        public async Task<IActionResult> AddNotification([FromBody]NotificationRequest notification)
        {
            try
            {
                var response = await _NotificationService.AddNotification(notification);

                return Ok(response);
            }
            catch (Exception ex) {
                return StatusCode(500, $"{ex.Message}");
            }
            
        }

        [HttpGet("GetById/{Id}")]
        public async Task<IActionResult> GetById([FromRoute]int Id)
        {
            try
            {
                var response = await _NotificationService.GetById(Id);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");

            }
        }


        [HttpGet("GetAllNotifications")]
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                var response = await _NotificationService.GetAllNotifications();

              

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");

            }
        }



        [HttpGet("GetUserNotifications/{userId}")]
        public async Task<IActionResult> GetUserNotifications( int userId, [FromQuery] string? Type)
        {
            try {
                var result = await _NotificationService.GetUserNotifications(userId, Type);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{ex.Message}");
            }
           
        }







        [HttpPatch("UpadteNotification/{id}")]
        public async Task<IActionResult> UpadteNotification(int id,[FromBody]NotificationRequest notification)
        {
            try
            {
                var response = await _NotificationService.UpdateUserNotifications(id,notification);

             

                return Ok(response);
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
