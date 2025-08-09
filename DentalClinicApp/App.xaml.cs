using DentalClinicApp.Data;
using DentalClinicApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Windows;


namespace DentalClinicApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((ctx, services) =>
                {
                    var connStr = ctx.Configuration.GetConnectionString("DefaultConnection");
                    services.AddDbContext<AppDbContext>(opt =>
                        opt.UseNpgsql(connStr));

                    var imgPath = ctx.Configuration["ImageBasePath"] ?? "./data";
                    services.AddSingleton(new ImageService(imgPath));
                    services.AddTransient<MainViewModel>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs\\app_.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            await _host.StartAsync();
            var mainWindow = new Views.MainWindow
            {
                DataContext = _host.Services.GetRequiredService<MainViewModel>()
            };
            mainWindow.Show();
            base.OnStartup(e);

            Log.Information("Приложение запущено");
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            Log.Information("Приложение завершено");
            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }

}
