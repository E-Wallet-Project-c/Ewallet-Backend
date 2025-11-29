using E_wallet.Application.Dtos.Request;
using E_wallet.Application.Dtos.Response;
using E_wallet.Application.Interfaces;
using E_wallet.Application.Mappers;
using E_wallet.Domain.Entities;
using E_wallet.Domain.IHelpers;
using E_wallet.Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;

namespace E_wallet.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailHelper _emailHelper;
        private readonly ISMSHelper _smsHelper;
        private readonly IProfileRepository _profileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<E_wallet.Applications.Hubs.AppHub> _hubContext;

    

        public NotificationService(
            INotificationRepository notificationRepository,
            IEmailHelper emailHelper,
            ISMSHelper smsHelper,
            IProfileRepository profileRepository,
            IUserRepository userRepository,
            IHubContext<E_wallet.Applications.Hubs.AppHub> hubContext)
        {
            _notificationRepository = notificationRepository;
            _emailHelper = emailHelper;
            _smsHelper = smsHelper;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _hubContext = hubContext;
        }


      
        public async Task<List<NotificationResponse>?> GetUserNotifications(int UserId)
        {
            if (await _userRepository.GetByIdAsync(UserId) == null)
            {
                return null;
            }

            List<Notification> notifications;

                notifications = await _notificationRepository.GetNotificationByUserId(UserId);

            if ( notifications.Count == 0)
                return null;

            return NotificationMapper.ToNotificationDtoList(notifications);
        }


        public  async Task<NotificationResponse?> DeleteUserNotification(int Id)
        {
            if (await _notificationRepository.GetById(Id) == null)
            {
                return null;
            }

            var deleted = await _notificationRepository.DeleteNotification(Id);

            if (deleted == null)
                return null;

            return 
                NotificationMapper.ToNotificationDto(deleted);
        }

    

      

        public async Task AddAndSendAsync(NotificationRequest request)
        {

            var profile = await _profileRepository.GetByUserIdAsync(request.UserId);

            if (profile == null)
                return;

            var user = await _userRepository.GetByIdAsync(request.UserId); 

            if (user == null)
                return ;

            var inAppNotification = NotificationMapper.ToNotificationEntity(request);
         


            var inAppSaved = await _notificationRepository.AddNotification(inAppNotification);

            if (inAppSaved is null)
                return;

          
                var smsNotification = NotificationMapper.ToNotificationEntity(request);
        
                smsNotification.UserId = request.UserId;

               

                await _hubContext.Clients
            .User(request.UserId.ToString())
            .SendAsync("NotificationReceived", NotificationMapper.ToNotificationDto(smsNotification));

                //if (!string.IsNullOrWhiteSpace(profile.Phone))
                {
                    await _smsHelper.SendSmsAsync(
                        "+962 7 9989 5351",
                        request.Content);
                }
            }
            
        }


    }

