using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Graphics_10
{
    public partial class Form1 : Form
    {
        float xmin = 1.0f, xmax = 9.0f, ymin = 1.0f, ymax = 6.0f;
        Graphics dc; Pen p;

        public Form1()
        {
            InitializeComponent();
            dc = pictureBox1.CreateGraphics();
            p = new Pen(Brushes.Red, 1);
        }
        /* Метод преобразования вещественной координаты X в целую */
        private int IX(double x)
        {
            double xx = x * (pictureBox1.Size.Width / 10.0) + 0.5;
            return (int)xx;
        }
        /* Метод преобразования вещественной координаты Y в целую */
        private int IY(double y)
        {
            double yy = pictureBox1.Size.Height - y * (pictureBox1.Size.Height / 7.0) + 0.5;
            return (int)yy;
        }
        /* Своя функция вычечивания линии (экран 10х7 условных единиц) */
        private void Draw(double x1, double y1, double x2, double y2)
        {
            Point point1 = new Point(IX(x1), IY(y1));
            Point point2 = new Point(IX(x2), IY(y2));
            dc.DrawLine(p, point1, point2);
        }
        /* Метод получение кода положения точки относительно окна по алгоритму Коєна-Сазерленда */
        private uint code(double x, double y)
        {
            return (uint)((Convert.ToUInt16(x < xmin) << 3) |
            (Convert.ToUInt16(x > xmax) << 2) |
            (Convert.ToUInt16(y < ymin) << 1) |
            Convert.ToUInt16(y > ymax));
        }
        /* Метод отсечения линий */
        private void clip(double x1, double y1, double x2, double y2)
        {
            uint c1;
            uint c2;
            double dx, dy;
            c1 = code(x1, y1);
            c2 = code(x2, y2);

            while ((c1 | c2) != 0)
            {
                if ((c1 & c2) != 0) return;
                dx = x2 - x1;
                dy = y2 - y1;
                if (c1 != 0)
                {
                    if (x1 < xmin) { y1 += dy * (xmin - x1) / dx; x1 = xmin; }
                    else
                    if (x1 > xmax) { y1 += dy * (xmax - x1) / dx; x1 = xmax; }
                    else
                    if (y1 < ymin) { x1 += dx * (ymin - y1) / dy; y1 = ymin; }
                    else
                    if (y1 > ymax) { x1 += dx * (ymax - y1) / dy; y1 = ymax; }
                    c1 = code(x1, y1);
                }
                else
                {
                    if (x2 < xmin) { y2 += dy * (xmin - x2) / dx; x2 = xmin; }
                    else
                    if (x2 > xmax) { y2 += dy * (xmax - x2) / dx; x2 = xmax; }
                    else
                    if (y2 < ymin) { x2 += dx * (ymin - y2) / dy; y2 = ymin; }
                    else
                    if (y2 > ymax) { x2 += dx * (ymax - y2) / dy; y2 = ymax; }
                    c2 = code(x2, y2);
                }
            }
            Draw(x1, y1, x2, y2); // Соединяем точки линией
        }
        /* Основной код программы */
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "") 
                { 
                    throw new EmptyExeption("Поля для ввода должны быть заполнены!"); 
                }
                if (-1>int.Parse(textBox1.Text)|int.Parse(textBox1.Text)>11|| -1 > int.Parse(textBox2.Text) | int.Parse(textBox2.Text) > 11 || -1 > int.Parse(textBox3.Text) | int.Parse(textBox3.Text) > 11 || -1 > int.Parse(textBox4.Text) | int.Parse(textBox4.Text) > 11) 
                {
                    throw new OutOfRange("В полях для ввода должны быть значения не меньше 0 и не больше 10");
                }
                xmin = float.Parse(textBox1.Text);
                xmax = float.Parse(textBox2.Text);
                ymin = float.Parse(textBox3.Text); 
                ymax = float.Parse(textBox4.Text);
                double r, x0, y0, x1, y1, x2, y2;
                x0 = 20.0; y0 = 8.0;
                /* Вычерчивание границ окна */
                Draw(xmin, ymin, xmax, ymin); Draw(xmax, ymin, xmax, ymax);
                Draw(xmax, ymax, xmin, ymax); Draw(xmin, ymax, xmin, ymin);
                /* В пределах границ окна вычерчиваются 20 правильных концентрических пятиугольников */
                for (r = 0.5; r < 30; r += 0.5)
                {
                    x2 = x0 - r * Math.Cos(-45); y2 = y0 + r * Math.Sin(-45);
                    x1 = x2; y1 = y2;
                    x2 = x0 - r; y2 = y0 + r;

                    clip(x1, y1, x2, y2);

                }
            }
            catch (EmptyExeption ex)
            {
                MessageBox.Show($"Ошибка:{ex.Message}");
            }
            catch (OutOfRange ex)
            {
                MessageBox.Show($"Ошибка:{ex.Message}");
            }
            
        }
    }
}
