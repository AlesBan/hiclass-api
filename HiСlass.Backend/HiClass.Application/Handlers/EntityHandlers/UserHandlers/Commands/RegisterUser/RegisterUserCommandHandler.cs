using AutoMapper;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Helpers;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces;
using HiClass.Application.Models.User;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, User>
{
    private readonly ISharedLessonDbContext _context;
    private readonly ITokenHelper _tokenHelper;
    private readonly IMapper _mapper;
    private readonly IUserHelper _userHelper;

    public RegisterUserCommandHandler(ISharedLessonDbContext context, ITokenHelper tokenHelper, IMapper mapper, IUserHelper userHelper)
    {
        _context = context;
        _tokenHelper = tokenHelper;
        _mapper = mapper;
        _userHelper = userHelper;
    }

    public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userEmail = request.UserRegisterRequestDto.Email;
        var userPassword = request.UserRegisterRequestDto.Password;

        var userExists = await _context.Users.AsNoTracking()
            .AnyAsync(x => x.Email == userEmail, cancellationToken);

        if (userExists)
        {
            throw new UserAlreadyExistsException(userEmail);
        }

        var newUser = new User
        {
            Email = userEmail
        };

        PasswordHelper.SetUserPasswordHash(newUser, userPassword);
        
        var verificationCode = _userHelper.GenerateVerificationCode();
        newUser.VerificationCode = verificationCode;
        
        var accessTokenUserDto = _mapper.Map<CreateAccessTokenUserDto>(newUser);
        var accessToken = _tokenHelper.CreateToken(accessTokenUserDto);

        newUser.AccessToken = accessToken;

        await _context.SaveChangesAsync(CancellationToken.None);

        await AddUserToDataBase(newUser, cancellationToken);

        return await Task.FromResult(newUser);
    }

    private async Task AddUserToDataBase(User user, CancellationToken cancellationToken)
    {
        await _context.Users
            .AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}