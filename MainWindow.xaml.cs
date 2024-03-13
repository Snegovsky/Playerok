using EasyCaptcha.Wpf;
using Playerok.Classes;
using Playerok.Model;
using Playerok.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Net.Mail;

namespace Playerok
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        System.Windows.Threading.DispatcherTimer timer;
        string answer;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Подключение к БД
            Classes.Helper.DB = new Model.BDPlayerokEntities();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            
            answer = captcha.CaptchaText;
            Password.Text = PasswordDot.Password;
            string login = Login.Text;
            //string password = Password.Text;
            string password = PasswordDot.Password;
            StringBuilder sb = new StringBuilder();
            //Обработка пустоты
            if (login == "")
            {
                sb.Append("Вы не введи логин.\n");
            }
            if (password == "")
            {
                sb.Append("Вы не ввели пароль.\n");
            }
            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString());
                return;
            }
            ////Работа с БД
            //List<Model.User> users = new List<Model.User>();
            ////Все записи БД
            //users = Classes.Helper.DB.Users.ToList();
            ////Получить первую запись таблицы
            //Model.User user = users.Where(u => u.UserLogin == login && u.UserPassword == password).FirstOrDefault();
            Classes.Helper.User = Classes.Helper.DB.Users.ToList().Where(u => u.Login == login && u.Password == password).FirstOrDefault();
            if (Helper.User != null)
            {
                string userName = Helper.User.Full_name;
                int userRoleId = Helper.User.ID_role;
                string userRole = Helper.User.Roles.role;
                MessageBox.Show("Здравствуйте, " + userName + "\n" + "Ваша роль: " + "\n" + userRole);
                GoToCatalog();
            }
            else if (captcha.IsVisible)
            {
                if (tbCaptcha.Text == captcha.CaptchaText && Helper.User != null)
                {
                    //string userName = Helper.User.UserFullName;
                    //int userRoleId = Helper.User.UserRole;
                    //string userRoleName = Helper.User.Role.RoleName;
                    GoToCatalog();
                }
                else
                {
                    MessageBox.Show("Вы заблокированы на 10 секунд!");
                    Start.IsEnabled = false;
                    captcha.CreateCaptcha(EasyCaptcha.Wpf.Captcha.LetterOption.Alphanumeric, 4);
                    timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Tick += new EventHandler(timerTick);
                    timer.Interval = new TimeSpan(0, 0, 10);
                    timer.Start();
                }
            }
            else
            {
                MessageBox.Show("Вы не зарегистрированны в системе.\nПопробуйте ещё раз.");
                tbCaptcha.Visibility = Visibility.Visible;
                captcha.Visibility = Visibility.Visible;
                captcha.CreateCaptcha(EasyCaptcha.Wpf.Captcha.LetterOption.Alphanumeric, 4);
            }
            //Доступ по навигационному свуйству в полю связвнной таблицы
            //string userRoleName = user.Role.RoleName;
            //MessageBox.Show(userName + " " + userRoleId + " " + userRoleName);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
        //При отображении пароля
        private void isVisiblePassword_Checked(object sender, RoutedEventArgs e)
        {
            Password.Visibility = Visibility.Visible;
            PasswordDot.Visibility = Visibility.Hidden;
            Password.Text = PasswordDot.Password;
        }

        private void isVisiblePassword_Unchecked(object sender, RoutedEventArgs e)
        {
            Password.Visibility = Visibility.Hidden;
            PasswordDot.Visibility = Visibility.Visible;
            PasswordDot.Password = Password.Text;
        }
        //Начальные настройки при отображении окна
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            tbCaptcha.Visibility = Visibility.Hidden;
            captcha.Visibility = Visibility.Hidden;
        }

        private async void timerTick(object sender, EventArgs e)
        {
            Start.IsEnabled = true;
        }
        //Переход в каталог Гостем
        private void Gost_Click(object sender, RoutedEventArgs e)
        {
            GoToCatalog();
        }


        //Метод перехода в каталог
        private void GoToCatalog()
        {
            Catalog catalog = new Catalog();
            this.Close();
            catalog.Show();
            
        }
    }
}
