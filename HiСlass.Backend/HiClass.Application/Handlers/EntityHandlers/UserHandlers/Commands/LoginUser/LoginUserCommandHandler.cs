using AutoMapper;
using HiClass.Application.Common.Exceptions.Database;
using HiClass.Application.Helpers;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces;
using HiClass.Application.Models.User;
using HiClass.Application.Models.User.Login;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginResponseDto>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMapper _mapper;
    private readonly ITokenHelper _tokenHelper;
    private readonly IUserHelper _userHelper;

    public LoginUserCommandHandler(ISharedLessonDbContext context, IMapper mapper, ITokenHelper tokenHelper,
        IUserHelper userHelper)
    {
        _context = context;
        _mapper = mapper;
        _tokenHelper = tokenHelper;
        _userHelper = userHelper;
    }

    public async Task<LoginResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x =>
                x.Email == request.UserLoginRequestDto.Email, cancellationToken);

        if (user == null)
        {
            throw new UserNotFoundException(request.UserLoginRequestDto.Email);
        }

        _userHelper.CheckUserVerification(user);
        PasswordHelper.VerifyPasswordHash(user, request.UserLoginRequestDto.Password);

        var tokenUserDto = _mapper.Map<CreateAccessTokenUserDto>(user);
        var newToken = _tokenHelper.CreateToken(tokenUserDto);

        user.AccessToken = newToken;

        _context.Users.Attach(user).State = EntityState.Modified;
        await _context.SaveChangesAsync(CancellationToken.None);

        var loginResponseDto = new LoginResponseDto
        {
            AccessToken = user.AccessToken,
        };

        return loginResponseDto;
    }
}