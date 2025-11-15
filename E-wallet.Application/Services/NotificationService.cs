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

        #region Basic CRUD

        public async Task<Result<NotificationResponse>> AddNotification(NotificationRequest notification)
        {
            if (await _userRepository.GetByIdAsync(notification.UserId)==null)
            {
                return Result<NotificationResponse>.Failure("No user with the inserted Id");
            }
            var entity = NotificationMapper.ToNotificationEntity(notification);
            var saved = await _notificationRepository.AddNotification(entity);

            if (saved is null)
                return Result<NotificationResponse>.Failure("Something went wrong and the notification is not added, try again!");

            return Result<NotificationResponse>.Success(NotificationMapper.ToNotificationDto(saved));
        }

        public async Task<Result<List<NotificationResponse>>> GetAllNotifications()
        {
            var notifications = await _notificationRepository.GetAllNotifications();

            if ( notifications.Count == 0)
                return Result<List<NotificationResponse>>.Failure("No notifications are available!");

            return Result<List<NotificationResponse>>.Success(
                NotificationMapper.ToNotificationDtoList(notifications));
        }


        public async Task<Result<List<NotificationResponse>>> GetUserNotifications(int UserId, string? Type)
        {
            if (await _userRepository.GetByIdAsync(UserId) == null)
            {
                return Result<List<NotificationResponse>>.Failure("No user with the inserted Id");
            }

            List<Notification> notifications;

            if (string.IsNullOrWhiteSpace(Type))
                notifications = await _notificationRepository.GetNotificationByUserId(UserId);
            else
                notifications = await _notificationRepository.GetByUserIdAndType(UserId, Type);

            if ( notifications.Count == 0)
                return Result<List<NotificationResponse>>.Failure("This user has no notifications");

            return Result<List<NotificationResponse>>.Success(
                NotificationMapper.ToNotificationDtoList(notifications));
        }


        public async Task<Result<NotificationResponse>> UpdateUserNotifications(int Id, NotificationRequest request)
        {

            if (await _notificationRepository.GetById(Id) == null)
            {
                return Result<NotificationResponse>.Failure("No notification with this Id");
            }

            if (await _userRepository.GetByIdAsync(request.UserId) == null)
            {
                return Result<NotificationResponse>.Failure("No user with the inserted Id");
            }

            var entity = NotificationMapper.ToNotificationEntity(request);
            entity.Id = Id;

            var updated = await _notificationRepository.UpdateNotification(entity);

            if (updated == null)
                return Result<NotificationResponse>.Failure("Something went wrong and the notification is not updated, try again!");

            return Result<NotificationResponse>.Success(
                NotificationMapper.ToNotificationDto(updated));
        }

        public async Task<Result<NotificationResponse>> DeleteUserNotification(int Id)
        {
            if (await _notificationRepository.GetById(Id) == null)
            {
                return Result<NotificationResponse>.Failure("No notification with this Id");
            }

            var deleted = await _notificationRepository.DeleteNotification(Id);

            if (deleted == null)
                return Result<NotificationResponse>.Failure("Something went wrong and the notification is not deleted, try again!");

            return Result<NotificationResponse>.Success(
                NotificationMapper.ToNotificationDto(deleted));
        }

        public async Task<Result<NotificationResponse>> GetById(int Id)
        {
            var notification = await _notificationRepository.GetById(Id);

            if (notification == null)
                return Result<NotificationResponse>.Failure("No notification with this Id.");

            // mark as read

            notification.IsRead = true;

            return Result<NotificationResponse>.Success(
                NotificationMapper.ToNotificationDto(await _notificationRepository.UpdateNotification(notification)));
        }

        #endregion

        #region AddAndSendAsync (InApp + Email + SMS)

        public async Task<Result> AddAndSendAsync(NotificationRequest request)
        {
            var notifications = new List<Notification>();

            var profile = await _profileRepository.GetByUserIdAsync(request.UserId);

            if (profile == null)
                return Result.Failure("User profile not found.");

            var user = await _userRepository.GetByIdAsync(request.UserId); 

            if (user == null)
                return Result.Failure("User not found.");

            var inAppNotification = NotificationMapper.ToNotificationEntity(request);
            inAppNotification.Type = "InApp";


            var inAppSaved = await _notificationRepository.AddNotification(inAppNotification);

            if (inAppSaved is null)
                return Result.Failure("Failed to save in-app notification.");

            notifications.Add(inAppSaved);

           
            if (profile.EmailNotifications)
            {
                var emailNotification = NotificationMapper.ToNotificationEntity(request);
                emailNotification.Type = "Email";
                emailNotification.UserId = request.UserId;

                var emailSaved = await _notificationRepository.AddNotification(emailNotification);
                if (emailSaved is null)
                    return Result.Failure("Failed to save email notification.");

                notifications.Add(emailSaved);


                await _emailHelper.SendEmailAsync(
                    email: "mohammedsabuabdo@gmail.com",
                    EmailSubject: request.Event,
                    EmailContent: request.Content,
                    UserName: "User");

            }


            if (profile.SMSNotifications)
            {
                var smsNotification = NotificationMapper.ToNotificationEntity(request);
                smsNotification.Type = "SMS";
                smsNotification.UserId = request.UserId;

                var smsSaved = await _notificationRepository.AddNotification(smsNotification);
                if (smsSaved is null)
                    return Result.Failure("Failed to save SMS notification.");

                notifications.Add(smsSaved);
                await _hubContext.Clients
            .User(request.UserId.ToString())
            .SendAsync("NotificationReceived", NotificationMapper.ToNotificationDto(smsSaved));

                //if (!string.IsNullOrWhiteSpace(profile.Phone))
                {
                    await _smsHelper.SendSmsAsync(
                        "+962 7 9989 5351",
                        request.Content);
                }
            }
            
            // 5) Return list
            return Result.Success();
        }

        #endregion
    }
}
