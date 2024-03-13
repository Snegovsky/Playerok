using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playerok.Classes
{
    public class GameInOrder
    {
        public GameExtended GameExtended { get; set; }

        //Количество этого товара в заказе
        public int CountGameInOrder { get; set; }
        //Конструктор c параметром - выбраный товар из каталога
        public GameInOrder(GameExtended gameExtended)
        {
            //ProductInOrder productInOrder = new ProductInOrder(productExtended);
            GameExtended = gameExtended;
            CountGameInOrder = 1;
        }
        //Конструктор по умолчанию
        public GameInOrder() { }
    }
}
