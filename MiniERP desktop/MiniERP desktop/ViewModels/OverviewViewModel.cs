using System;
using System.Collections.Generic;
using System.Windows.Media;
using Caliburn.Micro;
using MigraDoc.DocumentObjectModel;
using MiniERP_desktop.Models;
using MiniERP_desktop.Services;
using MiniERP_desktop.Services.Events;
using Color = System.Windows.Media.Color;

namespace MiniERP_desktop.ViewModels
{
    public class OverviewViewModel:Screen, IHandle<ColorSelected>
    {
        private Brush _textColor;
        private Brush _backColor;
        private IEventAggregator _eventAggregator;
        private Document _document;

        public OverviewViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            TextColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            BackColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));



            Document = new InvoicerApi(Models.SizeOption.A4, OrientationOption.Portrait, "zł")
                .TextColor(TextColor.ToString())
                .BackColor(BackColor.ToString())
                .BillingDate(DateTime.Today)
                .DueDate(DateTime.Today.AddDays(14))
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
                .GenerateDocumenteWithoutSave();
        }

        public Brush TextColor
        {
            get => _textColor;
            set
            {
                _textColor = value;
                NotifyOfPropertyChange(() => TextColor);
            }
        }

        public Brush BackColor
        {
            get => _backColor;
            set
            {
                _backColor = value;
                NotifyOfPropertyChange(() => BackColor);
            }
        }

        public Document Document
        {
            get => _document;
            set
            {
                _document = value;
                NotifyOfPropertyChange(() => Document);
            }
        }

        public void Handle(ColorSelected message)
        {
            Brush brush = new SolidColorBrush(message.Color);
            switch (message.Index)
            {
                case 1: BackColor = brush; break;
                case 2: TextColor = brush; break;
            }

        }
    }
}
