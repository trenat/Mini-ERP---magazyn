//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MiniERP_desktop.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class TotalRow
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Value { get; set; }
        public Nullable<bool> Inverse { get; set; }
        public Nullable<int> InvoiceID { get; set; }
    
        public virtual Invoice Invoice { get; set; }
    }
}
