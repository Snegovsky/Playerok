using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playerok.Classes
{
    internal class OrderExtended
    {
        //Данные о заказе
        public Model.Orders Order { get; set; }
        //Сумаа всего заказа
        public double SumOrder { get; set; }
        //Сумма скидки всего заказа
        public double SumOrderWithDiscount { get; set; }
        //Общая скидка в процентах
        public double SumDiscountPercent
        {
            get
            {
                return 100 - (SumOrderWithDiscount * 100 / SumOrder);
            }
        }
        //Метод расчёта общей суммы заказа
        public double CalcSumOrder(List<Model.OrdersGame> orderGame)
        {
            double sum = 0;
            foreach (Model.OrdersGame i in orderGame)
            {
                if (i.ID_order == Order.ID_order)
                {
                    double temp = i.Games.Price;
                    sum += temp * i.Count;
                }
            }
            return sum;
        }

        public double CalcSumOrderWithDiscount(List<Model.OrdersGame> orderGame)
        {
            double sum = 0;
            foreach (Model.OrdersGame i in orderGame)
            {
                if (i.ID_order == Order.ID_order)
                {
                    double temp = (double)(i.Games.Price - i.Games.Price * i.Games.Discount / 100);
                    sum += temp * i.Count;
                }
            }
            return sum;
        }
    }
}
