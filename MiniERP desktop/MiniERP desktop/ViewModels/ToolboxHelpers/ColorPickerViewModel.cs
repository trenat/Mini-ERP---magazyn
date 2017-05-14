using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using MiniERP_desktop.Services.Events;

namespace MiniERP_desktop.ViewModels.ToolboxHelpers
{
    public class ColorPickerViewModel:Screen
    {
        private Image _palete;
        private Color _rgb;
        private string _r;
        private string _g;
        private string _b;

        private IEventAggregator _eventAggregator;

        public ColorPickerViewModel(IEventAggregator eventAggregator, Color color, int index)
        {
            _eventAggregator = eventAggregator;
            Index = index;
            RGB = color;
        }

        public Image Palete
        {
            get => _palete;
            set
            {
                _palete = value;
                NotifyOfPropertyChange(() => Palete);
            }
        }

        public Color RGB
        {
            get => _rgb;
            set
            {
                _rgb = value;
                NotifyOfPropertyChange(()=> RGB);
                _eventAggregator.PublishOnUIThread(new ColorSelected(){Color = RGB, Index = this.Index});
            }
        }
        
        public int Index { set; get; }
        //public string R
        //{
        //    get => _r;
        //    set
        //    {
        //        _r = value;
        //        NotifyOfPropertyChange(() => R);
        //    }
        //}
        //public string B
        //{
        //    get => _b;
        //    set
        //    {
        //        _b = value;
        //        NotifyOfPropertyChange(() => B);
        //    }
        //}
        //public string G
        //{
        //    get => _g;
        //    set
        //    {
        //        _g = value;
        //        NotifyOfPropertyChange(() => G);
        //    }
        //}


        
    }
}
