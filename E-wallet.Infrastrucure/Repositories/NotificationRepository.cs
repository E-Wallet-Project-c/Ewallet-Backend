using E_wallet.Domain.Context;
using E_wallet.Domain.Entities;
using E_wallet.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_wallet.Infrastrucure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext _Context;

        public NotificationRepository(ApplicationDbContext Context)
        {
            _Context = Context;
        }

        public async Task<Notification?> AddNotification(Notification notification, CancellationToken ct)
        {
            notification.IsActive = true;
            notification.Id = 18;
            await _Context.Notifications.AddAsync(notification,ct);
            await _Context.SaveChangesAsync(ct);
            return notification;
        }




        public async Task<List<Notification>> GetNotificationByUserId(int userId,int PageNumber,int MaxItems, CancellationToken ct)
        {
            return await _Context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId && n.IsActive == true)
                .ToListAsync(ct);
        }



        public async Task<Notification?> GetById(int WallletId, CancellationToken ct)
        {
            return await _Context.Notifications
                .AsNoTracking()
                .Where(w => w.Id == WallletId && w.IsActive == true)
                .SingleOrDefaultAsync(ct);
        }
        public async Task<Notification> DeleteNotification (int Id, CancellationToken ct)
        {
            var notification = await _Context.Notifications.Where(n => n.Id == Id && n.IsDeleted == true).SingleOrDefaultAsync(ct);
            if ( notification == null)
            {
                return null;
            }
            notification.IsActive = false;
            notification.IsDeleted = true;
            notification.UpdatedAt = DateTime.Now;
            await _Context.SaveChangesAsync(ct);
            return notification;
        }

        public async Task<Notification> SetAsRead(int Id, CancellationToken ct)
        {
            var notification = await _Context.Notifications.Where(n=> n.Id == Id &&  n.IsActive==true).SingleOrDefaultAsync(ct);
            if ( notification == null)
            {
                return null;
            }
            notification.IsRead = true;
            notification.UpdatedAt = DateTime.Now;
            await _Context.SaveChangesAsync(ct);
            return notification;
        }

        public async Task<int?> UnreadNotificationCount(int Id, CancellationToken ct)
        {
            var notification = await _Context.Notifications.Where(n => n.Id == Id  && n.IsActive == true).SingleOrDefaultAsync(ct);
            if (notification == null)
            {
                return null;
            }

            var count = await _Context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == Id && n.IsRead == false && n.IsActive == true && n.IsDeleted == false)
                .CountAsync(ct);
            return count;

        }

    }
}
