using System;
using System.Windows.Forms;
using Tao.OpenGl;
using Tao.FreeGlut;
using System.Drawing;
using System.Security.Policy;

namespace KG3
{
    public partial class Form1 : Form
    {
        private double _Radius = 2;
        private double _Height = 5;
        private int _Slices = 25;
        private float _Alpha = 0.4f;
        public Form1()
        {
            InitializeComponent();
            AnT.InitializeContexts();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();

            // инициализация Glut
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);

            // очитка окна
            Gl.glClearColor(0, 0, 0, 1); // черный фон

            // установка порта вывода в соотвествии с размерами элемента 
            Gl.glViewport(0, 0, AnT.Width, AnT.Height);

            // настройка параллельной проекции
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

            const double W = 7;
            double H = W * AnT.Height / AnT.Width;
            Gl.glOrtho(-W, W, -H, H, -200, 200);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();

            // настройка параметров для визуализации

            //  включается расчет освещения
            Gl.glEnable(Gl.GL_LIGHTING);
            // разрешаем использовать light0
            Gl.glEnable(Gl.GL_LIGHT0);
        }
        private void Render(double x, double y)
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();            

            Gl.glPushMatrix();
            Gl.glTranslated(0, -1, -6);
            Gl.glRotated((x - AnT.Height) * 180 / AnT.Height, 0, 1, 0);
            Gl.glRotated((y - AnT.Width) * 180 / AnT.Width, 1, 0, 0);

           
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
            Gl.glEnable(Gl.GL_COLOR_MATERIAL);
            Gl.glColor4f(1, 1, 1, _Alpha);

            Glut.glutSolidCone(_Radius, _Height, _Slices, 1);

            Gl.glPopMatrix();
            Gl.glFlush();
            AnT.Invalidate();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow r in dataGridView1.Rows)
                    {
                        Gl.glEnable(Gl.GL_LIGHT0);
                        float[] pos =
                        {
                            (float) Convert.ToDecimal(dataGridView1.Rows[0].Cells[0].Value),
                            (float) Convert.ToDecimal(dataGridView1.Rows[0].Cells[1].Value),
                            (float) Convert.ToDecimal(dataGridView1.Rows[0].Cells[2].Value), 0.0f
                        };
                        Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, pos); // устанавливаем направление источника света
                   
                    float[] color =
                        {
                            ((Color) dataGridView1.Rows[0].Cells[3].Value).R/(float) 255,
                            ((Color) dataGridView1.Rows[0].Cells[3].Value).G/(float) 255,
                            ((Color) dataGridView1.Rows[0].Cells[3].Value).B/(float) 255, 1
                        };
                        Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, color);// устанавливаем источнику света light0 диффузный свет 
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка входных данных!");
            }
        }

        private void AnT_MouseMove(object sender, MouseEventArgs e)
        {            
            Render(e.X, e.Y);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3) 
                if (DialogResult.OK == colorDialog1.ShowDialog()) 
                    dataGridView1.Rows[e.RowIndex].Cells[3].Value = colorDialog1.Color;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _Radius = Convert.ToDouble(textBox1.Text);
            }
            catch (Exception)
            {
                _Radius = 5;
            }
            try
            {
                _Height = Convert.ToDouble(textBox2.Text);
            }
            catch (Exception)
            {
                _Height = 5;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

            try
            {
                double tmp = Convert.ToDouble(textBox3.Text);
                _Alpha = (float)tmp;
            }
            catch (Exception)
            {
                _Alpha = 0.4f;
            }
        }
    }
}

/*
            //Gl.glColor4f(1, 0, 0, 0.1f);
            //Gl.glEnable(Gl.GL_DEPTH_TEST);

            //Gl.glEnable(Gl.GL_ALPHA_TEST);
            //Gl.glAlphaFunc(Gl.GL_GREATER, 0.4f);
            // Gl.glDisable(Gl.GL_BLEND);
            //Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);
*/