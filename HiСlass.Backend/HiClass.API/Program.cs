using System.Net;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using Amazon.S3;
using HiClass.API.Configuration;
using HiClass.API.Configuration.Swagger;
using HiClass.API.Helpers.WebSocketNotificationsHelper;
using HiClass.API.Swagger;
using HiClass.Application;
using HiClass.Application.Common.Mappings;
using HiClass.Application.Helpers.DataHelper;
using HiClass.Application.Helpers.TokenHelper;
using HiClass.Application.Helpers.UserHelper;
using HiClass.Application.Interfaces;
using HiClass.Application.Interfaces.Services;
using HiClass.Infrastructure.Services.AccountServices;
using HiClass.Infrastructure.Services.ClassServices;
using HiClass.Infrastructure.Services.DataBaseDataService;
using HiClass.Infrastructure.Services.DefaultDataServices;
using HiClass.Infrastructure.Services.EditUserAccountService;
using HiClass.Infrastructure.Services.EmailHandlerService;
using HiClass.Infrastructure.Services.ImageServices;
using HiClass.Infrastructure.Services.ImageServices.Aws;
using HiClass.Infrastructure.Services.InvitationServices;
using HiClass.Infrastructure.Services.NotificationHandlerService;
using HiClass.Infrastructure.Services.SearchService;
using HiClass.Infrastructure.Services.StaticDataServices;
using HiClass.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.ConfigureSwagger();

builder.Services.AddScoped<ISharedLessonDbContext, SharedLessonDbContext>();
builder.Services.AddScoped<IDefaultSearchService, DefaultSearchService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
builder.Services.AddScoped<IEditUserAccountService, EditUserAccountService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IEmailHandlerService, EmailHandlerService>();
builder.Services.AddScoped<IInvitationService, InvitationService>();
builder.Services.AddScoped<IDataBaseDataService, DataBaseDataService>();
builder.Services.AddScoped<IStaticDataService, StaticDataService>();

builder.Services.AddScoped<IImageHandlerService, ImageHandlerService>();
builder.Services.AddScoped<IAwsImagesService, AwsImagesService>();

builder.Services.AddScoped<INotificationHandlerService, NotificationHandlerService>();

builder.Services.AddScoped<ITokenHelper, TokenHelper>();
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IUserDataHelper, UserDataHelper>();


builder.Services.AddSingleton<IAmazonS3, AmazonS3Client>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var servicesProvider = scope.ServiceProvider;
    try
    {
        var context = servicesProvider.GetRequiredService<SharedLessonDbContext>();
        context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = servicesProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AlloyAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();