using HiClass.Application.Dtos.ClassDtos;
using HiClass.Application.Models.Class;
using MediatR;

namespace HiClass.Application.Interfaces.Services;

public interface IClassService
{
    public Task<ClassProfileDto> CreateClass(Guid userId, CreateClassRequestDto requestClassDto, IMediator mediator);
    public Task<ClassProfileDto> GetClassProfile(Guid classId, IMediator mediator);
    public Task<ClassProfileDto> UpdateClass(Guid classId, UpdateClassRequestDto requestClassDto, IMediator mediator);
    public Task DeleteClass(Guid classId, IMediator mediator);
}