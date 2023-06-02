using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StogieSpotter.App.Services
{
    public class MyServiceLocator : Microsoft.Maui.Hosting.IMauiInitializeService
    {
        public static IServiceProvider Services { get; private set; }

        public void Initialize(IServiceProvider services)
        {
            Services = services;
        }
    }
}