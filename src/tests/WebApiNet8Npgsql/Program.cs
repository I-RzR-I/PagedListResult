
using Microsoft.EntityFrameworkCore;
using WebApiNet8Npgsql.Data;

namespace WebApiNet8Npgsql
{
    public class Program
    {
        private const string DefaultDbConnection = "Host=localhost;Port=5432;Username=postgres;Password=admin;Persist Security Info=true;Database=MMDbContextV2;MaxPoolSize=1000;";
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(DefaultDbConnection, npgsql =>
                    npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "app")));

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
        }
    }
}
