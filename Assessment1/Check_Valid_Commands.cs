using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Assessment1
{
    class Check_Valid_Commands
    {
        Form1 form;
        public Check_Valid_Commands(Form1 frm)
        {
            form = frm;
        }

        public bool valid_execute_command(string command)
        {
            string valid_cmd = command;
            if (valid_cmd.Equals("run") || valid_cmd.Equals("reset") || valid_cmd.Equals("clear"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool check_command(string command)
        {
            
            string draw_cmd = command.Split('(')[0];
            if (draw_cmd.Equals("moveto") || draw_cmd.Equals("drawto") || draw_cmd.Equals("pen") || draw_cmd.Equals("fill") || draw_cmd.Equals("circle") || draw_cmd.Equals("rectangle") || draw_cmd.Equals("traingle"))
            {
                string value_inside_brackets = command.Split('(', ')')[1];
                string[] parameters = value_inside_brackets.Split(',');

                //check moveto and drawto command
                if (draw_cmd.Equals("moveto") || draw_cmd.Equals("drawto"))
                {
                    if (parameters.Length == 2)
                    {
                        try
                        {
                            int.Parse(parameters[0]);
                            int.Parse(parameters[1]);
                            return true;
                        }
                        catch (FormatException)
                        {

                            form.error_display("X and Y points should be in numbers (0-9)");
                            return false;
                        }
                    }
                    else
                    {
                        form.error_display("Only 2 parameters allowed.");
                        return false;
                    }
                }
                //end of check moveto and drawto command

                //check pen command
                if (draw_cmd.Equals("pen"))
                {
                    if (!value_inside_brackets.Contains(','))
                    {
                        if (value_inside_brackets.Equals("red") || value_inside_brackets.Equals("green") || value_inside_brackets.Equals("blue") || value_inside_brackets.Equals("black") || value_inside_brackets.Equals("orange"))
                        {
                            return true;
                        }
                        else
                        {
                            form.error_display("Color not supported.");
                            return false;
                        }
                    }
                    else
                    {
                        form.error_display("Only 1 parameter allowed.");
                        return false;
                    }

                }
                //end of pen command

                //check fill command
                if (draw_cmd.Equals("fill"))
                {

                    if (!value_inside_brackets.Contains(','))
                    {
                        if (value_inside_brackets.Equals("on") || value_inside_brackets.Equals("off"))
                        {
                            return true;
                        }
                        else
                        {
                            form.error_display("Invalid Option.");
                            return false;
                        }
                    }
                    else
                    {
                        form.error_display("Only 1 parameter allowed.");
                        return false;
                    }

                }
                //end fill command

                //check circle command
                if (draw_cmd.Equals("circle"))
                {
                    if (parameters.Length == 1)
                    {
                        try
                        {
                            int.Parse(parameters[0]);
                            return true;
                        }
                        catch (FormatException)
                        {
                            form.error_display("Radius should be in numbers (0-9).");
                            return false;
                        }
                    }
                    else
                    {
                        form.error_display("Only 1 parameter allowed.");
                        return false;
                    }
                }
                //end circle command

                //check rectangle command
                if (draw_cmd.Equals("rectangle"))
                {

                    if (parameters.Length == 2)
                    {
                        try
                        {
                            int.Parse(parameters[0]);
                            int.Parse(parameters[1]);
                            return true;
                        }
                        catch (FormatException)
                        {
                            form.error_display("Length and breadth should be in numbers (0-9).");
                            return false;
                        }
                    }
                    else
                    {
                        form.error_display("Only 2 parameters allowed.");
                        return false;
                    }
                }
                //end rectangle command

                //check traingle command
                if (draw_cmd.Equals("traingle"))
                {
                    if (parameters.Length == 3)
                    {
                        try
                        {
                            int.Parse(parameters[0]);
                            int.Parse(parameters[1]);
                            int.Parse(parameters[2]);
                            return true;
                        }
                        catch (FormatException)
                        {
                            form.error_display("Sides should be in numbers (0-9).");
                            return false;
                        }
                    }
                    else
                    {
                        form.error_display("Only 3 parameters allowed.");
                        return false;
                    }
                }
                //end traingle command

            }
            else
            {
                form.error_display("Invalid Command.");
                return false;
            }

            return false;
        }

    }
}