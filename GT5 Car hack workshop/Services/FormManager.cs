using System;
using Microsoft.Extensions.DependencyInjection;
using GT5_Car_hack_workshop;

namespace GT5_Car_hack_workshop.Services
{
    public interface IFormManager
    {
        Form1 MainForm { get; }
        TransmissionEditor TransmissionEditor { get; }
        Customperformance CustomPerformance { get; }
    }

    public class FormManager : IFormManager
    {
        private readonly IServiceProvider _serviceProvider;
        
        public FormManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Form1 MainForm => _serviceProvider.GetRequiredService<Form1>();
        public TransmissionEditor TransmissionEditor => _serviceProvider.GetRequiredService<TransmissionEditor>();
        public Customperformance CustomPerformance => _serviceProvider.GetRequiredService<Customperformance>();
    }
} 