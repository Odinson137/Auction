using AuctionIdentity.Data;
using AuctionIdentity.Interfaces;
using AuctionIdentity.Repository;
using AuctionIdentity.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddTransient<Seed>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasherService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await new Seed(scope.ServiceProvider.GetRequiredService<DataContext>()).SeedDataContext();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
