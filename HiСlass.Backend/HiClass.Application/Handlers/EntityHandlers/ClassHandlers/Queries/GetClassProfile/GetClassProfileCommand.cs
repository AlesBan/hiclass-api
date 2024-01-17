using HiClass.Application.Dtos.ClassDtos;
using HiClass.Domain.Entities.Main;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Queries.GetClassProfile;

public class GetClassProfileCommand : IRequest<ClassProfileDto>
{
    public Class Class { get; set; }
}