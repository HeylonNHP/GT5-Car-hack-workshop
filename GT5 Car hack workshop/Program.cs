using System;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using GT5_Car_hack_workshop.Services;

namespace GT5_Car_hack_workshop
{
    static class Program
    {
        private static IServiceProvider _serviceProvider;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            using (var scope = _serviceProvider.CreateScope())
            {
                var formManager = scope.ServiceProvider.GetRequiredService<IFormManager>();
                Application.Run(formManager.MainForm);
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Register forms
            services.AddSingleton<Form1>();
            services.AddTransient<TransmissionEditor>();
            services.AddTransient<Customperformance>();

            // Register services
            services.AddSingleton<IFormManager, FormManager>();
        }
    }
} 