//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Playerok.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders()
        {
            this.OrdersGame = new HashSet<OrdersGame>();
        }
    
        public int ID_order { get; set; }
        public int Code { get; set; }
        public int ID_user { get; set; }
        public System.DateTime Date { get; set; }
        public int ID_status { get; set; }
        public string Comment { get; set; }
        public int Price { get; set; }
    
        public virtual Statuses Statuses { get; set; }
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrdersGame> OrdersGame { get; set; }
    }
}
