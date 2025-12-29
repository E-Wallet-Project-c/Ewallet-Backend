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


      
        public async Task<List<NotificationResponse>?> GetUserNotifications(int UserId,int PageNumber,int MaxItems, CancellationToken ct)
        { 
            if (UserId < 0)
            {
                return null;
            }
            if (await _userRepository.GetByIdAsync(UserId,ct) == null)
            {
                return null;
            }

            List<Notification> notifications;

                notifications = await _notificationRepository.GetNotificationByUserId(UserId,PageNumber,MaxItems,ct);

            if ( notifications.Count == 0)
                return null;

            return NotificationMapper.ToNotificationDtoList(notifications);
        }


        public  async Task<NotificationResponse?> DeleteUserNotification(int Id, CancellationToken ct)
        {
            if (Id < 0)
            {
                return null;
            }
            if (await _notificationRepository.GetById(Id,ct) == null)
            {
                return null;
            }

            var deleted = await _notificationRepository.DeleteNotification(Id,ct);

            if (deleted == null)
                return null;

            return 
                NotificationMapper.ToNotificationDto(deleted);
        }


        public async Task<int?> UnReadNotificationCount(int UserId, CancellationToken ct)
        {
            if(UserId < 0)
            {
                return null;
            }
            if (await _userRepository.GetByIdAsync(UserId,ct) == null)
            {
                return null;
            }
            return await _notificationRepository.UnreadNotificationCount(UserId, ct);
        }


        public async Task<Notification> SetAsRead(int Id, CancellationToken ct)
        {
            if (Id < 0)
            {
                return null;
            }
            return await _notificationRepository.SetAsRead(Id,ct);
        }



        public  async Task AddAndSendAsync(NotificationRequest request, CancellationToken ct)
        {

            var profile = await _profileRepository.GetByUserIdAsync(request.UserId);

            if (profile == null)
                return;

            var user = await _userRepository.GetByIdAsync(request.UserId,ct); 

            if (user == null)
                return ;

            var inAppNotification = NotificationMapper.ToNotificationEntity(request);
         


            var inAppSaved = await _notificationRepository.AddNotification(inAppNotification, ct);

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
                        "+962 7 8006 8648",
                        request.Content);
                }
            }
            
        }


    }

