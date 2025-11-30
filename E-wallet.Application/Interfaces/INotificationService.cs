using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Domain.Entities;
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
        Task<List<NotificationResponse>?> GetUserNotifications(int UserId, CancellationToken ct);
        Task<NotificationResponse?> DeleteUserNotification(int Id, CancellationToken ct);
        Task AddAndSendAsync(NotificationRequest request, CancellationToken ct);

        Task<Notification> SetAsRead(int Id, CancellationToken ct);

        Task<int?> UnReadNotificationCount(int UserId, CancellationToken ct);
    }
}
