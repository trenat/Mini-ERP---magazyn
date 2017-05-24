using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using MigraDoc.DocumentObjectModel;
using MiniERP_desktop.Helpers;
using MiniERP_desktop.Models;
using MiniERP_desktop.Services;
using MiniERP_desktop.Services.Events;
using PdfSharp;
using Color = System.Windows.Media.Color;

namespace MiniERP_desktop.ViewModels
{
    public class OverviewViewModel : Screen, IHandle<ColorSelected>, IHandle<PageOrientationChanged>,
        IHandle<PageSizeChanged>, IHandle<TitleChanged>, IHandle<CompanyOrientationChanged>, IHandle<LogoChanged>
    {
        private Brush _textColor;
        private Brush _backColor;
        private IEventAggregator _eventAggregator;
        private Double _pageWidth;
        private Double _pageHeight;
        private Document _document;
        private PageOrientation _pageOrientation;
        private PageSize _pageSize;
        private Double _addres1Width;
        private Double _addres2Width;
        private Double _invoiceColumnWidth;
        private string _rightAddress;
        private string _leftAddress;
        private string _title;
        private Visibility _addLogo;
        private double _logoHeight;
        private double _logoWidth;

        public OverviewViewModel(IEventAggregator eventAggregator, string title)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            TextColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            BackColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            _pageOrientation = PageOrientation.Portrait;
            PageSize = PageSize.A4;
            Title = title;
            LeftAddress = "Company Address ";
            RightAddress = "Client Address";
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

        public Double PageWidth
        {
            get => _pageWidth;
            set
            {
                _pageWidth = value;
                NotifyOfPropertyChange(() => PageWidth);
                Addres1Width = value / 2 - 50;
                Addres2Width = value / 2 - 30;
                InvoiceColumnWidth = (value - 10) / 2;
            }
        }

        public Double PageHeight
        {
            get => _pageHeight;
            set
            {
                _pageHeight = value;
                NotifyOfPropertyChange(() => PageHeight);

            }
        }

        public PageOrientation PageOrientation
        {
            get => _pageOrientation;
            set
            {
                _pageOrientation = value;
                NotifyOfPropertyChange(() => PageOrientation);
                SetWidthAndHeight();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        public string LeftAddress
        {
            get => _leftAddress;
            set
            {
                _leftAddress = value;
                NotifyOfPropertyChange(() => LeftAddress);
            }
        }

        public string RightAddress
        {
            get => _rightAddress;
            set
            {
                _rightAddress = value;
                NotifyOfPropertyChange(() => RightAddress);
            }
        }

        public PageSize PageSize
        {
            get => _pageSize;
            private set
            {
                _pageSize = value;
                NotifyOfPropertyChange(() => PageSize);
                SetWidthAndHeight();
            }
        }

        public Double Addres1Width
        {
            get => _addres1Width;
            set
            {
                _addres1Width = value;
                NotifyOfPropertyChange(() => Addres1Width);
            }
        }

        public Double Addres2Width
        {
            get => _addres2Width;
            set
            {
                _addres2Width = value;
                NotifyOfPropertyChange(() => Addres2Width);
            }
        }

        public double InvoiceColumnWidth
        {
            get => _invoiceColumnWidth;
            set
            {
                _invoiceColumnWidth = value;
                NotifyOfPropertyChange(() => InvoiceColumnWidth);
            }
        }

        public Visibility AddLogo
        {
            get => _addLogo;
            set
            {
                _addLogo = value;
                NotifyOfPropertyChange(() => AddLogo);
            }
        }

        public double LogoWidth
        {
            get => _logoWidth;
            set
            {
                _logoWidth = value;
                NotifyOfPropertyChange(() => LogoWidth);
            }
        }

        public double LogoHeight
        {
            get => _logoHeight;
            set
            {
                _logoHeight = value;
                NotifyOfPropertyChange(() => LogoHeight);
            }
        }

        public void Handle(ColorSelected message)
        {
            Brush brush = new SolidColorBrush(message.Color);
            switch (message.Index)
            {
                case 1:
                    BackColor = brush;
                    break;
                case 2:
                    TextColor = brush;
                    break;
            }

        }

        public void Handle(PageOrientationChanged message)
        {
            PageOrientation = message.Orientation;
        }

        public void Handle(PageSizeChanged message)
        {
            PageSize = message.PageSize;
        }


        private void SetWidthAndHeight()
        {
            double x = 0, y = 0;
            switch (PageSize)
            {
                case PageSize.A4:
                    x = 210;
                    y = 297;
                    break;
                case PageSize.Letter:
                    x = 216;
                    y = 279;
                    break;
                case PageSize.Legal:
                    x = 216;
                    y = 356;
                    break;
            }
            switch (PageOrientation)
            {
                case PageOrientation.Landscape:
                    PageHeight = x;
                    PageWidth = y;
                    break;
                case PageOrientation.Portrait:
                    PageHeight = y;
                    PageWidth = x;
                    break;
            }
        }


        public void Handle(TitleChanged message)
        {
            Title = message.Title;
        }

        public void Handle(CompanyOrientationChanged message)
        {
            if (message.Orientation == PositionOption.Right)
            {
                LeftAddress = "Campany address";
                RightAddress = "Client address";
            }
            else
            {
                LeftAddress = "Client address";
                RightAddress = "Campany address";
            }
        }

        public void Handle(LogoChanged message)
        {
            AddLogo = message.AddLogo;
            LogoHeight = message.Height/ 3.779528*2;
            LogoWidth = message.Width/ 3.779528*2;
        }

    }
}
