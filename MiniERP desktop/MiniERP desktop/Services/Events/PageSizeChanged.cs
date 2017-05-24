using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp;

namespace MiniERP_desktop.Services.Events
{
    public class PageSizeChanged
    {
        public PageSize PageSize { set; get; }
    }
}
