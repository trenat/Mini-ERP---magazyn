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
        private object _overViewContent;
        private Build _build;

        public Build Build
        {
            get => _build;
            set
            {
                _build = value;
                NotifyOfPropertyChange(() => Build);
            }
        }

        public object OverViewContent
        {
            get => _overViewContent;
            set
            {
                _overViewContent = value;
                NotifyOfPropertyChange(() => OverViewContent);
            }
        }

        public CreateBuildViewModel(IEventAggregator eventAggregator, ERPEntities dbContext)
        {
            _dbContext = dbContext;
            _eventAggregator = eventAggregator;
            OverViewContent = new BuildOverviewViewModel(_eventAggregator, _dbContext, Build);
        }



    }
    
}
