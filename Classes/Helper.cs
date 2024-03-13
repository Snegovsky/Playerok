using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playerok.Classes
{
    public class Helper
    {
        //Объявдление объект связи с БД
        public static Model.BDPlayerokEntities DB { get; set; }
        //
        public static Model.Users User { get; set; }

        public static Model.Games Game { get; set; }
        public static Model.Orders Order { get; set; }
        public static Model.Roles Role { get; set; }
        public static Model.Statuses Status{ get; set; }

    }
}
