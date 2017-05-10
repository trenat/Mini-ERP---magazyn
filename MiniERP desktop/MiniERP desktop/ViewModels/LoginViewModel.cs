using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MiniERP_desktop.Database;
using MiniERP_desktop.Services;
using MiniERP_desktop.Views;

namespace MiniERP_desktop.ViewModels
{
    public class LoginViewModel : Screen
    {
        private WindowManager _windowManager;
        private readonly SimpleContainer _container;
        private readonly ERPEntities _dbContext;
        private string _password;
        private string _username;

        public string Password
        {
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
            }
            get => _password;
        }

        public string Username
        {
            set
            {
                _username = value;
                NotifyOfPropertyChange(() => Username);
            }
            get => _username;
        }

        public LoginViewModel(SimpleContainer container, ERPEntities dbContext)
        {
            _container = container;
            _dbContext = dbContext;
        }



        protected override void OnInitialize()
        {
            base.OnInitialize();
            DisplayName = "Storage ERP";


        }


        public void Login()
        {
            User user = VeryfyUser(Username, Password);
            _windowManager = _container.GetInstance<WindowManager>();
            if (_windowManager == null)
            {
                _windowManager = new WindowManager();
                _container.Instance(_windowManager);
            }

            if (user != null)
            {
                _windowManager.ShowWindow(new ShellViewModel(_container, user));
                TryClose();
            }
            else
            {
                MessageBox.Show("Błędny login lub hasło");
            }
        }

        private User VeryfyUser(string username, string password)
        {
            User user = _dbContext.User.FirstOrDefault(b => b.Login.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                if (PasswordHasher.VerifyHashedPassword(user.HashPassword, password))
                {
                    return user;
                }
            }
            return null;
        }
    }
}
