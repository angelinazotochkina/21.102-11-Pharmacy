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
    
    public partial class supplierdeliveries
    {
        public int delivery_id { get; set; }
        public int product_id { get; set; }
        public int quantity { get; set; }
    
        public virtual deliveries deliveries { get; set; }
        public virtual pharmacyproducts pharmacyproducts { get; set; }
    }
}
