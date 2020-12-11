using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaiKanStudentDiningManagementSystem.Api.Configurations
{
    public class ServiceLocator
    {
        public static IServiceProvider Services { get; private set; }
        public static void SetServices(IServiceProvider services)
        {
            Services = services;
        }
    }
}
