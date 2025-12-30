using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace OrderService;

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
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Service", Version = "v1" });
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

        // Configure HttpClients for other services
        var productBase = _config.GetValue<string>("ServiceUrls:ProductService") ?? "http://localhost:5002";
        var paymentBase = _config.GetValue<string>("ServiceUrls:PaymentService") ?? "http://localhost:5003";
        var userBase = _config.GetValue<string>("ServiceUrls:UserService") ?? "http://localhost:5005";
        services.AddHttpClient("product", c => c.BaseAddress = new Uri(productBase));
        services.AddHttpClient("payment", c => c.BaseAddress = new Uri(paymentBase));
        services.AddHttpClient("user", c => c.BaseAddress = new Uri(userBase));

        // Repository & business scaffolding
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderServiceImpl>();
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

// Order repository & service scaffolding
public interface IOrderRepository { Task AddAsync(Order o); }
public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db; public OrderRepository(AppDbContext db) => _db = db;
    public async Task AddAsync(Order o) { _db.Orders.Add(o); await _db.SaveChangesAsync(); }
}

public interface IOrderService { Task PlaceOrderAsync(Order o); }
public class OrderServiceImpl : IOrderService
{
    private readonly IOrderRepository _repo; public OrderServiceImpl(IOrderRepository repo) => _repo = repo;
    public Task PlaceOrderAsync(Order o) => _repo.AddAsync(o);
}
