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
        Task<Notification> AddNotification(Notification notification);
        Task<List<Notification>> GetAllNotifications();
        Task<List<Notification>> GetNotificationByUserId(int UserId);

        Task<List<Notification>> GetByUserIdAndType(int userId, String Type);

        Task<Notification> UpdateNotification(Notification notification);

        Task<Notification> DeleteNotification(int Id);
        Task<Notification> GetById(int Id);
    }
}
