using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MiniERP_desktop.Database;

namespace MiniERP_desktop.ViewModels
{
    class HomeViewModel:Screen
    {
        private IEventAggregator _eventAggregator;
        private ERPEntities _dbContext;
        private User _user;

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                NotifyOfPropertyChange(() => User);
            }
        }

        public HomeViewModel(IEventAggregator eventAggregator, ERPEntities dbContext, User user)
        {
            _eventAggregator = eventAggregator;
            
            _dbContext = dbContext;
            _user = user;
        }
    }
}
