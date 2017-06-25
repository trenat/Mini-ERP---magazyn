using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MiniERP_desktop.Database;

namespace MiniERP_desktop.ViewModels
{
    class AddItemsViewModel:Screen
    {
        private IEventAggregator _eventAggregator;
        private ERPEntities _dbContext; 

        public AddItemsViewModel(IEventAggregator eventAggregator, ERPEntities dbContext)
        {
            _eventAggregator = eventAggregator;
            _dbContext = dbContext;
        }
    }
}
