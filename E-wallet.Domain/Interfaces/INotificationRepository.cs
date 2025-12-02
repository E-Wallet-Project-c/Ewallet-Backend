using E_wallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification> AddNotification(Notification notification, CancellationToken ct);
        Task<List<Notification>> GetNotificationByUserId(int userId, int PageNumber, int MaxItems, CancellationToken ct);
        Task<Notification> DeleteNotification(int Id, CancellationToken ct);
        Task<Notification> GetById(int WallletId, CancellationToken ct);
        Task<Notification> SetAsRead(int Id, CancellationToken ct);
        Task<int?> UnreadNotificationCount(int Id, CancellationToken ct);
    }
}
