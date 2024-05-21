using HiClass.Application.Common.Exceptions.Invitations;
using HiClass.Application.Constants;
using HiClass.Application.Handlers.EntityHandlers.FeedbackHandlers.Commands.CreateFeedback;
using HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.CreateInvitation;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces.Services;
using HiClass.Application.Models.Invitations.CreateInvitation;
using HiClass.Application.Models.Invitations.Feedbacks.CreateFeedback;
using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Enums;
using HiClass.Infrastructure.Services.NotificationHandlerService;
using MediatR;

namespace HiClass.Infrastructure.Services.InvitationServices
{
    public class InvitationService : IInvitationService
    {
        private readonly IEmailHandlerService _emailHandlerService;
        private readonly IUserHelper _userHelper;
        private readonly INotificationHandlerService _notificationHandlerService;

        public InvitationService(IEmailHandlerService emailHandlerService, IUserHelper userHelper, INotificationHandlerService notificationHandlerService)
        {
            _emailHandlerService = emailHandlerService;
            _userHelper = userHelper;
            _notificationHandlerService = notificationHandlerService;
        }

        public async Task<Invitation> CreateInvitation(Guid userSenderId, IMediator mediator,
            CreateInvitationRequestDto requestInvitationDto)
        {
            var userReceiverId = await _userHelper.GetUserIdByClassId(requestInvitationDto.ClassReceiverId, mediator);

            if (!DateTime.TryParse(requestInvitationDto.DateOfInvitation, out var dateOfInvitation))
            {
                throw new DateTimeInvalidFormatProvidedException();
            }

            var command = new CreateInvitationCommand
            {
                UserSenderId = userSenderId,
                UserReceiverId = userReceiverId,
                ClassSenderId = requestInvitationDto.ClassSenderId,
                ClassReceiverId = requestInvitationDto.ClassReceiverId,
                DateOfInvitation = dateOfInvitation,
                Status = InvitationStatuses.Pending.ToString(),
                InvitationText = requestInvitationDto.InvitationText
            };

            var invitation = await mediator.Send(command);

            var userSender = await _userHelper.GetUserById(userSenderId, mediator);
            var userReceiver = await _userHelper.GetUserById(userReceiverId, mediator);

            await _emailHandlerService.SendAsync(userSender.Email, EmailConstants.EmailInvitationSubject,
                EmailConstants.EmailSenderInvitationMessage(userReceiver.Email, dateOfInvitation));

            await _emailHandlerService.SendAsync(userReceiver.Email, EmailConstants.EmailInvitationSubject,
                EmailConstants.EmailReceiverInvitationMessage(userSender.Email, dateOfInvitation));
            
            return invitation;
        }

        public async Task<Feedback> CreateFeedback(Guid userSenderId, IMediator mediator,
            CreateFeedbackRequestDto sendFeedbackRequestDto)
        {
            var command = new CreateFeedbackCommand
            {
                UserSenderId = userSenderId,
                UserRecipientId = sendFeedbackRequestDto.UserReceiverId,
                ClassSenderId = sendFeedbackRequestDto.ClassSenderId,
                ClassReceiverId = sendFeedbackRequestDto.ClassReceiverId,
                InvitationId = sendFeedbackRequestDto.InvitationId,
                Rating = sendFeedbackRequestDto.Rating,
                WasTheJointLesson = sendFeedbackRequestDto.WasTheJointLesson,
                FeedbackText = sendFeedbackRequestDto.FeedbackText,
                ReasonForNotConducting = sendFeedbackRequestDto.ReasonForNotConducting
            };

            var feedback = await mediator.Send(command);
            
            return feedback;
        }
    }
}