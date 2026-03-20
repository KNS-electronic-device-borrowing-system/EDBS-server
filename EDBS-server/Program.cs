using EDBS_server.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddScoped<EDBS_server.Repositories.IUserRepository, EDBS_server.Repositories.UserRepository>();
builder.Services.AddScoped<EDBS_server.Services.IAuthService, EDBS_server.Services.AuthService>();
builder.Services.Configure<EDBS_server.Settings.EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<EDBS_server.Services.IEmailService, EDBS_server.Services.EmailService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();


try
{
    await DataSeeder.SeedDataAsync(app.Services);
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Có lỗi xảy ra trong quá trình Seed dữ liệu.");
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();