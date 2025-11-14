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

      


        public async Task<Notification?> AddNotification(Notification notification)
        {
            notification.IsActive = true;
            notification.Id = 18;
            await _Context.Notifications.AddAsync(notification);
            await _Context.SaveChangesAsync();
            return notification;
        }



        public async Task<List<Notification>> GetAllNotifications()
        {
            return await _Context.Notifications
                .AsNoTracking()
                .Where(n => n.IsActive == true)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetNotificationByUserId(int userId)
        {
            return await _Context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId && n.IsActive == true)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetByUserIdAndType(int userId,String Type)
        {
            return await _Context.Notifications
                .AsNoTracking()
                .Where(n => n.UserId == userId && n.Type == Type && n.IsActive == true)
                .ToListAsync();
        }

        

        public async Task<Notification> GetById(int Id)
        {
           return  await _Context.Notifications.Where(n => n.Id == Id && n.IsActive==true).SingleOrDefaultAsync();
        }



        public async Task<Notification> UpdateNotification(Notification notification)
        {
            _Context.Notifications.Update(notification);
            await _Context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification> DeleteNotification (int Id)
        {
            var notification = await _Context.Notifications.Where(u => u.Id == Id).SingleOrDefaultAsync();
            notification.IsActive = false;
            await _Context.SaveChangesAsync();
            return notification;
        }


    }
}
