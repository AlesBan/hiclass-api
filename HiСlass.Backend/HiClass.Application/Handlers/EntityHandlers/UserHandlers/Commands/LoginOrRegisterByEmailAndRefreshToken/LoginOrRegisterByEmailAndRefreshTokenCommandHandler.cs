using AutoMapper;
using HiClass.Application.Handlers.EntityConnectionHandlers.UserDeviceHandlers.Commands.CreateUserDevice;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Interfaces;
using HiClass.Application.Models.User.Authentication;
using HiClass.Domain.Entities.Main;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HiClass.Application.Handlers.EntityHandlers.UserHandlers.Commands.LoginOrRegisterByEmailAndRefreshToken
{
    public class LoginOrRegisterByEmailAndRefreshTokenCommandHandler : IRequestHandler<LoginOrRegisterByEmailAndRefreshTokenCommand, TokenModelResponseDto>
    {
        private readonly ISharedLessonDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenHelper _tokenHelper;
        private readonly IMediator _mediator;

        public LoginOrRegisterByEmailAndRefreshTokenCommandHandler(ISharedLessonDbContext context, IMapper mapper, ITokenHelper tokenHelper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _tokenHelper = tokenHelper;
            _mediator = mediator;
        }

        public async Task<TokenModelResponseDto> Handle(LoginOrRegisterByEmailAndRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var userEmail = request.Email;
            
            var user = await _context.Users
                .Include(u => u.UserDevices)
                .ThenInclude(d => d.Device)
                .FirstOrDefaultAsync(x => x.Email == userEmail, cancellationToken);

            if (user == null)
            {
                var newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    Email = userEmail,
                    IsVerified = true
                };

                var tokenUserDto = _mapper.Map<CreateTokenDto>(newUser);
                var accessToken = _tokenHelper.CreateAccessToken(tokenUserDto);

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync(cancellationToken);

                var refreshToken = await AddUserDevice(tokenUserDto, newUser.UserId, request.DeviceToken, cancellationToken);

                return new TokenModelResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }

            var existingUserTokenDto = _mapper.Map<CreateTokenDto>(user);
            var newAccessToken = _tokenHelper.CreateAccessToken(existingUserTokenDto);
            var newRefreshToken = _tokenHelper.CreateRefreshToken(existingUserTokenDto);

            var userDevice = user.UserDevices
                .FirstOrDefault(ud => ud.Device.DeviceToken == request.DeviceToken);

            if (userDevice != null)
            {
                userDevice.IsActive = true;
                userDevice.LastActive = DateTime.UtcNow;
                userDevice.RefreshToken = newRefreshToken;
                
                _context.UserDevices.Update(userDevice);
                await _context.SaveChangesAsync(CancellationToken.None);
            }
            else
            {
                var command = new CreateUserDeviceCommand
                {
                    DeviceToken = request.DeviceToken,
                    UserId = user.UserId,
                    RefreshToken = newRefreshToken,
                };
                await _mediator.Send(command, cancellationToken);
            }

            return new TokenModelResponseDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            };
        }

        private async Task<string> AddUserDevice(CreateTokenDto tokenUserDto, Guid userId, string deviceToken, CancellationToken cancellationToken)
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
}
