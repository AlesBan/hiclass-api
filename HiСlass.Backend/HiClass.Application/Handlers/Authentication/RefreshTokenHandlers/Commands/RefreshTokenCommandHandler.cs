using AutoMapper;
using HiClass.Application.Common.Exceptions.Authentication;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Models.User.Authentication;
using HiClass.Domain.EntityConnections;
using MediatR;

namespace HiClass.Application.Handlers.Authentication.RefreshTokenHandlers.Commands;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, TokenModelResponseDto>
{
    private readonly IUserHelper _userHelper;
    private readonly ITokenHelper _tokenHelper;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public RefreshTokenCommandHandler(IUserHelper userHelper, ITokenHelper tokenHelper, IMapper mapper,
        IMediator mediator)
    {
        _userHelper = userHelper;
        _tokenHelper = tokenHelper;
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<TokenModelResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var (userId, refreshToken, deviceToken) = (request.UserId, request.RefreshToken, request.DeviceToken);

        var user = await _userHelper.GetBlankUserWithDevicesById(userId, _mediator);

        UserDevice userDevice;

        userDevice = (!string.IsNullOrEmpty(deviceToken)
            ? user.UserDevices.FirstOrDefault(x => x.Device?.DeviceToken == deviceToken)
            : user.UserDevices.FirstOrDefault(x => x.RefreshToken == refreshToken))!;

        if (userDevice == null || userDevice.RefreshToken != refreshToken)
        {
            throw new InvalidTokenProvidedException(refreshToken);
        }

        if (!userDevice.IsActive)
        {
            throw new InvalidTokenProvidedException(refreshToken);
        }

        var createTokenDto = _mapper.Map<CreateTokenDto>(user);
        var newAccessToken = _tokenHelper.CreateAccessToken(createTokenDto);
        var newRefreshToken = _tokenHelper.CreateRefreshToken(createTokenDto);

        userDevice.RefreshToken = newRefreshToken;
        await _userHelper.UpdateAsync(user);

        return new TokenModelResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}