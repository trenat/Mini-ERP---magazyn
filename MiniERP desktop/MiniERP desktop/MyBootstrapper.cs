using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Caliburn.Micro;
using MiniERP_desktop.Database;
using MiniERP_desktop.Helpers;
using MiniERP_desktop.Services;
using MiniERP_desktop.ViewModels;


namespace MiniERP_desktop
{
    public sealed class MyBootstrapper: BootstrapperBase
    {
        private SimpleContainer _container;
//        private MyDbContext
        public MyBootstrapper()
        {
            Initialize();
            ConventionManager.AddElementConvention<PasswordBox>(
                PasswordBoxHelper.BoundPasswordProperty,
                "Password",
                "PasswordChanged");
        }
        protected override void Configure()
        {
            
            
                
            _container = new SimpleContainer();
            //CompositionBatch batch = new CompositionBatch();

            //batch.AddExportedValue<IWindowManager>(new WindowManager());
            //batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            //batch.AddExportedValue(container);
            //using (ERPEntities e = new ERPEntities())
            //{
            //    e.User.Add(new User()
            //    {
            //        Age = 15,
            //        Email = "dawiddo14@gmail.com",
            //        FirstName = "Dawid",
            //        Gender = true,
            //        HashPassword = PasswordHasher.HashPassword("123456"),
            //        LastName = "Sitek",
            //        Login = "Admin",
            //        Phone = "575079835",
            //        isAdmin = true
            //    });
            //    e.SaveChanges();

            //}

            _container.Instance(_container);

            ERPEntities dbContext = new ERPEntities();

            //_container
            //    .Singleton<IWindowManager, WindowManager>()
            //    .Singleton<IEventAggregator, EventAggregator>();
            //    //.

            _container
                .Instance(dbContext)
                .Instance<IWindowManager>(new WindowManager())
                .Instance<IEventAggregator>(new EventAggregator());
            _container
                .PerRequest<LoginViewModel>(); 
            //.PerRequest<MenuViewModel>()
            //.PerRequest<BindingsViewModel>()
            //.PerRequest<ActionsViewModel>()
            //.PerRequest<CoroutineViewModel>()
            //.PerRequest<ExecuteViewModel>()
            //.PerRequest<EventAggregationViewModel>()
            //.PerRequest<DesignTimeViewModel>()
            //.PerRequest<ConductorViewModel>()
            //.PerRequest<BubblingViewModel>();
        }

      
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<LoginViewModel>();
        }
        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(e.Exception.Message, "An error as occurred", MessageBoxButton.OK);
        }
    }
}
