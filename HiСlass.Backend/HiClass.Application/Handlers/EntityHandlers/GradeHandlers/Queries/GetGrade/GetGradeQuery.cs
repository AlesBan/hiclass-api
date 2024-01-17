using HiClass.Domain.Entities.Education;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.GradeHandlers.Queries.GetGrade;

public class GetGradeQuery : IRequest<Grade>
{
    public int GradeNumber { get; set; }

    public GetGradeQuery(int gradeNumber)
    {
        GradeNumber = gradeNumber;
    }
}