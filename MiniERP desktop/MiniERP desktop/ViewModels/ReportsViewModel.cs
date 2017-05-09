using System.Windows;
using Caliburn.Micro;

namespace MiniERP_desktop.ViewModels
{
    public class ReportsViewModel: Conductor<Screen>.Collection.OneActive
    {
        private BindableCollection<string> _documentType;
        private object _content;
        private string _selectedDocumentType;

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

        public string SelectedDocumentType
        {
            set
            {
                _selectedDocumentType = value;
                NotifyOfPropertyChange(()=> SelectedDocumentType);
                TypeChanged(_selectedDocumentType);
            }
            get => _selectedDocumentType;
        }

        public ReportsViewModel()
        {
            DocumentType = new BindableCollection<string>();
            DocumentType.Add("VAT Invoice");
            DocumentType.Add("Good Issued Notes");
            DocumentType.Add("Good Recieved Notes");
        }

        public void TypeChanged(string type)
        {
            Screen screen = null;
            switch (type)
            {
                case "VAT Invoice": screen = new InvoiceViewModel(); break;
                case "Good Issued Notes":; break;
                case "Good Recieved Notes;": break;
            }
            Content = screen;
        }

        public void Invoice() // VAT 
        {

            Content = new InvoiceViewModel();
        }

        public void GIN() //(RW - rozchód wewnętrzny) Goods issued notes
        {
            
        }

        public void GRN() //(PW - przyjęcie wewnętrzne) Goods Recived Notes
        {
            
        }

    }
}
