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
    
    public partial class orders
    {
        public orders()
        {
            this.orderdetails = new HashSet<orderdetails>();
        }
    
        public int order_id { get; set; }
        public int client_id { get; set; }
        public string status { get; set; }
        public System.DateTime delivery_date { get; set; }
        public string pickup_location { get; set; }
        public System.DateTime order_date { get; set; }
        public string order_code { get; set; }
        public Nullable<decimal> total_amount { get; set; }
    
        public virtual clients clients { get; set; }
        public virtual ICollection<orderdetails> orderdetails { get; set; }
    }
}