using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace GAMES
{
    public partial class Form3 : Form
    {

        Point EmptyPoint;
        ArrayList images = new ArrayList();

        public Form3()
        {
            InitializeComponent();
            EmptyPoint.X = 180;
            EmptyPoint.Y = 180;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            foreach (Button b in panel1.Controls) /*активируем все кнопки на панели(делаем их доступными для нажатия).*/
                b.Enabled = true;

           // Image orginal = Image.FromFile(@"img\img.jpg");

            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "img.jpg");
            Image orginal = Image.FromFile(@"img\img.jpg"); // Загружаем изображение
       

            cropImageTomages(orginal, 270, 270); /*разрезаем изображение на 8 кусочков(270x270 — размер изображения).*/

            AddImagesToButtons(images);/*добавляем разрезанные кусочки изображения на кнопки.*/
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //        Добавление изображений на кнопки

        private void AddImagesToButtons(ArrayList images)
        {
            int i = 0;
            int[] arr = { 0, 1, 2, 3, 4, 5, 6, 7 };/* массив индексов кусочков изображения*/

            arr = suffle(arr); /*перемешиваем индексы, чтобы части изображения оказались в случайном порядке.*/
            //Присваиваем кнопкам кусочки изображения в соответствии с перемешанным массивом.
            foreach (Button b in panel1.Controls)
            {
                if (i < arr.Length)
                {
                    b.Image = (Image)images[arr[i]];
                    i++;
                }
            }
        }
        //Перемешивание индексов
        private int[] suffle(int[] arr)
        {
            Random rand = new Random();
            arr = arr.OrderBy(x => rand.Next()).ToArray(); /*сортируем массив в случайном порядке с помощью генератора случайных чисел.*/
            return arr;
        }
        //        Разрезание изображения

        private void cropImageTomages(Image orginal, int w, int h)
        {
            Bitmap bmp = new Bitmap(w, h); /*создаем пустое изображение */

            Graphics graphic = Graphics.FromImage(bmp);

            graphic.DrawImage(orginal, 0, 0, w, h); /*копируем исходное изображение на созданный холст.*/

            graphic.Dispose();

            int movr = 0, movd = 0; /*сдвиги по горизонтали и вертикали для вырезания следующего кусочка.*/

            for (int x = 0; x < 8; x++)//Разрезаем изображение на кусочки 90x90 пикселей.
            {
                Bitmap piece = new Bitmap(90, 90);

                for (int i = 0; i < 90; i++)
                    for (int j = 0; j < 90; j++)
                        piece.SetPixel(i, j,
                            bmp.GetPixel(i + movr, j + movd));

                images.Add(piece); /*добавляем кусочек в список.*/

                movr += 90;

                if (movr == 270)
                {
                    movr = 0;
                    movd += 90;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MoveButton((Button)sender);
        }

        private void MoveButton(Button btn)
        {
            if (((btn.Location.X == EmptyPoint.X - 90 || btn.Location.X == EmptyPoint.X + 90)
                && btn.Location.Y == EmptyPoint.Y)
                || (btn.Location.Y == EmptyPoint.Y - 90 || btn.Location.Y == EmptyPoint.Y + 90)
                && btn.Location.X == EmptyPoint.X)
            {//Обмен местами:
                Point swap = btn.Location;
                btn.Location = EmptyPoint;
                EmptyPoint = swap;
            }
            //Меняем местами координаты кнопки и пустой ячейки.
            if (EmptyPoint.X == 180 && EmptyPoint.Y == 180)
                CheckValid(); /*проверяем, правильно ли собран пазл, если пустая ячейка вернулась в исходное положение.*/
        }
        //        Проверка собранности пазла

        private void CheckValid()
        {
            int count = 0, index;
            foreach (Button btn in panel1.Controls)
            {
                index = (btn.Location.Y / 90) * 3 + btn.Location.X / 90;//Вычисляем индекс кусочка в соответствии с его текущим положением на панели.
                if (images[index] == btn.Image)//Проверяем, соответствует ли изображение на кнопке правильному кусочку.
                    count++;
            }
            if (count == 8)//Если все 8 кусочков стоят на своих местах, игрок побеждает.
                MessageBox.Show("Победа!");
        }
    }
}
