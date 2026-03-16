using Fracto.Api.Data;
using Fracto.Api.Dtos.Auth;
using Fracto.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// CORS – allow Angular dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("FractoFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:4900")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Required for SignalR
    });
});

builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NikhilConnection")));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;



builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            ClockSkew = TimeSpan.FromSeconds(30),
            NameClaimType = System.Security.Claims.ClaimTypes.Name,
            RoleClaimType = System.Security.Claims.ClaimTypes.Role
        };

        // SignalR: Get token from query string (required for browser WebSockets)
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs/notifications"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fracto API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your token}"
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    };

    c.AddSecurityRequirement(securityRequirement);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();
    var passwordHasher = services.GetRequiredService<IPasswordHasher<User>>();

    // Ensure DB exists / migrations applied
    db.Database.EnsureCreated();

    // Seed default admin user if none exists
    var anyAdmin = db.Users.Any(u => u.Role == UserRole.Admin);
    if (!anyAdmin)
    {
        var admin = new User
        {
            Username = "nikhil11012",
            Email = "nikhil11012@gmail.com",
            FullName = "Nikhil Kumar Sahu",
            Role = UserRole.Admin
        };

        admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin@123");

        db.Users.Add(admin);
        db.SaveChanges();
    }

    // Seed specializations and doctors for testing if none exist
    if (!db.Specializations.Any())
    {
        var cardio = new Specialization { Name = "Cardiologist", Description = "Heart specialist" };
        var derm = new Specialization { Name = "Dermatologist", Description = "Skin specialist" };
        var neuro = new Specialization { Name = "Neurologist", Description = "Brain and nervous system specialist" };
        var ortho = new Specialization { Name = "Orthopedic", Description = "Bone and joint specialist" };

        db.Specializations.AddRange(cardio, derm, neuro, ortho);
        db.SaveChanges();

        if (!db.Doctors.Any())
        {
            var doctors = new List<Doctor>
            {
                new()
                {
                    FullName = "Dr. A. Sharma",
                    City = "Delhi",
                    SpecializationId = cardio.Id,
                    AverageRating = 4.5m,
                    IsActive = true
                },
                new()
                {
                    FullName = "Dr. B. Verma",
                    City = "Mumbai",
                    SpecializationId = cardio.Id,
                    AverageRating = 4.0m,
                    IsActive = true
                },
                new()
                {
                    FullName = "Dr. C. Gupta",
                    City = "Delhi",
                    SpecializationId = derm.Id,
                    AverageRating = 4.2m,
                    IsActive = true
                },
                new()
                {
                    FullName = "Dr. D. Iyer",
                    City = "Bengaluru",
                    SpecializationId = neuro.Id,
                    AverageRating = 4.7m,
                    IsActive = true
                },
                new()
                {
                    FullName = "Dr. E. Khan",
                    City = "Kolkata",
                    SpecializationId = ortho.Id,
                    AverageRating = 4.1m,
                    IsActive = true
                }
            };

            db.Doctors.AddRange(doctors);
            db.SaveChanges();
        }
    }
}

app.UseStaticFiles();

app.UseCors("FractoFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<Fracto.Api.Hubs.NotificationHub>("/hubs/notifications");

app.Run();
