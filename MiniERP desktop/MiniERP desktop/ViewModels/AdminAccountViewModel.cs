using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using MiniERP_desktop.Database;

namespace MiniERP_desktop.ViewModels
{
    class AdminAccountViewModel:Screen
    {
        private BindableCollection<User> _usersList;
        private ERPEntities _dbContext;

        public BindableCollection<User> UsersList
        {
            set
            {
                _usersList = value;
                NotifyOfPropertyChange(()=>UsersList);
            }
            get => _usersList;
        }

        public AdminAccountViewModel(SimpleContainer container)
        {
            _dbContext = container.GetInstance<ERPEntities>();

            UsersList = new BindableCollection<User>(_dbContext.User.ToList());
        }
    }
}
