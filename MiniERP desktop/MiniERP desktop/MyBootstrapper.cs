using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using MiniERP_desktop.ViewModels;

namespace MiniERP_desktop
{
    public class MyBootstrapper: BootstrapperBase
    {
        private SimpleContainer container;

        public MyBootstrapper()
        {
            Initialize();
        }
        protected override void Configure()
        {
            container = new SimpleContainer();

            container.Instance(container);

            container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();

            container
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
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            MessageBox.Show(e.Exception.Message, "An error as occurred", MessageBoxButton.OK);
        }
    }
}
