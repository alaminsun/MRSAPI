using MRSAPI.Gateway;
using MRSAPI.Data;
using MRSAPI.Repository.IRepository;
using MRSAPI.Repository;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;
using MRSAPI;
using System.Reflection;
using MRSAPI.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<RouteOptions>(options =>
{
    options.ConstraintMap.Add("string", typeof(string));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<MRSDbContext>();
builder.Services.AddScoped<DBHelper>();
builder.Services.AddScoped<IDGenerated>();
//builder.Services.AddTransient<ISampleRepository,SampleRepository>();
builder.Services.AddTransient<IDoctorRepository, DoctorRepository>();
builder.Services.AddTransient<IInstitutionRepository, InstitutionRepository>();
builder.Services.AddTransient<IMenuRepository, MenuRepository>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen();
// Configure Swagger UI
builder.Services.AddSwaggerGen(options =>
{
    // Configure the Swagger document for each version
    foreach (var description in builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
    {
        options.SwaggerDoc(description.GroupName, new OpenApiInfo
        {
            Title = "MRS API",
            Version = description.ApiVersion.ToString(),
            // Add any other relevant information
        });
        var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
        options.IncludeXmlComments(cmlCommentsFullPath);
    }
});

//var appSettingsSection = builder.Configuration.GetSection("AppSettings");
//builder.Services.Configure<AppSettings>(appSettingsSection);

//var appSettings = appSettingsSection.Get<AppSettings>();
//var key = Encoding.ASCII.GetBytes(appSettings.Secret);

//builder.services.AddAuthentication(x =>
//{
//    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(x =>
//{
//    x.RequireHttpsMetadata = false;
//    x.SaveToken = true;
//    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//        ValidateIssuer = false,
//        ValidateAudience = false
//    };
//});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Configure Swagger UI for each version
        foreach (var description in app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    // Configure Swagger UI for each version
    foreach (var description in app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});
//app.Use(async (context, next) =>
//{
//    // Set a custom header for all responses
//    context.Response.Headers["CustomKey"] = "My custom value";

//    await next();
//});
app.UseHttpsRedirection();

app.UseRouting();
app.UseCors(x => x
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
//app.UseApiKeyMiddleware();

//app.UseMiddleware<CustomHeaderMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

//app.MapControllers();

app.Run();
