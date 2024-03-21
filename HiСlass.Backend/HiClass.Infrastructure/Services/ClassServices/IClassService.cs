using HiClass.Application.Models.Class;
using HiClass.Application.Models.Class.SetImageDtos;
using HiClass.Application.Models.Class.UpdateClassDtos;
using HiClass.Application.Models.Class.UpdateClassDtos.UpdateImageDtos;
using HiClass.Application.Models.Images;
using MediatR;

namespace HiClass.Infrastructure.Services.ClassServices;

public interface IClassService
{
    public Task<ClassProfileDto> CreateClass(Guid userId, CreateClassRequestDto requestClassDto, IMediator mediator);

    public Task<SetImageResponseDto> SetClassImage(Guid classId, SetImageRequestDto requestDto,
        IMediator mediator);

    public Task<ClassProfileDto> GetClassProfile(Guid classId, IMediator mediator);
    public Task<ClassProfileDto> UpdateClass(Guid classId, UpdateClassRequestDto requestClassDto, IMediator mediator);
    public Task<UpdateClassImageResponseDto> UpdateClassImage(Guid classId, UpdateClassImageRequestDto requestDto, IMediator mediator);
    public Task DeleteClass(Guid classId, IMediator mediator);
}