using Playerok.Classes;
using Playerok.Model;
using Playerok.View;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {
        int filterCategory; 		//Фильтр по скидке и категории
        int countAll;			//Количество всех записей и после фильтрации
        List<GameInOrder> listOrder = new List<GameInOrder>(); 
        public Catalog()
        {
            InitializeComponent();
            DataContext = new GameExtended();
        }
        //Возврат на авторизацию
        private void buttonNavigate_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Загрузка окна - отображение товаров

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Информация о пользователе
            if (Helper.User != null)//Зарегистрированный пользователь
            {
                tbFIO.Text = Helper.User.Full_name;
               
            }
            else
            {
                tbFIO.Text = "Гость";
            }

            List<Genres> genres = Helper.DB.Genres.ToList();

            Genres allgenres = new Genres();
            allgenres.ID_genre = 0;
            allgenres.Genre = "Все жанры";
            genres.Insert(0, allgenres);
            cbGenre.ItemsSource = genres;

            cbGenre.DisplayMemberPath = "Genre";
            cbGenre.SelectedValuePath = "ID_genre";
            cbGenre.SelectedIndex = 0;
            cbGenre.SelectedValue = 0;
            rbSortAsc.IsChecked = true;


            //Первый отображается из списка
            //cbSort.SelectedIndex = 0;
            cbDiscount.SelectedIndex = 0;
            
            ShowProduct();
            
            buttonOrder.Visibility = Visibility.Hidden;
            buttonGameAdd.Visibility = Visibility.Hidden;

            if (Helper.User != null)
            {
                if (Helper.User.ID_role == 1)
                {
                    buttonGameAdd.Visibility = Visibility.Visible;
                }
            }
           

        }

        private void rbSortAsc_Checked(object sender, RoutedEventArgs e)
        {
            ShowProduct();
        }
        private void rbSortDesc_Checked(object sender, RoutedEventArgs e)
        {
            ShowProduct();
        }
        private void cbDiscount_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ShowProduct();
        }
        private void cbCategory_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ShowProduct();
        }
        private void tbSearch_TextChanged(object sender, RoutedEventArgs e)
        {
            ShowProduct();
        }

        private void buttonGameAdd_Click(object sender, RoutedEventArgs e)
        {
            AddGame addGame = new AddGame(null, false);
            addGame.Show();
            this.Close();
        }

        /// Получить данные из БД по условия фильтра и отобразить их
        private void ShowProduct()
        {
            List<Model.Games> games = new List<Model.Games>();
            games = Helper.DB.Games.ToList();
            listBoxProducts.ItemsSource = games;
            List<Classes.GameExtended> gameExtendeds = new List<Classes.GameExtended>();

            double maxDiscount = 100;
            double minDiscount = 0;
            switch (cbDiscount.SelectedIndex)
            {
               
                case 1:
                    maxDiscount = 9.99;
                    break;
                case 2:
                    minDiscount = 10;
                    maxDiscount = 14.99;
                    break;
                case 3:
                    minDiscount = 15;
                    break;
            }

            games = games.Where(x => x.Discount <= maxDiscount &&
                                                        x.Discount >= minDiscount).ToList();
            //Общее количество товаров в таблице товаров
            countAll = games.Count();
            

            //Сортировка от состояния радиокнопки
            if ((bool)rbSortAsc.IsChecked)		//По возрастанию
            {
                games = games.OrderBy(pr => pr.Price).ToList();
            }
            if ((bool)rbSortDesc.IsChecked)
            {
                games = games.OrderByDescending(pr => pr.Price).ToList();
            }

            if (filterCategory != 0)
            {
                games = games.Where(x => x.ID_genre == filterCategory).ToList();
            }
            if (cbGenre.SelectedIndex > 0)
            {
                games    = games.Where(x => x.ID_genre ==
                                                             cbGenre.SelectedIndex).ToList();
            }

            //Поиск по названию
            string search = tbSearch.Text;
            search = search.ToLower();
            if (!string.IsNullOrEmpty(search))
            {
                games = games.Where(pr => pr.Name.ToLower().Contains(search)).ToList();

            }
            
            //Количество товаров после фильтрации
            int filterCount = games.Count;
            tbCount.Text = filterCount + " Из " + countAll;


            
            foreach (Model.Games i in games)
            {
                Classes.GameExtended gameExtended = new Classes.GameExtended();
                gameExtended.Game = i;
                gameExtendeds.Add(gameExtended);
            }

            listBoxProducts.ItemsSource = gameExtendeds;
        }

       
        
        private void cmAddInOrder_Click(object sender, RoutedEventArgs e)
        {
            if (Helper.User != null)
            {
                if (Helper.User.ID_role == 2 || Helper.User.ID_role == 1)
                {
                    buttonOrder.Visibility = Visibility.Visible;
                    GameExtended selectedGame = listBoxProducts.SelectedItem as GameExtended;
                    if (selectedGame != null)
                    {
                        Classes.GameExtended gameExtended = new Classes.GameExtended();
                        gameExtended = selectedGame;
                        GameInOrder gameInOrder = new GameInOrder();
                        gameInOrder.GameExtended = gameExtended;
                        gameInOrder.CountGameInOrder = 1; // Установите начальное количество
                        listOrder.Add(gameInOrder);
                    }
                }
            }
            
        }

        private void buttonOrder_Click(object sender, RoutedEventArgs e)
        {

            CreateOrder createOrder = new CreateOrder(listOrder);
            createOrder.Show();
            this.Close();


        }
        private void listBoxProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            if (Helper.User != null)
            {
                if (Helper.User.ID_role == 1)
                {
                    // Получаем выбранный элемент списка
                    GameExtended selectedGame = listBoxProducts.SelectedItem as GameExtended;

                    if (selectedGame != null)
                    {
                        // Создаем и открываем окно AddGame, передавая выбранный элемент в конструктор
                        AddGame addGame = new AddGame(selectedGame, true);
                        addGame.Show();
                        this.Close();
                    }
                }
            }
           
        }
    }
}
