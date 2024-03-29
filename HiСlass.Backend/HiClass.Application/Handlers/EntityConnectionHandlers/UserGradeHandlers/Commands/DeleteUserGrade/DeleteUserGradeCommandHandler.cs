using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Interfaces;
using HiClass.Domain.EntityConnections;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityConnectionHandlers.UserGradeHandlers.Commands.DeleteUserGrade;

public class DeleteUserGradeCommandHandler : IRequestHandler<DeleteUserGradeCommand, Unit>
{
    private readonly ISharedLessonDbContext _context;

    public DeleteUserGradeCommandHandler(ISharedLessonDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteUserGradeCommand request, CancellationToken cancellationToken)
    {
        var userGrade = await _context.UserGrades.FirstOrDefaultAsync(ug =>
                ug.UserId == request.UserId &&
                ug.GradeId == request.GradeId,
            cancellationToken);

        if (userGrade == null)
        {
            throw new NotFoundException(nameof(UserGrade), request.UserId, request.GradeId);
        }

        _context.UserGrades.Remove(userGrade);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}