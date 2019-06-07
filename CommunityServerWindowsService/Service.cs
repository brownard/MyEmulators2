using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using System.IO;
using NHibernate;
using NHibernate.Cfg;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using MySql.Data.MySqlClient;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate;
using System.Threading;

[assembly: log4net.Config.XmlConfigurator(Watch=true)]
namespace CommunityServerWindowsService
{
    public partial class Service : ServiceBase
    {
        public static ServiceHost host = null;
        public static Engine engine = null;

        public Service()
        {
            InitializeComponent();
            this.CanStop = true;
            this.ServiceName = "My Emulators 2 Community Server"; 
        }

        public static void Main()
        {
            ServiceBase.Run(new Service());
        }

        protected override void OnStart(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();

            Log.Info("Service Starting");

            // Write unhandled exceptions to log.
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Log.Info("Stating service");

            try
            {
                Log.Info("Stating engine");
                engine = new Engine();

                Log.Info("Creating WCF host");
                host = new ServiceHost(typeof(CommunityServerWFCService));

                // start WebService
                Log.Info("Starting WCF host");
                host.Open();

            }
            catch (Exception ex)
            {
                Log.Fatal("Exception: " + ex.ToString());
                System.Environment.Exit(1);
            }

            Log.Info("Service started");
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal(e.ToString());
        }

        protected override void OnStop()
        {
            if (host != null)
            {
                host.Close();
                host = null;
                engine = null;
            }
        }

        private static readonly object logLock = new object();

        private static log4net.ILog log = null;
        public static log4net.ILog Log
        {
            get
            {
                lock (logLock)
                {
                    if (log == null)
                    {
                        log = log4net.LogManager.GetLogger(typeof(Service));
                    }
                    return Service.log;
                }
            }
        }

        private static readonly object sessionLock = new object();

		private static ISessionFactory sessionFactory = null;
		public static ISessionFactory SessionFactory
		{
			get
			{
				lock (sessionLock)
				{
					if (sessionFactory == null)
					{
						sessionFactory = CreateSessionFactory();
					}
					return Service.sessionFactory;
				}
			}
		}

        private static ISessionFactory CreateSessionFactory()
        {

            Log.Info("Creating Fluent Nhibernate session");

            FluentConfiguration config = Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                .ConnectionString(c => c
                        .Server(System.Configuration.ConfigurationManager.AppSettings["DBServer"].ToString())
                        .Database(System.Configuration.ConfigurationManager.AppSettings["DBSchema"].ToString())
                        .Username(System.Configuration.ConfigurationManager.AppSettings["DBUsername"].ToString())
                        .Password(System.Configuration.ConfigurationManager.AppSettings["DBPassword"].ToString())));

            try
            {
                var factory = config.Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<GameMap>())
                    .ExposeConfiguration(BuildSchema)
                    .BuildSessionFactory();

                Log.Info("Returning DB session");
                return factory;
            }
            catch (FluentNHibernate.Cfg.FluentConfigurationException ex)
            {
                Log.Fatal("Error creating Database session. Error: " + ex.InnerException.Message);


                Log.Fatal("Killing Server");
                System.Environment.Exit(1);
            }


            return null;
        }

        private static void BuildSchema(NHibernate.Cfg.Configuration config)
        {
            Log.Info("Exporting schema");

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            /*SchemaExport export = new SchemaExport(config);
            using (TextWriter stringWriter = new StreamWriter("c:\\create.sql"))
            {
                export.Execute(false, false, false, null, stringWriter);
            }*/

              /*new SchemaExport(config)
                  .Create(false, true);*/

            // This will auto update the DB to match our mappings.
              new SchemaUpdate(config).Execute(false, true);
        }
    }
}
