using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace MiniERP_desktop.ViewModels
{
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        int count = 1;

        public void OpenTab()
        {
            
            ActivateItem(new TabViewModel
            {
                DisplayName = "Tab " + count++
            });
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            this.DisplayName = "Storage ERP";
        }

        public void CloseItem(Screen screen)
        {
            DeactivateItem(screen, true);
        }
    }
}
