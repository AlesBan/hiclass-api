using HiClass.Application.Common.Exceptions.Invitations;
using HiClass.Application.Handlers.EntityHandlers.FeedbackHandlers.Commands.CreateFeedback;
using HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.ChangeInvitationStatus;
using HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.CreateInvitation;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Models.Invitations.CreateInvitation;
using HiClass.Application.Models.Invitations.Feedbacks.CreateFeedback;
using HiClass.Application.Models.Invitations.UpdateInvitationStatus;
using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Enums;
using HiClass.Infrastructure.InternalServices.EmailServices;
using HiClass.Infrastructure.InternalServices.EmailServices.EmailHandlerService;
using MediatR;

namespace HiClass.Infrastructure.InternalServices.InvitationServices
{
    public class InvitationService : IInvitationService
    {
        private readonly IEmailHandlerService _emailHandlerService;
        private readonly IUserHelper _userHelper;

        public InvitationService(IEmailHandlerService emailHandlerService, IUserHelper userHelper)
        {
            _emailHandlerService = emailHandlerService;
            _userHelper = userHelper;
        }

        public async Task<Invitation> CreateClassInvitationAndSendEmailInvitation(
            Guid userSenderId, 
            IMediator mediator,
            CreateClassInvitationRequestDto requestInvitationDto)
        {
            if (!DateTime.TryParse(requestInvitationDto.DateOfInvitation, out var dateOfInvitation))
            {
                throw new DateTimeInvalidFormatProvidedException(userSenderId);
            }
            
            var userReceiverId = await _userHelper.GetUserIdByClassId(
                requestInvitationDto.ClassRecipientId, 
                mediator);

            var command = new CreateInvitationCommand
            {
                Type = InvitationType.ClassInvitation,
                UserSenderId = userSenderId,
                UserReceiverId = userReceiverId,
                ClassSenderId = requestInvitationDto.ClassSenderId,
                ClassReceiverId = requestInvitationDto.ClassRecipientId,
                DateOfInvitation = dateOfInvitation,
                Status = InvitationStatus.Pending,
                InvitationText = requestInvitationDto.InvitationText
            };

            var invitation = await mediator.Send(command);
            var userSender = invitation.UserSender; 
            var userReceiver = invitation.UserRecipient;

            // Отправка HTML-писем с использованием шаблонов
            await _emailHandlerService.SendClassInvitationEmail(
                userSender.Email,
                userReceiver.Email,
                dateOfInvitation,
                requestInvitationDto.InvitationText);

            return invitation;
        }

        public async Task<Invitation> CreateExpertInvitationAndSendEmailInvitation(
            Guid userSenderId, 
            IMediator mediator,
            CreateExpertInvitationRequestDto requestInvitationDto)
        {
            if (!DateTime.TryParse(requestInvitationDto.DateOfInvitation, out var dateOfInvitation))
            {
                throw new DateTimeInvalidFormatProvidedException(userSenderId);
            }

            var command = new CreateInvitationCommand
            {
                Type = InvitationType.ExpertInvitation,
                UserSenderId = userSenderId,
                ClassSenderId = requestInvitationDto.ClassSenderId,
                UserReceiverId = requestInvitationDto.UserRecipientId,
                DateOfInvitation = dateOfInvitation,
                Status = InvitationStatus.Pending,
                InvitationText = requestInvitationDto.InvitationText
            };

            var invitation = await mediator.Send(command);
            var userSender = invitation.UserSender;
            var userReceiver = invitation.UserRecipient;

            // Отправка HTML-писем с использованием шаблонов
            await _emailHandlerService.SendExpertInvitationEmail(
                userSender.Email,
                userReceiver.Email,
                dateOfInvitation,
                requestInvitationDto.InvitationText);

            return invitation;
        }

        public async Task UpdateInvitationStatus(
            Guid userReceiverId, 
            IMediator mediator,
            UpdateInvitationStatusRequestDto requestDto)
        {
            var command = new UpdateInvitationStatusCommand
            {
                InvitationId = requestDto.InvitationId,
                UserId = userReceiverId,
                Status = requestDto.Status
            };

            await mediator.Send(command);
        }

        public async Task<Feedback> CreateFeedback(
            Guid userSenderId, 
            IMediator mediator,
            CreateFeedbackRequestDto sendFeedbackRequestDto)
        {
            var command = new CreateFeedbackCommand
            {
                UserFeedbackSenderId = userSenderId,
                InvitationId = sendFeedbackRequestDto.InvitationId,
                Rating = sendFeedbackRequestDto.Rating,
                WasTheJointLesson = sendFeedbackRequestDto.WasTheJointLesson,
                FeedbackText = sendFeedbackRequestDto.FeedbackText,
                ReasonForNotConducting = sendFeedbackRequestDto.ReasonForNotConducting
            };

            return await mediator.Send(command);
        }
    }
}