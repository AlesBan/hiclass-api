using System.Reflection;
using Amazon.S3;
using HiClass.API.Configuration;
using HiClass.API.Configuration.Swagger;
using HiClass.API.Helpers.NotificationDtoCreatorHelper;
using HiClass.API.Middleware;
using HiClass.Application;
using HiClass.Application.Common.Mappings;
using HiClass.Application.Helpers.DataHelper;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces;
using HiClass.Infrastructure.IntegrationServices.Aws;
using HiClass.Infrastructure.IntegrationServices.Firebase.FirebaseConnector;
using HiClass.Infrastructure.IntegrationServices.Firebase.FireBaseNotificationSender;
using HiClass.Infrastructure.InternalServices.ClassServices;
using HiClass.Infrastructure.InternalServices.DeviceHandlerService;
using HiClass.Infrastructure.InternalServices.EditUserAccountService;
using HiClass.Infrastructure.InternalServices.EmailServices;
using HiClass.Infrastructure.InternalServices.EmailServices.EmailHandlerService;
using HiClass.Infrastructure.InternalServices.EmailServices.EmailTemplateService;
using HiClass.Infrastructure.InternalServices.ImageServices;
using HiClass.Infrastructure.InternalServices.InvitationServices;
using HiClass.Infrastructure.InternalServices.NotificationHandlerService;
using HiClass.Infrastructure.InternalServices.SearchServices;
using HiClass.Infrastructure.InternalServices.StaticDataServices;
using HiClass.Infrastructure.InternalServices.UserServices;
using HiClass.Persistence;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();
var configuration = builder.Configuration
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(conf =>
{
    conf.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    conf.AddProfile(new AssemblyMappingProfile(typeof(ISharedLessonDbContext).Assembly));
});

builder.Services.AddApplication();
builder.Services.AddPersistence(configuration);

builder.Services.ConfigureCors();

builder.Services.ConfigureAuthentication(configuration);
builder.Services.ConfigureFirebase(configuration);

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.ConfigureSwagger();

builder.Services.AddScoped<IDefaultSearchService, DefaultSearchService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IEditUserAccountService, EditUserAccountService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<ISearchService, SearchService>();

builder.Services.AddScoped<IEmailHandlerService, EmailHandlerService>();
builder.Services.AddSingleton<IEmailTemplateService, EmailTemplateService>();

builder.Services.AddScoped<IInvitationService, InvitationService>();
builder.Services.AddScoped<IStaticDataService, StaticDataService>();
builder.Services.AddScoped<INotificationHandlerService, NotificationHandlerService>();
builder.Services.AddScoped<IDeviceHandlerService, DeviceHandlerService>();

builder.Services.AddScoped<IImageHandlerService, ImageHandlerService>();
builder.Services.AddScoped<IAwsImagesService, AwsImagesService>();
builder.Services.AddSingleton<IAmazonS3, AmazonS3Client>();

builder.Services.AddScoped<ITokenHelper, TokenHelper>();
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IDataForUserHelper, DataForUserHelper>();
builder.Services.AddScoped<INotificationDtoCreatorHelper, NotificationDtoCreatorHelper>();


builder.Services.AddScoped<IFirebaseConnector, FirebaseConnector>();
builder.Services.AddScoped<IFireBaseNotificationSender, FireBaseNotificationSender>();

builder.Services.AddTransient<DatabaseConnectionMiddleware>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var servicesProvider = scope.ServiceProvider;
    try
    {
        var context = servicesProvider.GetRequiredService<SharedLessonDbContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = servicesProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.UseMiddleware<DatabaseConnectionMiddleware>();

app.ConfigureSwagger();

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AlloyAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();