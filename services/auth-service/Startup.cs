using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AuthService;

public class Startup
{
    private readonly IConfiguration _config;
    public Startup(IConfiguration config) => _config = config;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost3000", builder =>
            {
                builder.WithOrigins("http://localhost:3000")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
        });
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth Service", Version = "v1" }));

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(_config.GetConnectionString("DefaultConnection")));

        // Configure HttpClient for User Service
        var userServiceBase = _config.GetValue<string>("ServiceUrls:UserService") ?? "http://localhost:5005";
        services.AddHttpClient("user", c => c.BaseAddress = new Uri(userServiceBase));

        // JWT configuration from appsettings
        var jwtKey = _config.GetValue<string>("Jwt:Key") ?? "ThisIsADevelopmentKeyReplaceMe";
        var jwtIssuer = _config.GetValue<string>("Jwt:Issuer") ?? "auth-service";
        var jwtAudience = _config.GetValue<string>("Jwt:Audience") ?? "auth-clients";

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "Bearer";
            options.DefaultChallengeScheme = "Bearer";
        })
        .AddJwtBearer("Bearer", options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
            };
        });

        services.AddSingleton<IJwtService>(new JwtService(jwtKey, jwtIssuer, jwtAudience));

        // Repository & business scaffolding
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, AuthServiceImpl>();
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseCors("AllowLocalhost3000");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
}

// Simple repository + service scaffolding
public interface IUserRepository
{
    Task AddAsync(User u);
    Task<User?> FindByEmailAsync(string email);
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;
    public async Task AddAsync(User u) { _db.Users.Add(u); await _db.SaveChangesAsync(); }
    public async Task<User?> FindByEmailAsync(string email) => await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
}

public interface IAuthService { Task<User> RegisterAsync(string email, string password); }

public class AuthServiceImpl : IAuthService
{
    private readonly IUserRepository _repo;
    public AuthServiceImpl(IUserRepository repo) => _repo = repo;
    public async Task<User> RegisterAsync(string email, string password)
    {
        var u = new User { Email = email, PasswordHash = password }; // keep simple for scaffold
        await _repo.AddAsync(u);
        return u;
    }
}
