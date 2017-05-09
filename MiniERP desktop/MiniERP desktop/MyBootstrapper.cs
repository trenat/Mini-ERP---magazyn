using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using MiniERP_desktop.ViewModels;

namespace MiniERP_desktop
{
    public class MyBootstrapper: BootstrapperBase
    {
        private SimpleContainer _container;

        public MyBootstrapper()
        {
            Initialize();
        }
        protected override void Configure()
        {
            _container = new SimpleContainer();

            _container.Instance(_container);

            
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();

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
