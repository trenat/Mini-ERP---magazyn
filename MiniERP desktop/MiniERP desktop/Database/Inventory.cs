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
    
    public partial class Inventory
    {
        public int ID { get; set; }
        public Nullable<int> ItemID { get; set; }
        public Nullable<int> ActAmount { get; set; }
        public Nullable<int> RecomendedAmount { get; set; }
        public Nullable<int> MinAmount { get; set; }
        public Nullable<int> BuildID { get; set; }
        public Nullable<int> CordinateID { get; set; }
    
        public virtual Build Build { get; set; }
        public virtual Cordinate Cordinate { get; set; }
        public virtual Item Item { get; set; }
    }
}
