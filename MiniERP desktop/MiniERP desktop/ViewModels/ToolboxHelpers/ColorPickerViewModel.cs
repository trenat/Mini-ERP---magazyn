using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Caliburn.Micro;
using MiniERP_desktop.Services.Events;

namespace MiniERP_desktop.ViewModels.ToolboxHelpers
{
    public class ColorPickerViewModel:Screen
    {
        private Color _rgb;

        private IEventAggregator _eventAggregator;
        private DispatcherTimer tim;
        private bool _newValue;

        public ColorPickerViewModel(IEventAggregator eventAggregator, Color color, int index)
        {
            _eventAggregator = eventAggregator;
            Index = index;
            tim = new DispatcherTimer();
            tim.Interval = TimeSpan.FromMilliseconds(1000);
            tim.Tick += Tim_Tick;
            RGB = color;
        }

        private void Tim_Tick(object sender, EventArgs e)
        {
            _eventAggregator.BeginPublishOnUIThread(new ColorSelected() { Color = RGB, Index = this.Index });
            ((DispatcherTimer)sender).Stop();
        }

        public Color RGB
        {
            get => _rgb;
            set
            {
                _rgb = value;
                _newValue = true;
                if(!tim.IsEnabled)
                    tim.Start();
                NotifyOfPropertyChange(()=> RGB);
            }
        }
        
        public int Index { set; get; }

        
    }
}
