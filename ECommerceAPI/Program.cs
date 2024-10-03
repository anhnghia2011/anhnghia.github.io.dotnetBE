using NikeShoeStoreApi.Models;
using Microsoft.EntityFrameworkCore;
using NikeShoeStoreApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký DBContext với Dependency Injection, sử dụng chuỗi kết nối "DefaultConnection"
builder.Services.AddDbContext<DBContextNikeShoeStore>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Đăng ký các dịch vụ khác
builder.Services.AddControllers();
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

// Sử dụng CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
