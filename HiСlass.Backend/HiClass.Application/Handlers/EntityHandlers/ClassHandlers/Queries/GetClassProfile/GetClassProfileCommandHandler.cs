using AutoMapper;
using HiClass.Application.Dtos.ClassDtos;
using MediatR;

namespace HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Queries.GetClassProfile;

public class GetClassProfileCommandHandler: IRequestHandler<GetClassProfileCommand, ClassProfileDto>
{
    private readonly IMapper _mapper;
    public GetClassProfileCommandHandler(IMapper mapper)
    {
        _mapper = mapper;
    }
    public Task<ClassProfileDto> Handle(GetClassProfileCommand request, CancellationToken cancellationToken)
    {
        var classProfile = _mapper.Map<ClassProfileDto>(request.Class);
        return Task.FromResult(classProfile);
    }
}