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
        //GDI+ Drawing surface
        Graphics graphics;

        //Object of class BasicDrawing
        BasicDrawing bd = new BasicDrawing();
        //Object of class Shape
        Shape s;

        //Object of class Check_Valid_Commands
        Check_Valid_Commands check_cmd;
        //store list of objects.
        ArrayList shape_list = new ArrayList();
        ArrayList if_commands = new ArrayList();
        //checks if fill is on/off
        private bool fillshape = false;
        //count line of commands
        int count_line = 0;
        IDictionary<string,
        int> variable = new Dictionary<string,
        int>();
        IDictionary<string,
        int> if_statement = new Dictionary<string,
        int>();
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
                        String[] lines = textBox1.Text.Trim().ToLower().Split(new String[] {
              Environment.NewLine
            },
                        StringSplitOptions.None);
                        foreach (string Draw in lines)
                        {

                            string command_type = check_cmd.check_command_type(Draw);

                            //if-command-start
                            if (command_type.Equals("if"))
                            {
                                if (check_cmd.check_if_command(Draw))
                                {
                                    string condition = Draw.Split('(', ')')[1].Trim();
                                    string operators = check_cmd.getOperator();
                                    string[] splitCondition = condition.Split(new string[] {
                    operators
                  },
                                    StringSplitOptions.RemoveEmptyEntries);
                                    if (splitCondition.Length == 2)
                                    {
                                        string conditionKey = splitCondition[0].Trim();
                                        int conditionValue = int.Parse(splitCondition[1].Trim());
                                        foreach (KeyValuePair<string, int> kvp in variable)
                                        {
                                            if (conditionKey == kvp.Key)
                                            {
                                                int variableValue = kvp.Value;

                                                bool conditionStatus = false;

                                                int value1 = variableValue;
                                                int value2 = conditionValue;

                                                if (operators == "<=")
                                                {
                                                    if (value1 <= value2) conditionStatus = true;
                                                }
                                                else if (operators == ">=")
                                                {
                                                    if (value1 >= value2) conditionStatus = true;
                                                }
                                                else if (operators == "=")
                                                {
                                                    if (value1 == value2) conditionStatus = true;
                                                }
                                                else if (operators == ">")
                                                {
                                                    if (value1 > value2) conditionStatus = true;
                                                }
                                                else if (operators == "<")
                                                {
                                                    if (value1 < value2) conditionStatus = true;
                                                }
                                                else if (operators == "!=")
                                                {
                                                    if (value1 != value2) conditionStatus = true;
                                                }

                                                if (conditionStatus)
                                                {
                                                    for (int i = (count_line + 1); i < lines.Length; i++)
                                                    {
                                                        while (!lines[i + 1].Contains("endif"))
                                                        {
                                                            if_commands.Add(lines[i]);
                                                        }
                                                    }
                                                    textBox2.AppendText(if_commands + "");
                                                }
                                            }
                                            else
                                            {
                                                throw (new CustomExceptions("Variable: " + conditionKey + "doesn't exist."));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        throw (new CustomExceptions("Invalid If EndIf Statement"));
                                    }
                                }
                                else
                                {
                                    throw (new CustomExceptions("Invalid If EndIf Statement"));
                                }

                            }
                            //if-command-end
                            else if (command_type.Equals("loop"))
                            {

                            }
                            else if (command_type.Equals("method"))
                            {

                            }
                            else if (command_type.Equals("drawing_commands"))
                            {
                                if (!check_cmd.Check_command(Draw))
                                {
                                    error++;
                                    textBox2.AppendText(Environment.NewLine + "Error on line " + count_line + " : " + check_cmd.error_list());
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
                                    if (Drawing_command.Equals("circle") || Drawing_command.Equals("triangle") || Drawing_command.Equals("rectangle") || Drawing_command.Equals("polygon"))
                                    {
                                        bd.SetBasicDrawing(Draw, color, fillshape, initX, initY);
                                    }
                                }

                                else
                                {

                                }

                            }
                            else if (command_type.Equals("variable"))
                            {
                                if (check_cmd.check_variable(Draw))
                                {
                                    string variable_name = Draw.Split('=')[0].Trim();
                                    int variable_value = int.Parse(Draw.Split('=')[1].Trim());

                                    if (!variable.ContainsKey(variable_name))
                                    {
                                        variable.Add(variable_name, variable_value);
                                    }
                                    else
                                    {
                                        variable[variable_name] = variable_value;
                                    }
                                }
                                else
                                {
                                    error++;
                                }
                            }
                            count_line++;
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
            shape_list = bd.getShape();
            for (int i = 0; i < shape_list.Count; i++)
            {
                s = (Shape)shape_list[i];
                s.Draw(graphics);
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

    }
}