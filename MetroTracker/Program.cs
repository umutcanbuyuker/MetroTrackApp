using MetroTracker.Controllers;
using MetroTracker.Hubs;
using MetroTracker.Kafka.Consumer;
using MetroTracker.Kafka.Producers;

namespace MetroTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton<LocationConsumer>(); 
            builder.Services.AddHostedService<M1Producer>();
            builder.Services.AddLogging(i =>
            {
                i.AddConsole();
                i.AddDebug();
            });
            builder.Services.AddSignalR();
            //builder.Services.AddScoped<M1ConsumerA>();
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapHub<ConsumerHub>("/ConsumerHub");
            

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
