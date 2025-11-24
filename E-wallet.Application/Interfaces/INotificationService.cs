using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Application.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationResponse?> AddNotification(NotificationRequest notification);
        Task<List<NotificationResponse>?> GetAllNotifications();

        Task<NotificationResponse?> UpdateUserNotifications(int Id, NotificationRequest request);
        Task<List<NotificationResponse>?> GetUserNotifications(int UserId, string? Type);
        Task<NotificationResponse?> DeleteUserNotification(int Id);
        Task<NotificationResponse?> GetById(int Id);

        Task AddAndSendAsync(NotificationRequest request);
    }
}
