using AutoMapper;
using HiClass.Application.Common.Exceptions.User;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserDeviceHandlers.Commands.CreateUserDevice;
using HiClass.Application.Helpers;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces;
using HiClass.Application.Models.User.Authentication;
using HiClass.Application.Models.User.Registration;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
{
    private readonly ISharedLessonDbContext _context;
    private readonly IMapper _mapper;
    private readonly ITokenHelper _tokenHelper;
    private readonly IUserHelper _userHelper;
    private readonly IMediator _mediator;

    public RegisterUserCommandHandler(ISharedLessonDbContext context, IMapper mapper, ITokenHelper tokenHelper,
        IUserHelper userHelper, IMediator mediator)
    {
        _context = context;
        _mapper = mapper;
        _tokenHelper = tokenHelper;
        _userHelper = userHelper;
        _mediator = mediator;
    }

    public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommand request,
        CancellationToken cancellationToken)
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
            UserId = Guid.NewGuid(),
            Email = userEmail,
        };

        PasswordHelper.SetUserPasswordHash(newUser, userPassword);

        var verificationCode = _userHelper.GenerateVerificationCode();
        newUser.VerificationCode = verificationCode;

        var tokenUserDto = _mapper.Map<CreateTokenDto>(newUser);
        var accessToken = _tokenHelper.CreateAccessToken(tokenUserDto);

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync(cancellationToken);

        var refreshToken = await AddUserDevice(tokenUserDto, newUser.UserId, request.UserRegisterRequestDto.DeviceToken,
            cancellationToken);

        return new RegisterUserCommandResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UserId = newUser.UserId,
            VerificationCode = verificationCode
        };
    }

    private async Task<string> AddUserDevice(CreateTokenDto tokenUserDto, Guid userId, string deviceToken,
        CancellationToken cancellationToken)
    {
        var refreshToken = _tokenHelper.CreateRefreshToken(tokenUserDto);

        var command = new CreateUserDeviceCommand
        {
            DeviceToken = deviceToken,
            UserId = userId,
            RefreshToken = refreshToken,
        };

        await _mediator.Send(command, cancellationToken);

        return refreshToken;
    }
}