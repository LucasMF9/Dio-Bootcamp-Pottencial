using Microsoft.EntityFrameworkCore;
using tech_test_payment_api.Bank;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .Services
    .AddDbContext<BankDb>(options => options.UseInMemoryDatabase("BankVendas"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
