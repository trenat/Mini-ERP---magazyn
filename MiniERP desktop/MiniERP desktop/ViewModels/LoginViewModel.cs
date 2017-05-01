using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;

namespace MiniERP_desktop.ViewModels
{
    public class LoginViewModel:Screen
    {

        protected override void OnInitialize()
        {
            base.OnInitialize();
            this.DisplayName = "Storage ERP";
        }

        public void Login()
        {
            MessageBox.Show("Niezła próba xD");
        }
    }
}
