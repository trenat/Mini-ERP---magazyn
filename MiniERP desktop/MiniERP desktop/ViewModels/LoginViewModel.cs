using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Caliburn.Micro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MiniERP_desktop.Database;
using MiniERP_desktop.Models;
using MiniERP_desktop.Services;
using MiniERP_desktop.Views;

namespace MiniERP_desktop.ViewModels
{
    public class LoginViewModel : Screen
    {
        private IWindowManager _windowManager;
        private readonly SimpleContainer _container;
        private readonly ERPEntities _dbContext;
        private string _password;
        private string _username;
        private IEventAggregator _eventAggregator;
       
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

        


        public LoginViewModel(SimpleContainer container, ERPEntities dbContext, IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            var v = new ObjectDataProvider();
            _container = container;
            _eventAggregator = eventAggregator;
            _dbContext = dbContext;
            _windowManager = windowManager;
        }



        protected override void OnInitialize()
        {
            base.OnInitialize();
            DisplayName = "Storage ERP";


        }


        public void Login()
        {
            //User user = VeryfyUser(Username, Password);
            //if (user != null)
            //{
                
                _windowManager.ShowWindow(new ShellViewModel(_container, new User(){FirstName = "Dawid"}, _eventAggregator, _dbContext));
                TryClose();
            //}
            //else
            //{
                MessageBox.Show("Błędny login lub hasło");
            //}
        }

        private User VeryfyUser(string username, string password)
        {
            User user = _dbContext.User.FirstOrDefault(u => u.Login == username );
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
