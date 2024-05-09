using System.ComponentModel.DataAnnotations;
using HiClass.Domain.Entities.Communication;

namespace HiClass.Application.Models.Invitations.Feedbacks.CreateFeedback;

public class CreateFeedbackResponseDto
{
    public CreateFeedbackResponseDto(Feedback feedback)
    {
        Feedback = feedback;
    }

    [Required] public Feedback Feedback { get; set; }
}