using System.ComponentModel.DataAnnotations;

namespace HiClass.Application.Models.Invitations.Feedback.CreateFeedback;

public class CreateFeedbackResponseDto
{
    public CreateFeedbackResponseDto(Domain.Entities.Communication.Feedback feedback)
    {
        Feedback = feedback;
    }

    [Required] public Domain.Entities.Communication.Feedback Feedback { get; set; }
}