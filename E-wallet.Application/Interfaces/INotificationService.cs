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
        Task<List<NotificationResponse>?> GetUserNotifications(int UserId);
        Task<NotificationResponse?> DeleteUserNotification(int Id);
        Task AddAndSendAsync(NotificationRequest request);
    }
}
