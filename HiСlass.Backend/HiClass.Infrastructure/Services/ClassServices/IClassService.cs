using HiClass.Application.Models.Class;
using HiClass.Application.Models.Class.EditClassDtos;
using HiClass.Application.Models.Images;
using HiClass.Application.Models.Images.Editing;
using HiClass.Application.Models.Images.Editing.Image;
using HiClass.Application.Models.Images.Setting;
using MediatR;

namespace HiClass.Infrastructure.Services.ClassServices;

public interface IClassService
{
    public Task<ClassProfileDto> CreateClass(Guid userId, CreateClassRequestDto requestClassDto, IMediator mediator);
    public Task<SetImageResponseDto> SetClassImage(Guid classId, SetImageRequestDto requestDto,
        IMediator mediator);
    public Task<ClassProfileDto> GetClassProfile(Guid classId, IMediator mediator);
    public Task<ClassProfileDto> EditClass(Guid classId, EditClassRequestDto requestClassDto, IMediator mediator);
    public Task<EditImageResponseDto> EditClassImage(Guid classId, EditImageRequestDto requestDto, IMediator mediator);
    public Task DeleteClass(Guid classId, IMediator mediator);
}