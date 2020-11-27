using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace Assessment1
{
    
    public partial class Form1 : Form
    {
        
        Graphics g;
        ShapeFactory factory = new ShapeFactory();
        bool fillshape = false;
        bool valid_exexute_command;
        bool valid_program_command;
        
        //default settings
        Color color = Color.Black;
        int initX = 0;
        int initY = 0;

        
        public Form1()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();
            
        }

        
        bool new_point = false;
        private void button1_Click(object sender, EventArgs e)
        {

            string executing_command = textBox3.Text.ToLower(); 
            Point panel_location = panel1.PointToScreen(Point.Empty);
            Pen p1 = new Pen(color, 4);
            SolidBrush sb = new SolidBrush(p1.Color);

            if (executing_command.Equals("clear"))
            {
                g.Clear(Color.White);
            }
            if (executing_command.Equals("reset"))
            {
                Cursor.Position = new Point(0, 0);
                new_point = false;
            }
            if (executing_command.Equals("run"))
            {
                String[] lines = textBox1.Text.ToLower().Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string drawing_command in lines)
                {
                
                if (drawing_command.Contains("moveto"))
                {
                    string positions = drawing_command.Split('(', ')')[1];
                    initX = int.Parse(positions.Split(',')[0]);
                    initY = int.Parse(positions.Split(',')[1]);
                    new_point = true;
                        //calculate_coordinate(initX, initY);
                    this.Cursor = new Cursor(Cursor.Current.Handle);
                    Cursor.Position = new Point(panel_location.X + initX, panel_location.Y + initY);
                }

                    if (drawing_command.Contains("drawto"))
                    {
                        string positions = (drawing_command.Split('(', ')')[1]);
                        int third_point = int.Parse(positions.Split(',')[0]);
                        int fourth_point = int.Parse(positions.Split(',')[1]);

                        g.DrawLine(p1, initX, initY, third_point, fourth_point);
                    }

                    if (drawing_command.Contains("pen"))
                    {
                        string pen_color_name = (drawing_command.Split('(', ')')[1]);
                        if (pen_color_name.Contains("red"))
                        {
                            color = Color.Red;
                        }
                        if (pen_color_name.Contains("green"))
                        {
                            color = Color.Green;
                        }
                        if (pen_color_name.Contains("blue"))
                        {
                            color = Color.Blue;
                        }
                        if (pen_color_name.Contains("black"))
                        {
                            color = Color.Black;
                        }
                        if (pen_color_name.Contains("orange"))
                        {
                            color = Color.Orange;
                        }
                    }
                    if (drawing_command.Contains("fill"))
                    {
                        string fillstring = (drawing_command.Split('(', ')')[1]);
                        if (fillstring.Contains("on"))
                        {
                            fillshape = true;
                        }
                        else if (fillstring.Contains("off"))
                        {
                            fillshape = false;
                        }
                        else
                        {
                            MessageBox.Show("Invalid Command. Commands are: on and off");
                        }
                        
                    }

                    if (drawing_command.Contains("circle"))
                {
                        int radius = int.Parse(drawing_command.Split('(', ')')[1]);
                        shape s = factory.getShape("circle");
                        s.set(color, initX, initY, radius);
                        s.draw(g, fillshape);

                }

                 //


                    //Rectangle
                    if (drawing_command.Contains("rectangle"))
                    {
                        
                        string size = (drawing_command.Split('(', ')')[1]);
                        float length = float.Parse(size.Split(',')[0]);
                        float width = float.Parse(size.Split(',')[1]);
                        if (new_point)
                        {
                            g.FillRectangle(sb, initX, initY, length, width);
                        }
                        else
                        {
                            g.FillRectangle(sb, 0, 0, length, width);
                        }
                        
                    }
                    if (drawing_command.Contains("traingle"))
                    {
                        
                        string size = (drawing_command.Split('(', ')')[1]);
                        float side1 = float.Parse(size.Split(',')[0]);
                        float side2 = float.Parse(size.Split(',')[1]);
                        float side3 = float.Parse(size.Split(',')[2]);

                        PointF[] points = new PointF[3];
                        points[0].X = initX;
                        points[0].Y = initY;

                        points[1].X = initX + side1;
                        points[1].Y = initY;

                        points[2].X = initX + side3 ;
                        points[2].Y = initY - side2;

                        
                        if (new_point)
                        {
                            g.DrawPolygon(p1, points);
                            //g.DrawLine(p1, initX, initY, initX + side1, initY);
                            //g.DrawLine(p1, initX + side1, initY, initX + side1, initY - side2);
                            //g.DrawLine(p1, initX + side1, initY - side2, initX, initY - side2 + side3);
                        }
                        else
                        {
                        
                        }

                    }


                }
            }

            
        }


        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save Text File";
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.ShowDialog();
            string path = saveFileDialog1.FileName;
            File.WriteAllText(path, textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Browse Text Files";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.ShowDialog();
            string path = openFileDialog1.FileName;
            string readfile = File.ReadAllText(path);
            textBox1.Text = readfile;

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
