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
    /// <summary>
    /// Form1 class inherits from the base class Form
    /// </summary>
    public partial class Form1 : Form
    {
        //GDI+ Drawing surface
        Graphics graphics;
        //Object of class ShapeFactory
        ShapeFactory factory;
        //Object of class Shape
        Shape s;
        //Object of class Check_Valid_Commands
        Check_Valid_Commands check_cmd;
        //store list of objects.
        ArrayList shape_list = new ArrayList();
        //checks if fill is on/off
        private bool fillshape = false;
        //count line of commands
        int count_line = 0;

        ArrayList drawline = new ArrayList();

        //default Settings
        //store pen color
        Color color = Color.Black;
        //store x-axis point
        int initX = 0;
        //store y-axis point
        int initY = 0;

        /// <summary>
        /// Default Constructor to instantiate values of form
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            graphics = panel1.CreateGraphics();
            factory = new ShapeFactory();
            check_cmd = new Check_Valid_Commands(this);

        }

        /// <summary>
        /// Event triggered on pressing any key while focused on this textbox
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>
        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                //store number of errors
                int error = 0;
                //store executing command in lowercase
                string executing_command = textBox3.Text.ToLower();

                //if executing command is valid
                if (check_cmd.Valid_execute_command(executing_command))
                {
                    //if executing command is clear
                    if (executing_command.Equals("clear"))
                    {
                        graphics.Clear(Color.White);
                        shape_list.Clear();
                        drawline.Clear();
                    }

                    //if executing command is reset
                    if (executing_command.Equals("reset"))
                    {
                        fillshape = false;
                        color = Color.Black;
                        initX = 0;
                        initY = 0;
                        textBox2.AppendText("Moved to 0,0");
                    }

                    //if executing command is run
                    if (executing_command.Equals("run"))
                    {
                        
                        //count current line number
                        count_line = 0;
                        //clear console
                        textBox2.Text = null;
                        //get string from textbox separated by newline and store into array
                        String[] lines = textBox1.Text.Trim().ToLower().Split(new String[] {Environment.NewLine},
                        StringSplitOptions.None);
                        foreach (string Draw in lines)
                        {
                            count_line++;
                            if (!check_cmd.Check_command(Draw))
                            {
                                error++;
                                textBox2.AppendText(Environment.NewLine + "Error on line "+count_line + " : " + check_cmd.error_list());
                            }
                            
                            if (error == 0)
                            {
                                string Drawing_command = Draw.Split('(')[0].Trim();

                                if (Drawing_command.Equals("moveto"))
                                {
                                    string positions = Draw.Split('(', ')')[1];
                                    initX = int.Parse(positions.Split(',')[0]);
                                    initY = int.Parse(positions.Split(',')[1]);
                                }

                                if (Drawing_command.Equals("drawto"))
                                {
                                    Pen p1 = new Pen(color, 4);
                                    string positions = (Draw.Split('(', ')')[1]);
                                    int pointX = int.Parse(positions.Split(',')[0]);
                                    int pointY = int.Parse(positions.Split(',')[1]);
                                    
                                    drawline.Add(p1);
                                    drawline.Add(initX);
                                    drawline.Add(initY);
                                    drawline.Add(pointX);
                                    drawline.Add(pointY);                                   
                                    //g.DrawLine(p1, initX, initY, pointX, pointY);
                                }

                                if (Drawing_command.Equals("pen"))
                                {
                                    string pen_color_name = (Draw.Split('(', ')')[1]);
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

                                }
                                if (Drawing_command.Equals("fill"))
                                {
                                    string fillstring = (Draw.Split('(', ')')[1]);
                                    if (fillstring.Equals("on"))
                                    {
                                        fillshape = true;
                                    }
                                    else if (fillstring.Equals("off"))
                                    {
                                        fillshape = false;
                                    }
                                }

                                if (Drawing_command.Equals("circle"))
                                {
                                    int radius = int.Parse(Draw.Split('(', ')')[1]);
                                    s = factory.getShape("circle");
                                    s.Set(color, fillshape, initX, initY, radius);
                                    shape_list.Add(s);
                                    
                                }

                                //Rectangle
                                if (Drawing_command.Equals("rectangle"))
                                {
                                    string size = (Draw.Split('(', ')')[1]);
                                    s = factory.getShape("rectangle");

                                    int length = int.Parse(size.Split(',')[0]);
                                    int width = int.Parse(size.Split(',')[1]);
                                    s.Set(color, fillshape, initX, initY, length, width);
                                    shape_list.Add(s);
                                    
                                }
                                //End Rectangle

                                //Start Triangle
                                if (Drawing_command.Equals("triangle"))
                                {
                                    string size = (Draw.Split('(', ')')[1]);
                                    s = factory.getShape("triangle");

                                    int side1 = int.Parse(size.Split(',')[0]);
                                    int side2 = int.Parse(size.Split(',')[1]);
                                    int side3 = int.Parse(size.Split(',')[2]);

                                    s.Set(color, fillshape, initX, initY, side1, side2, side3);
                                    shape_list.Add(s);
                                    
                                }

                                //End Triangle

                                //Start Polygon
                                if (Drawing_command.Equals("polygon"))
                                {
                                    string param = initX + "," + initY + "," +(Draw.Split('(', ')')[1]);
                                    int[] points = Array.ConvertAll(param.Split(','), int.Parse);                                   
                                    s = factory.getShape("polygon");                                   
                                    s.Set(color, fillshape, points);
                                    shape_list.Add(s);

                                }
                                //End Polygon
                            }
                            
                        }
                        if (error != 0)
                        {
                            textBox2.AppendText(Environment.NewLine + "Please check all commands again.");
                        }
                        panel1.Refresh();

                    }
                }
            }
        }

        /// <summary>
        /// Paint Event of panel1
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < shape_list.Count; i++)
            {
                s = (Shape)shape_list[i];
                s.Draw(graphics);
            }

            if(drawline.Count != 0)
            {
                int no_of_draw = drawline.Count / 5;
                
                for(int i=0; i < no_of_draw; i++)
                {
                    for(int j=0; j < drawline.Count; j = j + 5)
                    {
                        graphics.DrawLine((Pen)drawline[j], (int)drawline[j + 1], (int)drawline[j + 2], (int)drawline[j + 3], (int)drawline[j + 4]);
                    }
                    
                }
                               
            }
        }

        /// <summary>
        /// Opens a save dialog to save commands into a text format document
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save Text File";
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog1.FileName;
                File.WriteAllText(path, textBox1.Text);
                textBox2.AppendText("File Saved: " + path);
            }
        }

        /// <summary>
        /// Opens a open dialog to read text file and show commands
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Browse Text Files";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                string readfile = File.ReadAllText(path);
                textBox1.Text = readfile;
                textBox2.AppendText("File loaded: " + path);
            }
           

        }

        /// <summary>
        /// Shows list of program commands available
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>
        private void commandListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" moveto(x-point,y-point) \n Drawto(x-point,y-point) \n pen(color) \n fill(on || off) \n circle(radius) \n rectangle(length, breadth) \n traingle(side1, side2, side3)", "Valid Commands");
        }

        /// <summary>
        /// Shows list of color options available
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>
        private void colorListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You can use these colors:\n Red \n Green \n Blue \n Black \n Orange", "Valid Color");
        }

        /// <summary>
        /// Shows list of shapes commands available
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>
        private void shapeListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" Circle \n Rectangle \n Traingle", "Shape List");
        }

        /// <summary>
        /// Shows list of executing commands available
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>
        private void actionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" Run: Runs The Commands. \n Clear: Clear Painting Area \n Reset: Reset All settings.", "Actions List");
        }

        /// <summary>
        /// Indicates that the event handler has already processed the event and dealt with it, so it doesn't need to be processed any further.
        /// Disables user input for this textbox.
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

    }
}