using Microsoft.Win32;
using Playerok.Classes;
using Playerok.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Runtime.Remoting.Lifetime;
using Playerok.View;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using System.Diagnostics.Eventing.Reader;
using System.Data.Entity;

namespace Playerok.View
{
    /// <summary>
    /// Логика взаимодействия для AddGame.xaml
    /// </summary>
    public partial class AddGame : Window
    {
        public bool isEditingMode;
        public GameExtended selectedGame;
        public AddGame(GameExtended game, bool isEditing)
        {
            InitializeComponent();
            selectedGame = game;
            isEditingMode = isEditing;
        }

        public void Save_Click(object sender, RoutedEventArgs e)
        {
            //Получение данных        
            string strNameGame = tbName.Text; //Название игры
            string strGenre = cbGenres.Text;
            string strPrice = tbPrice.Text;
            string strDiscount = tbDiscount.Text;
            string strDeveloper = tbDeveloper.Text;
            string strReleaseDate = dpDateRelease.Text;
            string strDescription = tbDescription.Text;

            // Далее можно использовать selectedContent по своему усмотрению

            int selectedGenre = 1+cbGenres.SelectedIndex;

            //Изображение 
            // Получение текущего изображения из элемента Image
            string fileName;
            string fileName1 = null;
            

                if (int.Parse(strDiscount) > 30)
            {

                System.Windows.MessageBox.Show("Скидка больше 30 процентов!");
            }
            else
            {
                //Исключение если пустые поля
                if (strNameGame != "" && strGenre != "" && strDeveloper != "" && strReleaseDate != "" && strPrice != "")
                {
                    var image = SelectedPhoto.Source as BitmapSource;


                    fileName = null;
                    // Проверка, что изображение не равно null
                    if (image != null)
                    {
                        // Генерация уникального имени файла
                        string uniqName = strNameGame.Replace(" ", "");
                        fileName = $"{uniqName.ToString()}.jpg"; //название файла с фото

                        //Путь файла
                        string folderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources");

                        // Полный путь к сохраняемому файлу
                        string savePath = System.IO.Path.Combine(folderPath, fileName);

                        // Создание кодировщика JPEG для сохранения изображения
                        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(image));

                        // Удаление старого файла изображения, если он существует
                        if (File.Exists(savePath))
                        {
                            File.Delete(savePath);
                        }

                        // Сохранение файла на диск
                        using (var fileStream = new FileStream(savePath, FileMode.Create))
                        {
                            encoder.Save(fileStream);
                        }
                        if (fileName != null)
                        {
                            fileName1 = fileName;
                        }
                    }
                    else
                    {
                        fileName1 = "NULL";
                    }
                    //Наш товар
                    System.Windows.MessageBox.Show("Название игры:" + strNameGame.ToString() + "\n" +
                                    "Жанр: " + strGenre.ToString() + "\n" +
                                    "Цена: " + strPrice.ToString() + "\n" +
                                    "Скидка: " + strDiscount.ToString() + "\n" +
                                    "Разработчик: " + strDeveloper.ToString() + "\n" +
                                    "Дата релиза: " + strReleaseDate.ToString() + "\n" +
                                    "Описание товара: " + strDescription.ToString() + "\n" +
                                    "Изображение: " + fileName1.ToString() + "\n"
                        );

                    //Сохранение в базу данных

                    if (isEditingMode)
                    {

                        int gameId = int.Parse(selectedGame.ID);
                        var existingGame = Helper.DB.Games.Find(gameId);

                        if (existingGame != null)
                        {
                            // Обновляем только нужные поля
                            existingGame.Name = strNameGame;
                            existingGame.ID_genre = cbGenres.SelectedIndex;
                            existingGame.Price = int.Parse(strPrice);
                            existingGame.Discount = int.Parse(strDiscount);
                            existingGame.Developer = strDeveloper;
                            existingGame.Release_date = DateTime.Parse(strReleaseDate);
                            existingGame.Image = fileName;
                            existingGame.Description = strDescription;

                            // Устанавливаем состояние сущности в Modified
                            Helper.DB.Entry(existingGame).State = EntityState.Modified;

                            try
                            {
                                //Сохраняем изменения в базу
                                Helper.DB.SaveChanges();
                                System.Windows.MessageBox.Show("Товар обновлен");
                                Catalog catalog = new Catalog();
                                this.Close();
                                catalog.Show();
                            }
                            catch
                            {
                                System.Windows.MessageBox.Show("Произошёл сбой при обновлении");
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Игра не найдена в базе данных");
                        }

                    }
                    else
                    {
                        //Создаём объект товара
                        Model.Games game = new Model.Games()
                        {
                            //Запполняем поля
                            Name = strNameGame,
                            ID_genre = cbGenres.SelectedIndex,
                            Price = int.Parse(strPrice),
                            Discount = int.Parse(strDiscount),
                            Developer = strDeveloper,
                            Release_date = DateTime.Parse(strReleaseDate),
                            Image = fileName,
                            Description = strDescription
                        };

                        // Добавление нового продукта в таблицу product
                        Helper.DB.Games.Add(game);
                        try
                        {
                            //Сохранение в базу
                            Helper.DB.SaveChanges();
                            System.Windows.MessageBox.Show("Товар добавлен");
                            this.Close();
                            Catalog catalog = new Catalog();
                            catalog.Show();
                        }
                        catch
                        {
                            System.Windows.MessageBox.Show("Произошёл сбой при сохранении");
                        }
                    }

                }
                else
                {
                    System.Windows.MessageBox.Show("Вы не все заполнили");
                }
            }
           

        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Genres> genres1 = new List<Genres>();
            genres1 = Helper.DB.Genres.ToList();          
            cbGenres.ItemsSource = genres1;
            cbGenres.DisplayMemberPath = "Genre";
            cbGenres.SelectedValuePath = "ID_genre";
            cbGenres.SelectedIndex = 1;
            cbGenres.SelectedValue = 1;

            if (isEditingMode)
            {
                tbTitle.Text = "Редактирование игры";
                // Здесь можете заполнить элементы управления данными из выбранной игры для редактирования
                tbName.Text = selectedGame.Name;
                cbGenres.Text = selectedGame.Genre;
                
                tbPrice.Text = selectedGame.Price;
                tbDiscount.Text = selectedGame.Discount;
                tbDeveloper.Text = selectedGame.Developer;
                dpDateRelease.Text = selectedGame.ReleaseDate;
                tbDescription.Text = selectedGame.Description;
                SelectedPhoto.Source = new BitmapImage(new Uri(selectedGame.GameImage));
                // Заполнение остальных элементов управления
            }
        }

        public void SelectPhoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg) | *.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(openFileDialog.FileName);
                bitmap.EndInit();
                SelectedPhoto.Source = bitmap;
            }
        }

        public void tbPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$"); // Регулярное выражение для проверки числа
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true; // Если ввод не является числом, то обработка события формируется и значит ввод числа недопустим
            }
        }

        public void tbDiscount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$"); // Регулярное выражение для проверки числа
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true; // Если ввод не является числом, то обработка события формируется и значит ввод числа недопустим
            }
        }

        public void buttonBack_Click(object sender, RoutedEventArgs e)
        {
           
            Catalog catalog = new Catalog();
            catalog.Show();
            this.Close();
           
        }    
    }
}
