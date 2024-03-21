using AutoMapper;
using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Interfaces;
using HiClass.Application.Models.User;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.UpdateUserVerification;

public class UpdateUserVerificationCommandHandler : IRequestHandler<UpdateUserVerificationCommand, string>
{
    private readonly ISharedLessonDbContext _context;
    private readonly ITokenHelper _tokenHelper;
    private readonly IMapper _mapper;

    public UpdateUserVerificationCommandHandler(ISharedLessonDbContext sharedLessonDbContext, ITokenHelper tokenHelper,
        IMapper mapper)
    {
        _context = sharedLessonDbContext;
        _tokenHelper = tokenHelper;
        _mapper = mapper;
    }

    public async Task<string> Handle(UpdateUserVerificationCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x =>
                x.UserId == request.UserId, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(request.UserId);
        }

        if (user.VerificationCode != request.VerificationCode)
        {
            throw new InvalidVerificationCodeProvidedException();
        }

        var tokenUserDto = _mapper.Map<CreateAccessTokenUserDto>(user);
        var newToken = _tokenHelper.CreateToken(tokenUserDto);

        user.IsVerified = true;
        user.VerifiedAt = DateTime.UtcNow;
        user.AccessToken = newToken;

        _context.Users.Attach(user).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);

        return user.AccessToken;
    }
}