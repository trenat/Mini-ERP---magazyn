using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MiniERP_desktop.Models;

namespace MiniERP_desktop.ViewModels
{
    public class InvoiceViewModel:Screen
    {
        private BindableCollection<string> _orientOption;
        private BindableCollection<string> _sizeOption;
        private IEventAggregator _eventAggregator;
        public BindableCollection<string> OrientOption
        {
            get => _orientOption;
            set
            {
                _orientOption = value;
                NotifyOfPropertyChange(() => OrientOption);
            }
        }
        public BindableCollection<string> SizeOption
        {
            get => _sizeOption;
            set
            {
                _sizeOption = value;
                NotifyOfPropertyChange(() => SizeOption);
            }
        }

        public void EditItems()
        {
            _eventAggregator.PublishOnUIThread(new StorageViewModel());
        }

        public InvoiceViewModel(IEventAggregator eventAggregator)
        {
            OrientOption = new BindableCollection<string>();
            SizeOption = new BindableCollection<string>();

            OrientOption.AddRange(Enum.GetNames(typeof(OrientationOption)));
            SizeOption.AddRange(Enum.GetNames(typeof(SizeOption)));

            _eventAggregator = eventAggregator;
            
        }
    }
}
