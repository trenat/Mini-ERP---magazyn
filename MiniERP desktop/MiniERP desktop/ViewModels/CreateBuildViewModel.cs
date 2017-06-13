using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MiniERP_desktop.Database;

namespace MiniERP_desktop.ViewModels
{
    class CreateBuildViewModel:Screen
    {
        private IEventAggregator _eventAggregator;
        private ERPEntities _dbContext;

        public CreateBuildViewModel(IEventAggregator eventAggregator, ERPEntities dbContext)
        {
            _dbContext = dbContext;
            _eventAggregator = eventAggregator;
        }



    }
    
}
