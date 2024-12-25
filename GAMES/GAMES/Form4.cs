using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GAMES
{
    public partial class Form4 : Form
    {
        private const int CELL_SIZE_PIXELS = 30;                    // Размер клетки игрового поля, в пикселях
        private const int ROWS_NUMBER = 15;                         // Количество рядов в игровом поле
        private const int COLS_NUMBER = 15;                         // Количество столбцов в игровом поле
        private const int FIELD_LEFT_OFFSET_PIXELS = 40;            // Отступ в пикселях от левого края формы
        private const int FIELD_TOP_OFFSET_PIXELS = 15;             // Отступ в пикселях от правого края формы
        private const int INITIAL_SNAKE_SPEED_INTERVAL = 300;       // Задержка (свойство "Interval") для основного игрового таймера TimerGameLoop
        private const int SPEED_INCREMENT_BY = 5;                   // На сколько миллисекунд увеличить скорость "Змейки" при очередном поглощении змейкой "Еды"

        private enum SnakeDirection
        {
            Left,
            Right,
            Up,
            Down
        }

        private SnakeDirection snakeDirection = SnakeDirection.Up;  // текущее направление движения "Змейки"
        private LinkedList<Point> snake = new LinkedList<Point>();  // Список точек, содержащих координаты всего "тела Змейки"
        private Point food;                                         // Точка, содержащая координаты "Еды" для "Змейки"
        private Random rand = new Random();                         // генератор псевдослучайных чисел. нужен для генерации очередной "Еды" в произвольном месте игрового поля
        private bool isGameEnded;

        public Form4()
        {
            InitializeComponent();
        }

        private void InitializeSnake()
        {
            snakeDirection = SnakeDirection.Up;
            snake.Clear();
            snake.AddFirst(new Point(ROWS_NUMBER - 1, COLS_NUMBER / 2 - 1));
        }

        private void DrawGrid(Graphics g)
        {
            for (int row = 0; row <= ROWS_NUMBER; row++)
            {
                g.DrawLine(Pens.Cyan,
                    new Point(FIELD_LEFT_OFFSET_PIXELS, FIELD_TOP_OFFSET_PIXELS + row * CELL_SIZE_PIXELS),
                    new Point(FIELD_LEFT_OFFSET_PIXELS + CELL_SIZE_PIXELS * ROWS_NUMBER, FIELD_TOP_OFFSET_PIXELS + row * CELL_SIZE_PIXELS)
                );

                for (int col = 0; col <= COLS_NUMBER; col++)
                {
                    g.DrawLine(Pens.Cyan,
                        new Point(FIELD_LEFT_OFFSET_PIXELS + col * CELL_SIZE_PIXELS, FIELD_TOP_OFFSET_PIXELS),
                        new Point(FIELD_LEFT_OFFSET_PIXELS + col * CELL_SIZE_PIXELS, FIELD_TOP_OFFSET_PIXELS + CELL_SIZE_PIXELS * COLS_NUMBER)
                    );
                }
            }
        }

        private void GenerateFood()
        {
            bool isFoodClashWithSnake;
            do
            {
                food = new Point(rand.Next(0, ROWS_NUMBER), rand.Next(0, COLS_NUMBER));
                isFoodClashWithSnake = false;
                foreach (Point p in snake)
                {
                    if (p.X == food.X && p.Y == food.Y)
                    {
                        isFoodClashWithSnake = true;
                        break;
                    }
                }
            } while (isFoodClashWithSnake);

            timer.Interval -= SPEED_INCREMENT_BY;
        }


    }
}
