using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MiniERP_desktop.Database;

namespace MiniERP_desktop.ViewModels
{
    class BuildOverviewViewModel : Screen
    {
        private IEventAggregator _eventAggregator;
        private ERPEntities _dbContext;
        private Build _build;
        private BindableCollection<Cordinate> _buildPlacement;
        private Cordinate _selectedCordinate;
       

        public BindableCollection<Cordinate> BuildPlacement
        {
            get => _buildPlacement;
            set
            {
                _buildPlacement = value;
                NotifyOfPropertyChange(() => BuildPlacement);
            }
        }

        public Cordinate SelectedCordinate
        {
            get => _selectedCordinate;
            set
            {
                _selectedCordinate = value;
                NotifyOfPropertyChange(() => SelectedCordinate);
            }
        }

        public void Selected()
        {
        }

        public BuildOverviewViewModel(IEventAggregator eventAggregator, ERPEntities dbContext, Build build)
        {
            _eventAggregator = eventAggregator;
            _dbContext = dbContext;
            _build = build;

            string xd = "Kopa ma ta kawa        ";
            var r = xd.Split(' ');
            String.Join(" ", r);

        }
    }
}
