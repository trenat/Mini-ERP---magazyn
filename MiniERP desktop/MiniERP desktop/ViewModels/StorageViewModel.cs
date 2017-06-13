using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MiniERP_desktop.Database;

namespace MiniERP_desktop.ViewModels
{
    public class StorageViewModel : Screen
    {

        private IEventAggregator _eventAggregator;
        private ERPEntities _dbContext;
        private BindableCollection<Build> _builds;
        private Build _build;
        private object _content;

        public Build Build
        {
            get => _build;
            set
            {
                _build = value;
                NotifyOfPropertyChange(() => Build);
            }
        }

        public BindableCollection<Build> Builds
        {
            get => _builds;
            set
            {
                _builds = value;
                NotifyOfPropertyChange(() => Builds);
            }
        }


        public object Content
        {
            set
            {
                _content = value;
                NotifyOfPropertyChange(() => Content);
            }
            get => _content;
        }



        public void Create()
        {
            Content = new CreateBuildViewModel(_eventAggregator, _dbContext);
        }

        public void BuildSelectionChanged(Build build)
        {
            Content = new BuildViewModel(_eventAggregator, _dbContext, build);
        }


        public StorageViewModel(IEventAggregator eventAggregator, ERPEntities dbContext)
        {
            _eventAggregator = eventAggregator;
            Builds = new BindableCollection<Build>();
            Builds.Add(new Build() { ID = 1 });
            Builds.Add(new Build() { ID = 2 });
            Builds.Add(new Build() { ID = 3 });
            _dbContext = dbContext;
        }

    }
}
