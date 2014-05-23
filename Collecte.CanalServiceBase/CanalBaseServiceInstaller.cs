using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Collecte.CanalServiceBase
{
	[RunInstaller(true)]
	[System.ComponentModel.DesignerCategory("Code")] // this tell vs2010 to open the file in code mode directly
	public partial class CanalBaseServiceInstaller : System.Configuration.Install.Installer
	{
		public CanalBaseServiceInstaller()
		{
			//InitializeComponent();
			ServiceProcessInstaller processInstaller = new ServiceProcessInstaller();
			processInstaller.Account = ServiceAccount.LocalSystem;

			ServiceInstaller mainServiceInstaller = new ServiceInstaller();
			mainServiceInstaller.ServiceName = "CanalBaseService";
			mainServiceInstaller.Description = "CanalBaseService";
			mainServiceInstaller.StartType = ServiceStartMode.Automatic;

			Installers.Add(processInstaller);
			Installers.Add(mainServiceInstaller);
		}
	}
}
