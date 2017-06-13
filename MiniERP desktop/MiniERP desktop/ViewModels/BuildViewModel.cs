using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MiniERP_desktop.Database;

namespace MiniERP_desktop.ViewModels
{
    public class BuildViewModel:Screen
    {
        private Build _build;
        private IEventAggregator _eventAggregator;
        private ERPEntities _dbContex; 

        public BuildViewModel(IEventAggregator eventAggregator, ERPEntities dbContext, Build build)
        {
            _eventAggregator = eventAggregator;
            _dbContex = dbContext;
            _build = build;
        }
    }
}
