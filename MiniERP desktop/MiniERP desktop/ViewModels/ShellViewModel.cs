using System.Windows;
using Caliburn.Micro;
using MiniERP_desktop.Database;

namespace MiniERP_desktop.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly WindowManager _windowManager;
        private readonly SimpleContainer _container;
        private readonly User _currentUser; 
        private object _controlContent;
        private IEventAggregator _eventAggregator;
        private ERPEntities _dbContext;

        public User CurrentUser
        {
            get => _currentUser;
        }
        public object ControlContent
        {
            get => _controlContent;
            set
            {
                _controlContent = value;
                NotifyOfPropertyChange(()=> ControlContent);
            }
        }

        public ShellViewModel(SimpleContainer container, User user, IEventAggregator eventAggregator, ERPEntities dbContext)
        {
            _container = container;
            _currentUser = user;
            _windowManager = container.GetInstance<WindowManager>();
            _eventAggregator = eventAggregator;
            _dbContext = dbContext;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            this.DisplayName = "Storage ERP";
        }

        public void OpenTab()
        {
            
            //ActivateItem(new TabViewModel
            //{
            //    DisplayName = "Ta
            //});
        }

        public void Settings()
        {
            MessageBox.Show("Settings");
        }

        public void Account()
        {
            if (_currentUser.isAdmin)
            {
                ControlContent = new AdminAccountViewModel(_container);
            }
            else
            {
                
            }
        }

        public void Storage()
        {
            
            ControlContent = new StorageViewModel()
            {
                DisplayName = "Tab "// + count++
            };
        }

        public void Reports()
        {
            ControlContent = new ReportsViewModel(_eventAggregator, _dbContext)
            {
                DisplayName = "Tab "// + count++
            };
        }

        public void Logout()
        {
            //_windowManager.ShowWindow(new LoginViewModel(_container));
            this.TryClose();
        }

        public void CloseItem(Screen screen)
        {
            DeactivateItem(screen, true);
        }
    }
}
