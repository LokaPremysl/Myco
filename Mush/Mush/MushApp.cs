using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mush.src.Application.Ports;
using Mush.src.Infrastructure.Stores;
using static Mush.src.Application.Ports.IProjectService;
namespace Mush
{
    internal static class MushApp
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            using var host = Host.CreateDefaultBuilder().ConfigureServices(s =>
                {
                    s.AddSingleton<IMycologyStore, MycologyStore>();
                    //s.AddScoped<IProjectService, ProjectService>();
                    s.AddTransient<MainForm>();
                }).Build();
            Application.Run(host.Services.GetRequiredService<MainForm>());
        }
    }
}