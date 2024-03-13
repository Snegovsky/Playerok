using Playerok.Classes;
using Playerok.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Playerok.View
{
    /// <summary>
    /// Логика взаимодействия для CreateOrder.xaml
    /// </summary>
    public partial class CreateOrder : Window
    {
        List<GameInOrder> listOrder;
        int orderCode = 0;
        double sumTotalWithDiscount = 0;
        public CreateOrder(List<GameInOrder> listOrder2)
        {
            InitializeComponent();
            this.listOrder = listOrder2;
            System.Diagnostics.Debug.WriteLine("Number of items in listOrder: " + listOrder.Count);
            ShowInfo();
        }



        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            Catalog catalog = new Catalog();    
            this.Close();
            catalog.Show();
        }

        
        private void ShowInfo()
        {
           
            if (listOrder != null)
            {
                listBoxGames.ItemsSource = listOrder;
                double sumTotal = 0;
                
                
                foreach (var item in listOrder)
                {
                    sumTotal += item.GameExtended.Game.Price * item.CountGameInOrder;
                    sumTotalWithDiscount += item.GameExtended.GameDiscountCost * item.CountGameInOrder;
                    
                }
                tbSumTotal.Text = "Сумма заказа: " + sumTotal.ToString();
                tbSumDiscount.Text = "Cумма скидки: " + Convert.ToString(sumTotal - sumTotalWithDiscount);
                tbSumTotalWithDiscount.Text = "Сумма со скидкой: " + sumTotalWithDiscount.ToString();
                
                
            }
            
        }
        private void buttonDeleteProductInOrder_Click(object sender, RoutedEventArgs e)
        {

            string art = (sender as Button).Tag as string;
            GameInOrder selectGame = listOrder.Find(pr => pr.GameExtended.Game.ID_game.ToString() == art);
            //ProductInOrder selectProduct = (sender as Button).DataContext as ProductInOrder;
            listOrder.Remove(selectGame);
            ShowInfo();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Classes.Helper.DB = new Model.BDPlayerokEntities();
            this.listOrder = listOrder;
            ShowInfo();
            Random random = new Random();
            orderCode = random.Next(100, 1000); // Генерация числа от 100 до 999
            tbDate.Text += DateTime.Now.Date.ToString("dd.MM.yyyy");
            tbCode.Text += orderCode.ToString();
        }

        private void btnMakeOrder_Click(object sender, RoutedEventArgs e)
        {
            if (listOrder.Count <= 0)
            {
                MessageBox.Show("Корзина пуста!");
                return;
            }
            else
            {
                
                try
                {
                    //Создаём оьъект заказ
                    Model.Orders Order = new Model.Orders();
                    //Запполняем поля
                    Order.Date = DateTime.Now;
                    Order.ID_user = Helper.User.ID_user;
                    Order.Code = orderCode;
                    Order.ID_status = 6;
                    Order.Price = (int)sumTotalWithDiscount;
                    Helper.DB.Orders.Add(Order);
                    Helper.DB.SaveChanges();
                    foreach (Classes.GameInOrder item in listOrder)
                    {
                        Model.OrdersGame orderGame = new Model.OrdersGame();
                        orderGame.ID_order = Order.ID_order;
                        orderGame.ID_game = item.GameExtended.Game.ID_game;
                        orderGame.Count = item.CountGameInOrder;
                        Helper.DB.OrdersGame.Add(orderGame);
                        Helper.DB.SaveChanges();
                    }
                    MessageBox.Show("Заказ оформлен");
                    listOrder.Clear();
                    Catalog catalog = new Catalog();
                    this.Close();
                    catalog.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Произошёл сбой при сохранении: {ex.Message}");
                }
            }
        }
    }
}
