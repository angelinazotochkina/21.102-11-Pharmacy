//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _21._102_11_Pharmacy.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class pharmacyproducts
    {
        public pharmacyproducts()
        {
            this.orderdetails = new HashSet<orderdetails>();
            this.supplierdeliveries = new HashSet<supplierdeliveries>();
        }
    
        public int product_id { get; set; }
        public string article_number { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string category { get; set; }
        public string image_url { get; set; }
        public string manufacturer { get; set; }
        public decimal price { get; set; }
        public Nullable<decimal> discount { get; set; }
        public Nullable<int> stock_quantity { get; set; }
        public string status { get; set; }
        public string unit_of_measurement { get; set; }
        public Nullable<int> quantity_per_pack { get; set; }
    
        public virtual ICollection<orderdetails> orderdetails { get; set; }
        public virtual ICollection<supplierdeliveries> supplierdeliveries { get; set; }
    }
}