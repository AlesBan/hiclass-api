using AutoMapper;
using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserAccessToken;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Interfaces;
using HiClass.Application.Models.User;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.EditUserEmail;

public class EditUserEmailAndRemoveVerificationCommandHandler :
    IRequestHandler<EditUserEmailAndRemoveVerificationCommand, User>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMapper _mapper;
    private readonly ITokenHelper _tokenHelper;

    public EditUserEmailAndRemoveVerificationCommandHandler(ISharedLessonDbContext context, 
        ITokenHelper tokenHelper, IMapper mapper)
    {
        _context = context;
        _tokenHelper = tokenHelper;
        _mapper = mapper;
    }

    public async Task<User> Handle(EditUserEmailAndRemoveVerificationCommand request,
        CancellationToken cancellationToken)
    {
        var user = _context.Users
            .FirstOrDefault(u =>
                u.UserId == request.UserId);

        if (user == null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        user.Email = request.Email;
        user.IsVerified = false;
        
        var createAccessTokenUserDto = _mapper.Map<CreateAccessTokenUserDto>(user);
        var newToken = _tokenHelper.CreateToken(createAccessTokenUserDto);
        user.AccessToken = newToken;
        
        _context.Users.Update(user);

        await _context.SaveChangesAsync(cancellationToken);

        user = _context.Users
            .Include(u => u.City)
            .Include(u => u.Country)
            .Include(u => u.Institution)
            .Include(u => u.Classes)
            .ThenInclude(c => c.ClassLanguages)
            .ThenInclude(cl => cl.Language)
            .Include(u => u.Classes)
            .ThenInclude(c => c.ClassDisciplines)
            .ThenInclude(cd => cd.Discipline)
            .Include(u => u.Classes)
            .ThenInclude(c => c.Grade)
            .Include(u => u.UserDisciplines)
            .ThenInclude(ud => ud.Discipline)
            .Include(u => u.UserLanguages)
            .ThenInclude(ul => ul.Language)
            .Include(u => u.UserGrades)
            .ThenInclude(ug => ug.Grade)
            .FirstOrDefault(u =>
                u.UserId == request.UserId);

        return user;
    }
}