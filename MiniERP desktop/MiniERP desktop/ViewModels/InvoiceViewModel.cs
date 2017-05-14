using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;
using MiniERP_desktop.Models;
using MiniERP_desktop.ViewModels.ToolboxHelpers;
using MiniERP_desktop.Services.Events;
using MiniERP_desktop.Services.s;
using Xceed.Wpf.Toolkit;

namespace MiniERP_desktop.ViewModels
{
    public class InvoiceViewModel:Screen, IHandle<ColorSelected>, IHandle<ConvertPdf>
    {
        private BindableCollection<string> _orientOption;
        private BindableCollection<string> _sizeOption;
        private IEventAggregator _eventAggregator;
        private Database.Invoice _myInvoice;

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

        public string SelectedSizeOption
        {
            get;
            set;
            //get => MyInvoice.;
            //set { MyInvoice.PageSize = value; }

        }
        

        public Database.Invoice MyInvoice
        {
            get => _myInvoice;
            set
            {
                _myInvoice = value;
                NotifyOfPropertyChange(()=> MyInvoice);
            }
        }
        
    
        public InvoiceViewModel(IEventAggregator eventAggregator)
        {
            OrientOption = new BindableCollection<string>();
            SizeOption = new BindableCollection<string>();

            OrientOption.AddRange(Enum.GetNames(typeof(OrientationOption)));
            SizeOption.AddRange(Enum.GetNames(typeof(SizeOption)));

            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            MyInvoice = new Database.Invoice()
            {
                BackColor = "#FFFFFFFF",
                TextColor = "#FF000000"
            };
           

        }

        public void Handle(ColorSelected message)
        {
            string color = message.Color.ToString();
            switch (message.Index)
            {
                case 1: MyInvoice.BackColor = color; break;
                case 2: MyInvoice.TextColor = color; break;
            }

            NotifyOfPropertyChange(() => MyInvoice);
        }
        

        public void EditItems()
        {
            //var convertFromString = ColorConverter.ConvertFromString(MyInvoice.BackColor);
            //Color color = (Color)convertFromString;
            //_eventAggregator.PublishOnUIThread(new ColorPickerViewModel(_eventAggregator){RGB = color});
        }

        public void SelectColor(int i)
        {
            string strColor = "";
            Color color = new Color();
            switch (i)
            {
                case 1: strColor = MyInvoice.BackColor ; break;
                case 2: strColor = MyInvoice.TextColor ; break;
            }
            if (strColor != null)
            {
                color = (Color)ColorConverter.ConvertFromString(strColor);
            }
            _eventAggregator.PublishOnUIThread(new HelperScreen(){ Screen = new ColorPickerViewModel(_eventAggregator, color, i)});
        }

        public void Handle(ConvertPdf message)
        {
            MessageBox.Show(MyInvoice.ToString());
        }
    }
}
