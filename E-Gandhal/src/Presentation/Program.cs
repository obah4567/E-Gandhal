

using E_Gandhal.Infrastructure.Repositories;
using E_Gandhal.src.Application.IServices;
using E_Gandhal.src.Domain.Models.Authentification;
using E_Gandhal.src.Infrastructure.ApplicationDBContext;
using E_Gandhal.src.Infrastructure.Repositories;
using EGandhal.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add support to logging with SERILOG
builder.Host.UseSerilog((context, configuration) =>
    {
        configuration.WriteTo.Console();
        configuration.ReadFrom.Configuration(context.Configuration);
    });

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Of Injection
builder.Services.AddScoped<IUserAuthentificationService, UserRepository>();
builder.Services.AddScoped<IStudentService, StudentRepository>();
builder.Services.AddScoped<ITeacherService, TeacherRepository>();
builder.Services.AddScoped<IPasswordHasher<Register>, PasswordHasher<Register>>();
builder.Services.AddScoped<IClasseService, ClasseRepository>();
builder.Services.AddScoped<ISubjectService, SubjectRepository>();
builder.Services.AddScoped<INoteService, NoteRepository>();
builder.Services.AddScoped<ISchoolCreation, SchoolCreationRepository>();

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddIdentity<Register, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

// Enable Authentication & Authorization
app.UseAuthentication();  // JWT + Cookies
app.UseAuthorization();

app.MapControllers();

app.Run();
