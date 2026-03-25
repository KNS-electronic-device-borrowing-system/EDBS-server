using EDBS_server.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- DbContext ---
builder.Services.AddDbContext<AssetManagementDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseSnakeCaseNamingConvention());

// --- Services ---
builder.Services.AddControllers();
builder.Services.AddScoped<EDBS_server.Repositories.IUserRepository, EDBS_server.Repositories.UserRepository>();
builder.Services.AddScoped<EDBS_server.Services.IAuthService, EDBS_server.Services.AuthService>();
builder.Services.Configure<EDBS_server.Settings.EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EDBS_server.Services.IEmailService, EDBS_server.Services.EmailService>();

builder.Services.AddOpenApi();

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173") // FE của bạn
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// --- Seed dữ liệu ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AssetManagementDbContext>();
        await DataSeeder.SeedDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Có lỗi xảy ra trong quá trình Seed dữ liệu.");
    }
}

// --- OpenAPI ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();

// --- Enable CORS ---
app.UseCors();

app.UseAuthorization();
app.MapControllers();

app.Run();