using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            _container.Instance(_container);
            
            _container
                .Instance(new ERPEntities())
                .Instance<IWindowManager>(new WindowManager())
                .Instance<IEventAggregator>(new EventAggregator());
            _container
                .PerRequest<LoginViewModel>(); 

            MessageBinder.SpecialValues.Add("$mousepoint", ctx =>
            {
                var e = ctx.EventArgs as MouseEventArgs;
                if (e == null)
                    return null;
                return e.GetPosition(ctx.Source);
            });
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
