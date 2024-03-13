using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace Playerok.Classes
{
    public class GameExtended
    {
        public Model.Games Game { get; set; }

        public string GameImage
        {
            get
            {
                string temp;
                if (!String.IsNullOrEmpty(this.Game.Image))
                {
                    temp = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources") +"\\" + this.Game.Image; // Используем относительный путь
                }
                else
                {
                    temp = "/Resources/picture.png"; // Используем относительный путь
                }

                return temp;
            }
        }
        public string Name
        {
            get { return this.Game.Name; }
        }
        public string Description
        {
            get { return this.Game.Description; }
        }
        public string Price
        {
            get {
                if (this.Game.Price == 0 || this.Game.Price == null)
                {
                    return "FREE";
                }

                else { return this.Game.Price.ToString(); }
            }
        }
        public string Genre
        {
            get { return this.Game.Genres.Genre; }
        }
        public string Discount
        {
            get { return this.Game.Discount.ToString(); }
        }
        public string Developer
        {
            get { return this.Game.Developer; }
        }
        public string ReleaseDate
        {
            get { return this.Game.Release_date.ToString(); }
        }

        public string ID
        {
            get { return this.Game.ID_game.ToString(); }
        }
        public string Image
        {
            get { return this.Game.Image.ToString(); }
        }

        public double GameDiscountCost
        {
          
            get
            {
                double discountAmount = (double)this.Game.Discount * (Game.Price / 100.0);
                double priceWithDiscount = (double)Game.Price - discountAmount;
                return priceWithDiscount;

            }
            set { this.GameDiscountCost = value; }
        }

        public Visibility GameCostDiscountVisibility
        {
            get
            {
                Visibility result = Visibility.Collapsed;
                if (Game.Discount > 0)
                {
                    result = Visibility.Visible;
                }
                return result;
            }
        }
        public SolidColorBrush ColorFocused
        {
            get
            {
                SolidColorBrush result = new SolidColorBrush();
                //result.Color = Color.FromArgb(255, 255, 255, 0);
                if (Game.Discount > 15)
                {
                    result.Color = Color.FromArgb(255, 162, 215, 41);
                }
                return result;
            }
        }
    }
}
