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
                    s.AddSingleton<IMycologyStore, MycologyStore>();
                    s.AddSingleton<ITextService>(sp => new TextService("cs"));
                    s.AddSingleton<JsonMycologyStore>();
                    //s.AddScoped<IProjectService, ProjectService>();
                    s.AddTransient<MainForm>();
                    s.AddTransient<SpawnDialog>();
                    s.AddTransient<InputDialog>();
                    s.AddTransient<IMycologyPresenter, MycologyPresenter>();
                }).Build();
            
            var sp = host.Services;

            Application.Run(sp.GetRequiredService<MainForm>());
        }
    }
}