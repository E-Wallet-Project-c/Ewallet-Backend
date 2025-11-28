using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Request.Auth;
using E_wallet.Domain.Entities;
using E_wallet.Application.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Mappers
{
    public class NotificationMapper
    {
        public static GenerateTokenRequest ToTokenRequest(User uesr)
        {
            return new GenerateTokenRequest
            {
                UserId = uesr.Id,
                FullName = uesr.FullName,
                Email = uesr.Email,
                Role = "User"
            };
        }

        public static Notification ToNotificationEntity(NotificationRequest _notification)
        {
            return new Notification { 
            UserId= _notification.UserId,
            Event= _notification.Event,
            Content= _notification.Content,
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            };
        }


     

        public static NotificationResponse ToNotificationDto(Notification _notification)
        {
            return new NotificationResponse
            {
                Id = _notification.Id,
                UserId = _notification.UserId,
                Event = _notification.Event,
                IsRead= _notification.IsRead,
                Content= _notification.Content,
                createdAt=_notification.CreatedAt.ToString(),
            };
        }
        public static List<NotificationResponse> ToNotificationDtoList(List<Notification> _notification)
        {
            var Response = new List<NotificationResponse>();
            foreach (Notification item in _notification)
            {
                NotificationResponse temp = ToNotificationDto(item);
                Response.Add(temp);
            }

            return Response;
          
        }
      
    }
}
