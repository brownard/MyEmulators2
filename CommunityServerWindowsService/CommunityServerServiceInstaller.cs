using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Install;
using System.ComponentModel;
using System.ServiceProcess;

namespace CommunityServerWindowsService
{
    [RunInstaller(true)]
    public class CommunityServerServiceInstaller : Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;

        public CommunityServerServiceInstaller()
        {
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            service = new ServiceInstaller();
            service.ServiceName = "Community Server Service";
            Installers.Add(process);
            Installers.Add(service);
        }

    }
}
