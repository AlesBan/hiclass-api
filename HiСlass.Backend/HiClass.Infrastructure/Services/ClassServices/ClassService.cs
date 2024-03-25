using AutoMapper;
using HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.CreateClass;
using HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.DeleteClass;
using HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.EditClass;
using HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.EditClassImage;
using HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Commands.SetClassImage;
using HiClass.Application.Handlers.EntityHandlers.ClassHandlers.Queries.GetClass;
using HiClass.Application.Handlers.EntityHandlers.DisciplineHandlers.Queries.GetDisciplinesByTitles;
using HiClass.Application.Handlers.EntityHandlers.LanguageHandlers.Queries.GetLanguagesByTitles;
using HiClass.Application.Models.Class;
using HiClass.Application.Models.Class.EditClassDtos;
using HiClass.Application.Models.Images;
using HiClass.Application.Models.Images.Editing;
using HiClass.Application.Models.Images.Editing.Image;
using HiClass.Application.Models.Images.Setting;
using HiClass.Infrastructure.Services.ImageServices;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HiClass.Infrastructure.Services.ClassServices;

public class ClassService : IClassService
{
    private readonly IMapper _mapper;
    private readonly IImageHandlerService _imageHandlerService;
    private readonly IConfiguration _configuration;

    public ClassService(IMapper mapper, IImageHandlerService uploadImageService, IConfiguration configuration)
    {
        _mapper = mapper;
        _imageHandlerService = uploadImageService;
        _configuration = configuration;
    }

    public async Task<ClassProfileDto> CreateClass(Guid userId, CreateClassRequestDto requestClassDto,
        IMediator mediator)
    {
        var disciplines = await mediator.Send(new GetDisciplinesByTitlesQuery(requestClassDto.DisciplineTitles),
            CancellationToken.None);
        var languages = await mediator.Send(new GetLanguagesByTitlesQuery(requestClassDto.LanguageTitles),
            CancellationToken.None);

        //Id initialization needs to be here (in config nope)
        var classId = Guid.NewGuid();

        var command = new CreateClassCommand
        {
            UserId = userId,
            ClassId = classId,
            Title = requestClassDto.Title,
            GradeNumber = requestClassDto.GradeNumber,
            DisciplineIds = disciplines.Select(d => d.DisciplineId).ToList(),
            LanguageIds = languages.Select(l => l.LanguageId).ToList(),
        };

        var @class = await mediator.Send(command, CancellationToken.None);

        await Task.Delay(20);

        var classProfile = _mapper.Map<ClassProfileDto>(@class);

        await Task.Delay(20);

        classProfile.Languages = @class.ClassLanguages.Select(cl => cl.Language.Title).ToList();
        classProfile.Disciplines = @class.ClassDisciplines.Select(cd => cd.Discipline.Title).ToList();

        return classProfile;
    }

    public async Task<SetImageResponseDto> SetClassImage(Guid classId, SetImageRequestDto requestDto,
        IMediator mediator)
    {
        var awsS3UploadImageResponseDto = await _imageHandlerService.UploadImageAsync(requestDto.ImageFormFile,
            _configuration["AWS_CONFIGURATION:CLASS_IMAGES_FOLDER"], classId.ToString());
        var imageUrl = awsS3UploadImageResponseDto.ImageUrl;

        var command = new SetClassImageCommand()
        {
            ClassId = classId,
            ImageUrl = imageUrl
        };

        var result = await mediator.Send(command);

        return new SetImageResponseDto()
        {
            ImageUrl = result
        };
    }

    public async Task<ClassProfileDto> GetClassProfile(Guid classId, IMediator mediator)
    {
        var command = new GetClassByIdQuery(classId);

        var @class = await mediator.Send(command);

        await Task.Delay(20);

        var classProfile = _mapper.Map<ClassProfileDto>(@class);

        await Task.Delay(20);

        classProfile.Languages = @class.ClassLanguages.Select(cl => cl.Language.Title).ToList();
        classProfile.Disciplines = @class.ClassDisciplines.Select(cd => cd.Discipline.Title).ToList();

        return classProfile;
    }

    public async Task<ClassProfileDto> UpdateClass(Guid classId, EditClassRequestDto requestDto,
        IMediator mediator)
    {
        var command = new EditClassCommand
        {
            ClassId = classId,
            Title = requestDto.Title,
            GradeNumber = requestDto.GradeNumber,
            DisciplineTitles = requestDto.DisciplineTitles,
            LanguageTitles = requestDto.LanguageTitles,
        };

        var @class = await mediator.Send(command, CancellationToken.None);

        await Task.Delay(20);

        var classProfile = _mapper.Map<ClassProfileDto>(@class);

        await Task.Delay(20);

        classProfile.Languages = @class.ClassLanguages.Select(cl => cl.Language.Title).ToList();
        classProfile.Disciplines = @class.ClassDisciplines.Select(cd => cd.Discipline.Title).ToList();

        return classProfile;
    }

    public async Task<EditImageResponseDto> UpdateClassImage(Guid classId, EditImageRequestDto requestDto,
        IMediator mediator)
    {
        var file = requestDto.ImageFormFile;
        var awsS3UpdateImageResponseDto = await _imageHandlerService.UpdateImageAsync(file,
            _configuration["AWS_CONFIGURATION:CLASS_IMAGES_FOLDER"], classId.ToString());
        var imageUrl = awsS3UpdateImageResponseDto.ImageUrl;

        var command = new EditClassImageCommand()
        {
            ClassId = classId,
            ImageUrl = imageUrl
        };

        await mediator.Send(command);

        return new EditImageResponseDto()
        {
            ImageUrl = imageUrl
        };
    }

    public async Task DeleteClass(Guid classId, IMediator mediator)
    {
        var command = new DeleteClassCommand(classId);
        await mediator.Send(command, CancellationToken.None);
    }
}