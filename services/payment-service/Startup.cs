using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace PaymentService;

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
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment Service", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            });
        });

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(_config.GetConnectionString("DefaultConnection")));

        // Configure HttpClient for User Service
        var userServiceBase = _config.GetValue<string>("ServiceUrls:UserService") ?? "http://localhost:5005";
        services.AddHttpClient("user", c => c.BaseAddress = new Uri(userServiceBase));

        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IPaymentService, PaymentServiceImpl>();
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
        app.UseAuthorization();
        app.MapControllers();
    }
}

public interface IWalletRepository { Task<Wallet?> GetByUserAsync(Guid userId); Task AddAsync(Wallet w); Task SaveChangesAsync(); }
public class WalletRepository : IWalletRepository
{
    private readonly AppDbContext _db; public WalletRepository(AppDbContext db) => _db = db;
    public Task<Wallet?> GetByUserAsync(Guid userId) => _db.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
    public Task AddAsync(Wallet w) { _db.Wallets.Add(w); return _db.SaveChangesAsync(); }
    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}

public interface IPaymentService { Task<int> GetBalanceAsync(Guid userId); }
public class PaymentServiceImpl : IPaymentService
{
    private readonly IWalletRepository _repo; public PaymentServiceImpl(IWalletRepository repo) => _repo = repo;
    public async Task<int> GetBalanceAsync(Guid userId) { var w = await _repo.GetByUserAsync(userId); return w?.Balance ?? 0; }
}
