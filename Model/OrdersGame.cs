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
    
    public partial class OrdersGame
    {
        public int ID_order { get; set; }
        public int ID_game { get; set; }
        public int Count { get; set; }
    
        public virtual Games Games { get; set; }
        public virtual Orders Orders { get; set; }
    }
}