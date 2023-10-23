using BlogsiteAppAccountAccess.Context;
using BlogsiteDomain.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(c =>
{
    c.AddDebug();
    c.AddConsole();
    c.AddAzureWebAppDiagnostics();
});

builder.Services.Configure<AzureFileLoggerOptions>(options =>
{
    options.FileName = "azure-diagnostics-";
    options.FileSizeLimit = 50 * 1024;
    options.RetainedFileCountLimit = 5;
});

builder.Services.Configure<AzureBlobLoggerOptions>(options =>
{
    options.BlobName = "log.txt";
});

string appAccountConnectionString = Environment.GetEnvironmentVariable("app_account_connectionString")
    ?? builder.Configuration["app-account:connectionString"]
    ?? throw new NullReferenceException(nameof(appAccountConnectionString));

builder.Services.AddDbContext<AccountDbContext>(
        options =>
        {
            options.UseSqlServer(connectionString: appAccountConnectionString);
        }
    );

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AccountDbContext>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("BlogAPIv1", new OpenApiInfo
    {
        Title = "BlogAPI",
        Description = "A set of endpoints to be used for my blog site.",
        Version = "v1"
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/BlogAPIv1/swagger.json", "BlogsiteAPIv1");
    c.DefaultModelRendering(ModelRendering.Example);
    c.DisplayRequestDuration();
    c.DocExpansion(DocExpansion.List);
    c.EnableFilter();
    c.EnableValidator();
    c.ShowCommonExtensions();
    c.ShowExtensions();
});

app.UseHttpsRedirection();
app.UseHttpLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();