using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mush.AppLayer.Ports;
using Mush.Infrastructure.Stores;
using Mush.Infrastructure.Localization;
using Mush.AppLayer.Services;
using static Mush.AppLayer.Ports.IProjectService;
using Mush.WinForms;
using System;
using System.Windows.Forms;

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
                    //s.AddSingleton<ITextService>(_ => new TextService("cs"));
                    //s.AddSingleton<ITextService>(sp => (ITextService)new TextService("cs"));
                    s.AddSingleton<ITextService>(sp => new TextService("cs"));
                    s.AddSingleton<IMycologyStore, MycologyStore>();
                    s.AddScoped<IProjectService, ProjectService>();
                    s.AddTransient<MainForm>();
                    s.AddTransient<SpawnDialog>();
                    s.AddTransient<InputDialog>();
                    s.AddSingleton<JsonMycologyStore>();
                }).Build();
            //Application.Run(host.Services.GetRequiredService<MainForm>());
            
            using var scope = host.Services.CreateScope();
            var sp = scope.ServiceProvider;

            Application.Run(sp.GetRequiredService<MainForm>());
        }
    }
}