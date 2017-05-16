using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;
using MiniERP_desktop.Helpers;
using MiniERP_desktop.Models;
using MiniERP_desktop.Services;
using MiniERP_desktop.ViewModels.ToolboxHelpers;
using MiniERP_desktop.Services.Events;
using MiniERP_desktop.Services.s;
using PdfSharp;
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

            get => ((SizeOption)MyInvoice.PageSize).ToString();
            set
            {
                NotifyOfPropertyChange(() => SelectedSizeOption);
                PageSize p;
                PageSize.TryParse(value, out p);
                MyInvoice.PageSize = (int)p;
                _eventAggregator.PublishOnUIThread(new PageSizeChanged(){PageSize = p});
            }

        }
        public string SelectedOrientOption
        {
            get => ((PageOrientation) Convert.ToInt16((bool) MyInvoice.PageOrientation)).ToString();
            set
            {
                PageOrientation o;
                PageOrientation.TryParse(value, out o);
                MyInvoice.PageOrientation = o > 0;
                _eventAggregator.PublishOnUIThread(new PageOrientationChanged() { Orientation = o});
                NotifyOfPropertyChange(() => SelectedOrientOption);
            }

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

        public string Title
        {
            get => MyInvoice.Title;
            set
            {
                MyInvoice.Title = value;
                NotifyOfPropertyChange(() => Title);
                _eventAggregator.PublishOnUIThread(new TitleChanged(){Title = value});
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
                TextColor = "#FF000000",
                BillingDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(14),
                Title = "Invoice VAT",
                PageOrientation = false,
                PageSize = 0,
                Currency = "zł",
                Reference = string.Format("{0}{1}{2}", DateTime.Now.GetShortYear(), DateTime.Now.GetWeekNumber(), (int)DateTime.Now.DayOfWeek)

            };

            NotifyOfPropertyChange(() => MyInvoice);
           

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

        public void SelectClient()
        {

            _eventAggregator.PublishOnUIThread(new HelperScreen() { Screen = new SearchUserViewModel() });
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

            //var filename = System.IO.Path.ChangeExtension(Invoice.Reference, "pdf");
            //new PdfInvoice(MyInvoice).Save(filename);

            new InvoicerApi(Models.SizeOption.A4, OrientationOption.Portrait, "zł")
                .TextColor(MyInvoice.TextColor)
                .BackColor(MyInvoice.BackColor)
                .BillingDate((DateTime)MyInvoice.BillingDate)
                .DueDate((DateTime)MyInvoice.DueDate)
                //.Image(@"..\..\images\vodafone.jpg", 125, 27)
                .Company(Address.Make("FROM", new string[] { "Vodafone Limited", "Vodafone House", "The Connection", "Newbury", "Berkshire RG14 2FN" }, "1471587", "569953277"))
                .Client(Address.Make("BILLING TO", new string[] { "Isabella Marsh", "Overton Circle", "Little Welland", "Worcester", "WR## 2DJ" }))
                .Items(new List<ItemRow> {
                    ItemRow.Make("Nexus 6", "Midnight Blue", (decimal)1, 20, (decimal)166.66, (decimal)199.99),
                    ItemRow.Make("24 Months (£22.50pm)", "100 minutes, Unlimited texts, 100 MB data 3G plan with 3GB of UK Wi-Fi", (decimal)1, 20, (decimal)360.00, (decimal)432.00),
                    ItemRow.Make("Special Offer", "Free case (blue)", (decimal)1, 0, (decimal)0, (decimal)0),
                })
                .Totals(new List<TotalRow> {
                    TotalRow.Make("Sub Total", (decimal)526.66),
                    TotalRow.Make("VAT @ 20%", (decimal)105.33),
                    TotalRow.Make("Total", (decimal)631.99, true),
                })
                .Details(new List<DetailRow> {
                    DetailRow.Make("PAYMENT INFORMATION", "Make all cheques payable to Vodafone UK Limited.", "", "If you have any questions concerning this invoice, contact our sales department at sales@vodafone.co.uk.", "", "Thank you for your business.")
                })
                .Footer("http://www.vodafone.co.uk")
                .Save();
            MessageBox.Show(MyInvoice.ToString());
        }


    }
}
