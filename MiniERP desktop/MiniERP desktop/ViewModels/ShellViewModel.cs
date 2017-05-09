using System.Windows;
using Caliburn.Micro;

namespace MiniERP_desktop.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        int count = 1;

        private readonly WindowManager _windowManager;
        private readonly SimpleContainer _container;
        private object _controlContent;

        public object ControlContent
        {
            get => _controlContent;
            set
            {
                _controlContent = value;
                NotifyOfPropertyChange(()=> ControlContent);
            }
        }

        public ShellViewModel(SimpleContainer container)
        {
            _container = container;
            _windowManager = container.GetInstance<WindowManager>();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            this.DisplayName = "Storage ERP";
        }

        public void OpenTab()
        {
            
            ActivateItem(new TabViewModel
            {
                DisplayName = "Tab " + count++
            });
        }

        public void Settings()
        {
            MessageBox.Show("Settings");
        }

        public void Storage()
        {
            
            ControlContent = new StorageViewModel()
            {
                DisplayName = "Tab " + count++
            };
        }

        public void Reports()
        {
            ControlContent = new ReportsViewModel()
            {
                DisplayName = "Tab " + count++
            };
        }

        public void Logout()
        {
            _windowManager.ShowWindow(new LoginViewModel(_container));
            this.TryClose();
        }

        public void CloseItem(Screen screen)
        {
            DeactivateItem(screen, true);
        }
    }
}
