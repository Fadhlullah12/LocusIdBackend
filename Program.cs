using System.Text;
using LocusIDBackend.Context;
using LocusIDBackend.Dtos;
using LocusIDBackend.Repositories.Implementations;
using LocusIDBackend.Repositories.Interfaces;
using LocusIDBackend.Services.Implementations;
using LocusIDBackend.Services.Interfaces;
using LocusIDBackend.Services.SignalRHub;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Add this for better discovery
builder.Services.AddOpenApi(); // This uses the stable .NET 10 generator

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ConnectionString"), 
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ConnectionString"))));
// SignalR
builder.Services.AddSignalR();
// Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ILecturerRepository, LecturerRepository>();
builder.Services.AddScoped<IDirectorRepository, DirectorRepository>();
builder.Services.AddScoped<IFacultyRepository, FacultyRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IAcademicSessionRepository, AcademicSessionRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();
builder.Services.AddScoped<ILecturerCourseRepository, LecturerCourseRepository>();
builder.Services.AddScoped<IDepartmentCourseRepository, DepartmentCourseRepository>();
builder.Services.AddScoped<IStudentSessionRepository, StudentSessionRepository>();

// Registering Services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ILecturerService, LecturerService>();
builder.Services.AddScoped<IFacultyService, FacultyService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IDecodeTokenService, DecodeTokenService>();
builder.Services.AddScoped<IGeoService, GeoService>();
builder.Services.AddScoped<IAttendanceService, AttendanceService>();
builder.Services.AddScoped<IAcademicSessionService, AcademicSessionService>();
// Session Checker Background Worker
builder.Services.AddHostedService<SessionCleanupWorker>();
// Message repositories & services
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageService, MessageService>();
// Registering JWT Settings
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("Jwt").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

// Configuring JWT Authentication
var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://127.0.0.1:5500") // Your frontend URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
// SignalR
builder.Services.AddSignalR();
var app = builder.Build();

// Map SignalR hubs
app.MapHub<NotificationHub>("/hubs/notifications");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Generates the JSON at /openapi/v1.json using native .NET 10 logic
    app.MapOpenApi(); 

    // Use SwaggerUI only as a viewer for the native JSON
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowFrontend");
app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
