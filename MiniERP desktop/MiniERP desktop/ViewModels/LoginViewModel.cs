using System.Windows;
using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MiniERP_desktop.Views;

namespace MiniERP_desktop.ViewModels
{
    public class LoginViewModel:Screen
    { 
        private WindowManager _windowManager;
        private readonly SimpleContainer _container;

      public LoginViewModel(SimpleContainer container)
      {
          _container = container;

            

      }



        protected override void OnInitialize()
        {
            base.OnInitialize();
            DisplayName = "Storage ERP";
            
            
        }
        

        public void Login(string password, string username)
        {
            if (username.Length >= 1)
            {
                _windowManager = _container.GetInstance<WindowManager>();
                if(_windowManager == null)
                {
                    _windowManager = new WindowManager();
                    _container.Instance(_windowManager);
                }
                _windowManager.ShowWindow(new ShellViewModel(_container));
                TryClose();
            }
            else
                MessageBox.Show("Niezła próba xD");
        }
    }
}
