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
    
    public partial class clients
    {
        public clients()
        {
            this.orders = new HashSet<orders>();
        }
    
        public int client_id { get; set; }
        public Nullable<int> user_id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string patronymic { get; set; }
        public string contact_info { get; set; }
    
        public virtual users users { get; set; }
        public virtual ICollection<orders> orders { get; set; }
    }
}