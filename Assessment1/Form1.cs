using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assessment1
{
    public partial class Form1 : Form
    {
        Graphics g;
        public Form1()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string executing_command = textBox3.Text.ToLower();
            string drawing_command = textBox1.Text.ToLower();
            if (executing_command.Equals("clear"))
            {
                g.Clear(Color.White);
            }
            if (executing_command.Equals("reset"))
            {
     
                Cursor.Position = new Point(0, 0);
            }
            if (executing_command.Equals("run"))
            {
                if (drawing_command.Contains("moveto"))
                {
                    Point location = panel1.PointToScreen(Point.Empty);
                    string positions = drawing_command.Split('(', ')')[1];
                    int initX = int.Parse(positions.Split(',')[0]);
                    int initY = int.Parse(positions.Split(',')[1]);
                    int Panel_PositionX = location.X;
                    int Panel_PositionY = location.Y;
                    textBox2.AppendText(location+"");
                    this.Cursor = new Cursor(Cursor.Current.Handle);
                    Cursor.Position = new Point( Panel_PositionX + initX, Panel_PositionY + initY);
                    Point latestpoint = new Point(Panel_PositionX + initX, Panel_PositionY + initY);
                    Cursor.Clip = new Rectangle(this.Location, this.Size);
                }
                if (drawing_command.Contains("circle"))
                {
                    int radius = int.Parse(drawing_command.Split('(', ')')[1]);
                    
                    Pen p = new Pen(Color.Black, 4);
                    g.DrawEllipse(p, 75, 160, radius, radius);
                }
                }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
           label4.Text = string.Format("X: {0} , Y: {1}", Cursor.Position.X, Cursor.Position.Y);
        }

    }
}
