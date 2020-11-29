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
        ShapeFactory factory;
        shape s;
        Check_Valid_Commands check_cmd;
        private bool fillshape = false;
        int count_line = 0;

        //default settings
        Color color = Color.Black;
        int initX = 0;
        int initY = 0;

        public Form1()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();
            factory = new ShapeFactory();
            check_cmd = new Check_Valid_Commands(this);

        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            string executing_command = textBox3.Text.ToLower();
            if (check_cmd.valid_execute_command(executing_command))
            { 
                Point panel_location = panel1.PointToScreen(Point.Empty);
                

                if (executing_command.Equals("clear"))
                {
                    g.Clear(Color.White);
                }

                if (executing_command.Equals("reset"))
                {
                    Cursor.Position = new Point(panel_location.X, panel_location.Y);
                    initX = 0;
                    initY = 0;
                    color = Color.Black;
                    fillshape = false;

                }
                if (executing_command.Equals("run"))
                {
                    count_line = 0;
                    textBox2.Text = null;
                    String[] lines = textBox1.Text.Trim().ToLower().Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
                    foreach (string draw in lines)
                    {
                        count_line++;
                        if (check_cmd.check_command(draw)) { 
                            string drawing_command = draw.Split('(')[0];

                            if (drawing_command.Equals("moveto"))
                            {
                                string positions = draw.Split('(', ')')[1];
                                initX = int.Parse(positions.Split(',')[0]);
                                initY = int.Parse(positions.Split(',')[1]);

                                //calculate_coordinate(initX, initY);
                                this.Cursor = new Cursor(Cursor.Current.Handle);
                                Cursor.Position = new Point(panel_location.X + initX, panel_location.Y + initY);
                            }

                            if (drawing_command.Equals("drawto"))
                            {
                                Pen p1 = new Pen(color, 4);
                                string positions = (draw.Split('(', ')')[1]);
                                int pointX = int.Parse(positions.Split(',')[0]);
                                int pointY = int.Parse(positions.Split(',')[1]);

                                g.DrawLine(p1, initX, initY, pointX, pointY);
                            }

                            if (drawing_command.Equals("pen"))
                            {
                                string pen_color_name = (draw.Split('(', ')')[1]);
                                if (pen_color_name.Contains("red"))
                                {
                                    color = Color.Red;
                                }
                                else if (pen_color_name.Contains("green"))
                                {
                                    color = Color.Green;
                                }
                                else if (pen_color_name.Contains("blue"))
                                {
                                    color = Color.Blue;
                                }
                                else if (pen_color_name.Contains("black"))
                                {
                                    color = Color.Black;
                                }
                                else if (pen_color_name.Contains("orange"))
                                {
                                    color = Color.Orange;
                                }
                                else
                                {
                                    MessageBox.Show("Supported Color are:\n Red \n Green \n Blue \n Black \n Orange", "Unsupported Color");
                                }
                            }
                            if (drawing_command.Equals("fill"))
                            {
                                string fillstring = (draw.Split('(', ')')[1]);
                                if (fillstring.Equals("on"))
                                {
                                    fillshape = true;
                                }
                                else if (fillstring.Equals("off"))
                                {
                                    fillshape = false;
                                }
                                else
                                {
                                    MessageBox.Show("Invalid Command. Commands are: on and off");
                                }
                            }

                            if (drawing_command.Equals("circle"))
                            {
                                int radius = int.Parse(draw.Split('(', ')')[1]);
                                s = factory.getShape("circle");
                                s.set(color,fillshape, initX, initY, radius);
                                s.draw(g);
                            }

                            //Rectangle
                            if (drawing_command.Equals("rectangle"))
                            {
                                string size = (draw.Split('(', ')')[1]);
                                s = factory.getShape("rectangle");

                                int length = int.Parse(size.Split(',')[0]);
                                int width = int.Parse(size.Split(',')[1]);
                                s.set(color, fillshape, initX, initY, length, width);
                                s.draw(g);

                            }
                            if (drawing_command.Equals("traingle"))
                            {
                                string size = (draw.Split('(', ')')[1]);
                                s = factory.getShape("traingle");

                                int side1 = int.Parse(size.Split(',')[0]);
                                int side2 = int.Parse(size.Split(',')[1]);
                                int side3 = int.Parse(size.Split(',')[2]);

                                s.set(color, fillshape, initX, initY, side1, side2, side3);
                                s.draw(g);
                            }
                        }
                    }
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void error_display(string error)
        {
            string newline = Environment.NewLine;
            textBox2.AppendText(newline + "Line " + count_line +" : " + error);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save Text File";
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.ShowDialog();
            string path = saveFileDialog1.FileName;
            File.WriteAllText(path, textBox1.Text);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Browse Text Files";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.ShowDialog();
            string path = openFileDialog1.FileName;
            if (!string.IsNullOrEmpty(path))
            {
                string readfile = File.ReadAllText(path);
                textBox1.Text = readfile;
            }
            
        }

        private void commandListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" moveto(x-point,y-point) \n drawto(x-point,y-point) \n pen(color) \n fill(on || off) \n circle(radius) \n rectangle(length, breadth) \n traingle(side1, side2, side3)", "Valid Commands");
        }

        private void colorListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can use these colors:\n Red \n Green \n Blue \n Black \n Orange", "Valid Color");
        }

        private void shapeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" Circle \n Rectangle \n Traingle","Shape List");
        }

        private void actionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" Run: Runs The Commands. \n Clear: Clear Painting Area \n Reset: Reset All Settings.", "Actions List");
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
