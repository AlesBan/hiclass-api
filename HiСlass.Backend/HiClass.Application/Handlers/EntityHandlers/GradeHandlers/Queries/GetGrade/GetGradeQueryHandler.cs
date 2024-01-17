using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.Entities.Education;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.GradeHandlers.Queries.GetGrade;

public class GetGradeQueryHandler : IRequestHandler<GetGradeQuery, Grade>
{
    private readonly ISharedLessonDbContext _context;

    public GetGradeQueryHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public Task<Grade> Handle(GetGradeQuery request, CancellationToken cancellationToken)
    {
        var grade = _context.Grades.FirstOrDefault(g => g.GradeNumber == request.GradeNumber);

        if (grade == null)
        {
            throw new NotFoundException(nameof(Grade), request.GradeNumber);
        }

        return Task.FromResult(grade);
    }
}