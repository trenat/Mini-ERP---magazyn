using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using MiniERP_desktop.Database;

namespace MiniERP_desktop.ViewModels
{
    class NewItemsViewModel:Screen
    {
        private IEventAggregator _eventAggregator;
        private ERPEntities _dbContext;
        private Item _item;

        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
                NotifyOfPropertyChange(() => Item);
            }
        }

        public NewItemsViewModel(IEventAggregator eventAggregator, ERPEntities dbContext)
        {
            _eventAggregator = eventAggregator;
            _dbContext = dbContext;
            _item = new Item();
        }

        public void AddItem()
        {
            _dbContext.Item.Add(Item);
            _dbContext.SaveChangesAsync();
        }


    }
}
