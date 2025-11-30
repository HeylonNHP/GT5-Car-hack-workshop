using System;
using Microsoft.Extensions.DependencyInjection;

namespace GT5_Car_hack_workshop.Services
{
    public interface IFormManager
    {
        MainWindow MainForm { get; }
    }

    public class FormManager : IFormManager
    {
        private readonly IServiceProvider _serviceProvider;

        public FormManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public MainWindow MainForm => _serviceProvider.GetRequiredService<MainWindow>();
    }
}
