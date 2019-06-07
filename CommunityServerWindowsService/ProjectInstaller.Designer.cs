namespace CommunityServerWindowsService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CommunityServerWindowsServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.CommunityServerWindowsServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // CommunityServerWindowsServiceProcessInstaller
            // 
            this.CommunityServerWindowsServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.CommunityServerWindowsServiceProcessInstaller.Password = null;
            this.CommunityServerWindowsServiceProcessInstaller.Username = null;
            // 
            // CommunityServerWindowsServiceInstaller
            // 
            this.CommunityServerWindowsServiceInstaller.Description = "My Emulators 2 Community Server Windows Service";
            this.CommunityServerWindowsServiceInstaller.DisplayName = "My Emulators 2 Community Server";
            this.CommunityServerWindowsServiceInstaller.ServiceName = "My Emulators 2 Community Server";
            this.CommunityServerWindowsServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.CommunityServerWindowsServiceProcessInstaller,
            this.CommunityServerWindowsServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller CommunityServerWindowsServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller CommunityServerWindowsServiceInstaller;
    }
}