using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using GT5_Car_hack_workshop.Services;
using System;

namespace GT5_Car_hack_workshop
{
    public partial class App : Application
    {
        public IServiceProvider? ServiceProvider { get; private set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var formManager = ServiceProvider.GetRequiredService<IFormManager>();
                desktop.MainWindow = formManager.MainForm;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register forms
            services.AddSingleton<MainWindow>();

            // Register services
            services.AddSingleton<IFormManager, FormManager>();
        }
    }
}