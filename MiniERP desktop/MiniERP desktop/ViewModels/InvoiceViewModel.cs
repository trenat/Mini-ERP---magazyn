using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using MiniERP_desktop.Database;
using MiniERP_desktop.Helpers;
using MiniERP_desktop.Models;
using MiniERP_desktop.Services;
using MiniERP_desktop.ViewModels.ToolboxHelpers;
using MiniERP_desktop.Services.Events;
using MiniERP_desktop.Services.s;
using PdfSharp;
using Xceed.Wpf.DataGrid.Converters;
using DetailRow = MiniERP_desktop.Models.DetailRow;
using MessageBox = Xceed.Wpf.Toolkit.MessageBox;
using TotalRow = MiniERP_desktop.Models.TotalRow;

namespace MiniERP_desktop.ViewModels
{
    public class InvoiceViewModel:Screen, IHandle<ColorSelected>, IHandle<ConvertPdf>
    {
        private BindableCollection<string> _orientOption;
        private BindableCollection<string> _sizeOption;
        private BindableCollection<string> _companyOrient;
        private BindableCollection<string> _currency;
        private IEventAggregator _eventAggregator;
        private Database.Invoice _myInvoice;
        private ERPEntities _dbContext;

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
        public BindableCollection<string> CompanyOrient
        {
            get => _companyOrient;
            set
            {
                _companyOrient = value;
                NotifyOfPropertyChange(() => CompanyOrient);
            }
        }
        public BindableCollection<string> Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                NotifyOfPropertyChange(() => Currency);
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
            get => ((PageOrientation)Convert.ToInt16((bool)MyInvoice.PageOrientation)).ToString();
            set
            {
                PageOrientation o;
                PageOrientation.TryParse(value, out o);
                MyInvoice.PageOrientation = o > 0;
                _eventAggregator.PublishOnUIThread(new PageOrientationChanged() { Orientation = o });
                NotifyOfPropertyChange(() => SelectedOrientOption);
            }

        }
        public string SelectedCompanyOrient
        {
            get => ((PositionOption)Convert.ToInt16((bool)MyInvoice.CompanyOrientation)).ToString();
            set
            {
                PositionOption o;
                PositionOption.TryParse(value, out o);
                MyInvoice.CompanyOrientation = o > 0;
                _eventAggregator.PublishOnUIThread(new CompanyOrientationChanged() { Orientation = o });
                NotifyOfPropertyChange(() => SelectedOrientOption);
            }

        }
        public Database.Invoice MyInvoice
        {
            get => _myInvoice;
            private set
            {
                _myInvoice = value;
                NotifyOfPropertyChange(()=> MyInvoice);
            }
        }

        public string SelectedCurrency
        {
            get => _dbContext.Currency.Find(MyInvoice.CurrencyID).Name;
            set
            {
                MyInvoice.CurrencyID = _dbContext.Currency.FirstOrDefault(curr => curr.Name == value).ID;
                NotifyOfPropertyChange(() => SelectedCurrency);
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
        public bool AddLogo
        {
            get => (bool)MyInvoice.HaveLogo;
            set
            {
                MyInvoice.HaveLogo = value;
                NotifyOfPropertyChange(()=>AddLogo);
                RaiseLogoChanged();
            }
        }

        public double LogoHeight
        {
            get => (double)MyInvoice.LogoHeight;
            set
            {
                MyInvoice.LogoHeight = (int)value;
                NotifyOfPropertyChange(() => LogoHeight);
                RaiseLogoChanged();
            }
        }


        public double LogoWidth
        {
            get => (double)MyInvoice.LogoWidth;
            set
            {
                MyInvoice.LogoWidth = (int)value;
                NotifyOfPropertyChange(() => LogoWidth);
                RaiseLogoChanged();
            }
        }
        

        public InvoiceViewModel(IEventAggregator eventAggregator, ERPEntities dbContext)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _dbContext = dbContext;

            OrientOption = new BindableCollection<string>(Enum.GetNames(typeof(OrientationOption)));
            SizeOption = new BindableCollection<string>(Enum.GetNames(typeof(SizeOption)));
            CompanyOrient = new BindableCollection<string>(Enum.GetNames(typeof(PositionOption)));
            Currency = new BindableCollection<string>(_dbContext.Currency.Select(x => x.Name).ToList());
            

            MyInvoice = new Database.Invoice()  
            {
                BackColor = "#FFFFFFFF",
                TextColor = "#FF000000",
                BillingDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(14),
                Title = "Invoice",
                PageOrientation = false,
                PageSize = 0,
                CurrencyID = 1,
                Reference = $"{DateTime.Now.GetShortYear()}{DateTime.Now.GetWeekNumber()}{(int) DateTime.Now.DayOfWeek}",
                LogoHeight = 27,
                LogoWidth = 125,
                CompanyOrientation = false,
                HaveLogo = false
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
            string LogoPath = "";
            if ((bool)MyInvoice.HaveLogo)
                LogoPath = @"..\..\images\vodafone.jpg";

            new InvoicerApi(Models.SizeOption.A4, OrientationOption.Portrait, "zł")
                .TextColor(MyInvoice.TextColor)
                .BackColor(MyInvoice.BackColor)
                .BillingDate((DateTime)MyInvoice.BillingDate)
                .DueDate((DateTime)MyInvoice.DueDate)
                .Title(MyInvoice.Title)
                .Image(LogoPath, 125, 27)
                .Company(Models.Address.Make("FROM", new string[] { "Vodafone Limited", "Vodafone House", "The Connection", "Newbury", "Berkshire RG14 2FN" }, "1471587", "569953277"))
                .Client(Models.Address.Make("BILLING TO", new string[] { "Isabella Marsh", "Overton Circle", "Little Welland", "Worcester", "WR## 2DJ" }))
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

        private void RaiseLogoChanged()
        {
            Visibility v;
            if(AddLogo)
                v = Visibility.Visible;
            else 
                v = Visibility.Collapsed;
           _eventAggregator.PublishOnUIThread(new LogoChanged(){AddLogo = v, Width = this.LogoWidth, Height = this.LogoHeight});
        }

    }
}
