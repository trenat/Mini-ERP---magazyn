using System.Windows.Media;
using Caliburn.Micro;
using MiniERP_desktop.Services.Events;

namespace MiniERP_desktop.ViewModels
{
    public class OverviewViewModel:Screen, IHandle<ColorSelected>
    {
        private Brush _textColor;
        private Brush _backColor;
        private IEventAggregator _eventAggregator;

        public OverviewViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            TextColor = new SolidColorBrush(Color.FromRgb(0, 0, 0));
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
