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
        Task<Result<NotificationResponse>> AddNotification(NotificationRequest notification);
        Task<Result<List<NotificationResponse>>> GetAllNotifications();

        Task<Result<List<NotificationResponse>>> GetUserNotifications(int userId, string? type);

        Task<Result<NotificationResponse>> UpdateUserNotifications(int Id,NotificationRequest _notification);

        Task<Result<NotificationResponse>> DeleteUserNotification(int Id);
        Task<Result<NotificationResponse>> GetById(int Id);

        Task<Result> AddAndSendAsync(NotificationRequest request);
    }
}
