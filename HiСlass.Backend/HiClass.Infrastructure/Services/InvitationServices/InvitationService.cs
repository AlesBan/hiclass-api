using HiClass.Application.Constants;
using HiClass.Application.Handlers.EntityHandlers.FeedbackHandlers.Commands.CreateFeedback;
using HiClass.Application.Handlers.EntityHandlers.InvitationHandlers.Commands.CreateInvitation;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces.Services;
using HiClass.Application.Models.EmailManager;
using HiClass.Application.Models.Invitation;
using HiClass.Application.Models.Invitation.Feedback;
using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Enums;
using HiClass.Infrastructure.Services.EmailHandlerService;
using MediatR;

namespace HiClass.Infrastructure.Services.InvitationServices;

public class InvitationService : IInvitationService
{
    private readonly IEmailHandlerService _emailHandlerService;
    private readonly IUserHelper _userHelper;

    public InvitationService(IEmailHandlerService emailHandlerService, IUserHelper userHelper)
    {
        _emailHandlerService = emailHandlerService;
        _userHelper = userHelper;
    }

    public async Task<Invitation> CreateInvitation(Guid userSenderId, IMediator mediator,
        CreateInvitationRequestDto requestInvitationDto)
    {
        var userReceiverId = await _userHelper.GetUserIdByClassId(requestInvitationDto.ClassReceiverId, mediator);
        var dateOfInvitation = DateTime.Parse(requestInvitationDto.DateOfInvitation);

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

        await SendInvitationEmail(userSenderId, userReceiverId, dateOfInvitation, mediator);
        
        return invitation;
    }

    public async Task SendFeedback(Guid userSenderId, IMediator mediator,
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
        
        await mediator.Send(command);
    }

    private async Task SendInvitationEmail(Guid userSenderId, Guid userReceiverId,
        DateTime dateOfInvitation,
        IMediator mediator)
    {
        var userSender = await _userHelper.GetUserById(userSenderId, mediator);
        var userReceiver = await _userHelper.GetUserById(userReceiverId, mediator);

        await _emailHandlerService.SendAsync(userSender.Email, EmailConstants.EmailInvitationSubject,
            EmailConstants.EmailSenderInvitationMessage(userReceiver.Email, dateOfInvitation));
        await _emailHandlerService.SendAsync(userReceiver.Email, EmailConstants.EmailInvitationSubject,
            EmailConstants.EmailReceiverInvitationMessage(userSender.Email, dateOfInvitation));
    }
}