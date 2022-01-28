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
using System.Text.RegularExpressions;
using System.IO;

namespace Assessment1
{
    /// <summary>
    /// Form1 class inherits from the base class Form
    /// </summary>
    public partial class Form1 : Form
    {
        //GDI+ Drawing surfaces
        Graphics graphics;

        //Object of class Shape
        Shape s;

        //store list of objects.
        ArrayList shape_list = new ArrayList();

        //checks if fill is on/off
        private bool fillshape = false;

        //count line of commands
        int count_line = 0;

        //rotate degree
        static int rotate_degree = 0;

        //translate X and Y
        static int translateX = 0;
        static int translateY = 0;

        //Generic collection to store variable name  and value
        IDictionary<string, int> variables = new Dictionary<string, int>();

        //stores line drawing settings
        ArrayList drawline = new ArrayList();


        //default Settings
        //store pen color
        Color color = Color.Black;

        //Initalized vairables for flash colors
        Color flash_color1;
        Color flash_color2;
        //store x-axis point
        int initX = 0;
        //store y-axis point
        int initY = 0;

        //Initalized variable for Storing flash status
        bool flash = false;


        /// <summary>
        /// Default Constructor to instantiate values of form
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            graphics = panel1.CreateGraphics();
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
                if (Check_Valid_Commands.GetInstance.Valid_execute_command(executing_command))
                {
                    //if executing command is clear
                    if (executing_command.Equals("clear"))
                    {
                        graphics.Clear(Color.White);
                        drawline.Clear();
                        shape_list.Clear();
                        Shape.running = false;
                        textBox2.Text = "All shapes are cleared.";
                        if (ComplexDrawing.getMethodSignature().Count != 0 || ComplexDrawing.getVariables().Count != 0)
                        {
                            string message = "Do you want to variables and methods also?";
                            string title = "Clear Data";
                            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                            DialogResult result = MessageBox.Show(message, title, buttons);
                            if (result == DialogResult.Yes)
                            {
                                Check_Valid_Commands.GetInstance.clear_list();
                                ComplexDrawing.GetInstance.Clear_list();
                                textBox2.Text = "All shapes, variable and methods are cleared.";
                            }
                        }
                    }

                    //if executing command is reset
                    if (executing_command.Equals("reset"))
                    {
                        fillshape = false;
                        color = Color.Black;
                        initX = 0;
                        initY = 0;
                        flash = false;
                        translateX = 0;
                        translateY = 0;
                        rotate_degree = 0;
                        textBox2.Text = "Pen color set to Black" + Environment.NewLine + "Fill turned off"+Environment.NewLine + "Flash turned off" + Environment.NewLine + "Moved to 0,0";
                    }

                    //if executing command is run
                    if (executing_command.Equals("run"))
                    {
                        bool complex_command = false;                        //count current line number
                        count_line = 0;
                        //
                        int break_single_if_line = 0;
                        //

                        Check_Valid_Commands.GetInstance.clear_error();
                        //clear console
                        textBox2.Text = null;
                        //get string from textbox separated by newline and store into array
                        String[] lines = textBox1.Text.Trim().ToLower().Split(new String[] { Environment.NewLine }, StringSplitOptions.None);
                        ArrayList error_lines = new ArrayList();
                        error_lines.Clear();
                        //loop through commands
                        for (int i = 0; i < lines.Length; i++)
                        {
                            count_line++;
                            if (lines[i].Length == 0)
                            {
                                continue;
                            }
                            string Draw = lines[i];
                            //check which command is currently active
                            string command_type = Check_Valid_Commands.GetInstance.check_command_type(Draw);

                            //if endcommand is found before any command open
                            if (command_type.Equals("end_tag"))
                            {
                                if (!complex_command)
                                {
                                    error++;
                                    textBox2.AppendText(Environment.NewLine + "Error: Command " + Draw + " found. Command initiation doesnot exist.");
                                }
                                else
                                {
                                    complex_command = false;
                                }

                            }

                            //if command is invalid
                            if (command_type.Equals("invalid"))
                            {
                                textBox2.AppendText(Environment.NewLine + "Invalid Command ( " + Draw + " ) on line " + count_line);
                                continue;
                            }

                            //if/then command break
                            if (command_type.Equals("singleif"))
                            {
                                break_single_if_line = count_line + 1;
                            }

                            //if commands 'if/loop/method' are inactive
                            if (!complex_command && !command_type.Equals("end_tag"))
                            {
                                if (command_type.Equals("drawing_commands"))
                                {
                                    if (Check_Valid_Commands.GetInstance.Check_command(Draw))
                                    {
                                        if (error == 0)
                                        {
                                            draw_commands(Draw);
                                        }
                                    }
                                    else
                                    {
                                        error++;
                                        error_lines.Add(count_line);
                                    }
                                }
                            }


                            //type of command 
                            if (command_type.Equals("variable") || command_type.Equals("if") || command_type.Equals("loop") || command_type.Equals("method") || command_type.Equals("variableoperation") || command_type.Equals("methodcall"))
                            {
                                //method command
                                if (command_type.Equals("method"))
                                {
                                    complex_command = true;
                                    if (Check_Valid_Commands.GetInstance.check_method(Draw))
                                    {
                                        if (error == 0)
                                        {
                                            ComplexDrawing.GetInstance.run_method_command(Draw, lines, count_line, this);
                                        }
                                    }
                                    else
                                    {
                                        error++;
                                        error_lines.Add(count_line);
                                    }

                                }
                                //if command
                                if (command_type.Equals("if"))
                                {
                                    complex_command = true;
                                    if (Check_Valid_Commands.GetInstance.check_if_command(Draw))
                                    {

                                        if (error == 0)
                                        {

                                            ComplexDrawing.GetInstance.run_if_command(Draw, lines, count_line, this);
                                        }
                                    }
                                    else
                                    {
                                        error++;
                                        error_lines.Add(count_line);
                                    }
                                }

                                //loop command
                                if (command_type.Equals("loop"))
                                {
                                    complex_command = true;
                                    //check command validity
                                    if (Check_Valid_Commands.GetInstance.check_loop(Draw))
                                    {
                                        if (error == 0)
                                        {
                                            ComplexDrawing.GetInstance.run_loop_command(Draw, lines, count_line, this);
                                        }
                                    }
                                    else
                                    {
                                        error++;
                                        error_lines.Add(count_line);
                                    }

                                }

                                //variable command
                                if (command_type.Equals("variable"))
                                {
                                    if (Check_Valid_Commands.GetInstance.check_variable(Draw))
                                    {
                                        if (error == 0)
                                        {
                                            ComplexDrawing.GetInstance.run_variable_command(Draw);
                                        }
                                    }
                                    else
                                    {
                                        error++;
                                        error_lines.Add(count_line);
                                    }
                                }

                                //variable operation command
                                if (command_type.Equals("variableoperation"))
                                {
                                    if (Check_Valid_Commands.GetInstance.check_variable_operation(Draw))
                                    {
                                        if (error == 0)
                                        {
                                            ComplexDrawing.GetInstance.runVariableOperation(Draw, this);
                                        }
                                    }
                                    else
                                    {
                                        error++;
                                        error_lines.Add(count_line);
                                    }
                                }

                                //method call 
                                if (command_type.Equals("methodcall"))
                                {
                                    if (Check_Valid_Commands.GetInstance.check_methodcall(Draw))
                                    {
                                        if (error == 0)
                                        {
                                            ComplexDrawing.GetInstance.run_method_call(Draw, this);
                                        }
                                    }
                                    else
                                    {
                                        error++;
                                        error_lines.Add(count_line);
                                    }

                                }
                            }
                            if (break_single_if_line == count_line)
                            {
                                complex_command = false;
                            }
                        }

                        //show errors
                        if (error != 0)
                        {
                            int i = 0;
                            foreach (string error_description in Check_Valid_Commands.GetInstance.error_list())
                            {
                                textBox2.AppendText(Environment.NewLine + "Error on line " + error_lines[i] + " : " + error_description);
                                i++;
                            }
                            textBox2.AppendText(Environment.NewLine + "Please correct command syntax to continue.");
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Basic drawing commands are executed here
        /// </summary>
        /// <param name="Draw">command to be executed</param>
        public void draw_commands(string Draw)
        {
            //get variables
            variables = ComplexDrawing.getVariables();
            //remove all whitespace
            Draw = Regex.Replace(Draw, @"\s+", "");
            string Drawing_command = Draw.Split('(')[0];
            if (Drawing_command.Equals("moveto"))
            {
                string positions = Draw.Split('(', ')')[1];

                if (!Regex.IsMatch(positions.Split(',')[0], @"^[0-9]+$"))
                {
                    variables.TryGetValue(positions.Split(',')[0], out initX);
                }
                else
                {
                    initX = int.Parse(positions.Split(',')[0]);
                }
                if (!Regex.IsMatch(positions.Split(',')[1], @"^[0-9]+$"))
                {
                    variables.TryGetValue(positions.Split(',')[1], out initY);
                }
                else
                {
                    initY = int.Parse(positions.Split(',')[1]);
                }
            }

            if (Drawing_command.Equals("drawto"))
            {
                Pen p1 = new Pen(color, 4);
                string positions = (Draw.Split('(', ')')[1]);
                int pointX;
                int pointY;
                if (!Regex.IsMatch(positions.Split(',')[0], @"^[0-9]+$"))
                {
                    variables.TryGetValue(positions.Split(',')[0], out pointX);
                }
                else
                {
                    pointX = int.Parse(positions.Split(',')[0]);
                }
                if (!Regex.IsMatch(positions.Split(',')[1], @"^[0-9]+$"))
                {
                    variables.TryGetValue(positions.Split(',')[1], out pointY);
                }
                else
                {
                    pointY = int.Parse(positions.Split(',')[1]);
                }
                drawline.Add(p1);
                drawline.Add(initX);
                drawline.Add(initY);
                drawline.Add(pointX);
                drawline.Add(pointY);
                panel1.Refresh();
            }

            if (Drawing_command.Equals("rotate"))
            {
                string positions = Draw.Split('(', ')')[1];

                if (!Regex.IsMatch(positions.Split(',')[0], @"^[0-9]+$"))
                {
                    variables.TryGetValue(positions.Split(',')[0], out rotate_degree);
                }
                else
                {
                    rotate_degree = int.Parse(positions.Split(',')[0]);
                }
            }
            if (Drawing_command.Equals("translate"))
            {
                string positions = Draw.Split('(', ')')[1];

                if (!Regex.IsMatch(positions.Split(',')[0], @"^[0-9]+$"))
                {
                    variables.TryGetValue(positions.Split(',')[0], out translateX);
                }
                else
                {
                    translateX = int.Parse(positions.Split(',')[0]);
                }

                if (!Regex.IsMatch(positions.Split(',')[0], @"^[0-9]+$"))
                {
                    variables.TryGetValue(positions.Split(',')[1], out translateY);
                }
                else
                {
                    translateY = int.Parse(positions.Split(',')[1]);
                }
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


            if (Drawing_command.Equals("redgreen"))
            {
                flash = true;
                flash_color1 = Color.Red;
                flash_color2 = Color.Green;
                Shape.running = true;

            }
            if (Drawing_command.Equals("blueyellow"))
            {
                flash = true;
                flash_color1 = Color.Blue;
                flash_color2 = Color.Yellow;
                Shape.running = true;

            }
            if (Drawing_command.Equals("blackwhite"))
            {
                flash = true;
                flash_color1 = Color.Black;
                flash_color2 = Color.White;
                Shape.running = true;
            }


            if (Drawing_command.Equals("circle") || Drawing_command.Equals("triangle") || Drawing_command.Equals("rectangle") || Drawing_command.Equals("polygon"))
            {
                BasicDrawing.GetInstance.SetBasicDrawing(Draw, color, fillshape, flash, flash_color1, flash_color2, initX, initY);
                panel1.Refresh();
            }

        }

        /// <summary>
        /// Stores degree of rotation
        /// </summary>
        /// <returns>degree of rotation</returns>
        public static int RotateShape()
        {
            return rotate_degree;
        }

        /// <summary>
        /// Stores X of Translate
        /// </summary>
        /// <returns>X of Translate</returns>
        public static int TranslateX()
        {
            return translateX;
        }
        /// <summary>
        /// Stores Y of Translate
        /// </summary>
        /// <returns>Y of Translate</returns>
        public static int TranslateY()
        {
            return translateY;
        }

        /// <summary>
        /// Paint Event of panel1
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            shape_list = BasicDrawing.GetInstance.getShape();
            for (int i = 0; i < shape_list.Count; i++)
            {
                s = (Shape)shape_list[i];
                s.Draw(graphics);
                Console.WriteLine(1000000);
            }

            if (drawline.Count != 0)
            {
                int no_of_draw = drawline.Count / 5;

                for (int i = 0; i < no_of_draw; i++)
                {
                    for (int j = 0; j < drawline.Count; j = j + 5)
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

        /// <summary>
        /// Shows list of executing loop commands available
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>
        private void ifLoopMethodCmdsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" IF: If <variable>=10 \n Line 1 \n Line 2 Endif \n " +
                "-----------------------\nLOOP: radius = 10 \n count = 1 \n loop for count <= 5 \n circle(radius) \n radius + 10  \n count + 1 \n endloop \n" +
                "-----------------------\nMETHOD: Define a method with: \n Method myMethod(parameter list) \n Line 1 \n Etc \n Endmethod \n Call a method with: \n myMethod(< parameter list >) ");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter_1(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Opens a save dialog to save commands into a text format document
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
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
        private void loadToolStripMenuItem1_Click(object sender, EventArgs e)
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
        /// Shows About of the Program
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>
        private void loadToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Console Based Paint Application. Developed by © Rakshya Moktan 2021 ");

        }

        /// <summary>
        /// Exits the Program
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        /// <summary>
        /// Clears the Program
        /// </summary>
        /// <param name="sender">contains a reference to the control/object that raised the event.</param>
        /// <param name="e">contains the event data</param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            graphics.Clear(Color.White);
            drawline.Clear();
            shape_list.Clear();
            Shape.running = true;
            textBox2.Text = "All shapes are cleared.";
            if (ComplexDrawing.getMethodSignature().Count != 0 || ComplexDrawing.getVariables().Count != 0)
            {
                string message = "Do you want to variables and methods also?";
                string title = "Clear Data";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    Check_Valid_Commands.GetInstance.clear_list();
                    ComplexDrawing.GetInstance.Clear_list();
                    textBox2.Text = "All shapes, variable and methods are cleared.";
                }
            }
        }
    }
}