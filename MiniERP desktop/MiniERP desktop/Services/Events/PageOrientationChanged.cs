using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MiniERP_desktop.Models;
using PdfSharp;

namespace MiniERP_desktop.Services.Events
{
    public class PageOrientationChanged
    {
        public PageOrientation Orientation { set; get; }
    }
}
