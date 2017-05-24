using System.Windows;
using Caliburn.Micro;
using MiniERP_desktop.Database;
using MiniERP_desktop.Services.Events;
using MiniERP_desktop.Services.

    s;
using MiniERP_desktop.ViewModels.ToolboxHelpers;

namespace MiniERP_desktop.ViewModels
{
    public class ReportsViewModel : Conductor<Screen>.Collection.OneActive, IHandle<HelperScreen>
    {
        private BindableCollection<string> _documentType;
        private object _content;
        private object _helperContent;
        private object _overviewContent;
        private string _selectedDocumentType;
        private IEventAggregator _eventAggregator;
        private ERPEntities _dbContext;

        public BindableCollection<string> DocumentType
        {
            set => _documentType = value;
            get => _documentType;
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


        public object HelperContent
        {
            set
            {
                _helperContent = value;
                NotifyOfPropertyChange(() => HelperContent);
            }
            get => _helperContent;
        }

        public object OverviewContent
        {
            set
            {
                _overviewContent = value;
                NotifyOfPropertyChange(() => OverviewContent);
            }
            get => _overviewContent;
        }

        public string SelectedDocumentType
        {
            set
            {
                _selectedDocumentType = value;
                NotifyOfPropertyChange(() => SelectedDocumentType);
                TypeChanged(_selectedDocumentType);
            }
            get => _selectedDocumentType;
        }

        public ReportsViewModel(IEventAggregator eventAggregator, ERPEntities dbContext)
        {
            DocumentType = new BindableCollection<string>();
            DocumentType.Add("VAT Invoice");
            DocumentType.Add("Good Issued Notes");
            DocumentType.Add("Good Recieved Notes");
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _dbContext = dbContext;
        }

        public void Handle(IScreen e)
        {
            HelperContent = e;
        }

        public void TypeChanged(string type)
        {
            Screen screen = null;
            string title = type;
            switch (type)
            {
                case "VAT Invoice":
                    title = "Invoice VAT";
                    screen = new InvoiceViewModel(_eventAggregator, _dbContext); break;
                case "Good Issued Notes": break;
                case "Good Recieved Notes;": break;
            }
            Content = screen;
            OverviewContent = new OverviewViewModel(_eventAggregator, title);

        }

        public void Generate()
        {
            _eventAggregator.PublishOnUIThread(new ConvertPdf());
        }


        public void Handle(HelperScreen message)
        {
            HelperContent = message.Screen;
        }
    }
}
