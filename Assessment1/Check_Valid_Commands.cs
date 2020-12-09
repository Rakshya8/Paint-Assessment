using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;

namespace Assessment1
{
    /// <summary>
    /// Check if given commands are valid
    /// </summary>
    public class Check_Valid_Commands
    {
        Form1 form;
        ArrayList errors = new ArrayList();
        private string conditionOperator = null;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Check_Valid_Commands()
        {

        }
        /// <summary>
        /// Constructor with form object as parameter
        /// </summary>
        /// <param name="frm">Object of Form1</param>
        public Check_Valid_Commands(Form1 frm)
        {
            form = frm;
        }

        /// <summary>
        /// Check for valid execution commands
        /// </summary>
        /// <param name="command">executing command</param>
        /// <returns>true if valid and false if invalid</returns>
        public bool Valid_execute_command(string command)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public string check_command_type(string cmd)
        {
            string[] command = cmd.Split(' ');
            string type = null;
            if (command[0].Equals("if"))
            {
                type = "if";
            }
            else if (command[0].Equals("loop") && command[1].Equals("for"))
            {
                type = "loop";
            }
            else if (command[0].Equals("method"))
            {
                type = "method";
            }
            else if (command[0].Equals("moveto") || command[0].Equals("drawto") || command[0].Equals("pen") || command[0].Equals("fill") || command[0].Equals("circle") || command[0].Equals("rectangle") || command[0].Equals("triangle") || command[0].Equals("polygon"))
            {
                type = "drawing_commands";
            }
            else
            {
                
                if(cmd.Split('=').Length == 2) {
                    //string variable_name = cmd.Split('=')[0].Trim();
                    //string variable_value = cmd.Split('=')[1].Trim();

                    type = "variable";
                }
                else
                {
                    type = "invalid";
                }
                              
            }
            return type;
        }

        /// <summary>
        /// Check validitiy of complex commands
        /// </summary>
        /// <param name="command">command to be checked</param>
        /// <returns></returns>
        public bool check_variable(string command)
        {
            string variable_name = command.Split('=')[0].Trim();
            string first_char = variable_name.Substring(0, 1);
            if(Regex.IsMatch(first_char, @"^[a-zA-Z]+$"))
            {
                if (Regex.IsMatch(variable_name, @"^[a-zA-Z0-9]+$"))
                {
                    try
                    {
                        int.Parse(command.Split('=')[1].Trim());
                        return true;
                    }
                    catch
                    {
                        return false;
                        throw (new CustomExceptions("Variable value should be in number format."));                        
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw (new CustomExceptions("Variable name should start with alphabet."));
            }        
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool check_if_command(string command)
        {
            string condition;
            if (command.Split('(', ')').Length > 1){
                 condition = command.Split('(', ')')[1].Trim();
                //check for operator
                if (condition.Contains("<=") && !condition.Contains(">"))
                {
                    conditionOperator = "<=";
                    return true;
                }
                else if (condition.Contains(">=") && !condition.Contains("<"))
                {
                    conditionOperator = ">=";
                    return true;
                }
                else if (condition.Contains("!="))
                {
                    conditionOperator = "!=";
                    return true;
                }
                else if (condition.Contains("=") && !condition.Contains(">") && !condition.Contains("<"))
                {
                    conditionOperator = "=";
                    return true;
                }
                else if (!condition.Contains("=") && condition.Contains(">") && !condition.Contains("<"))
                {
                    conditionOperator = ">";
                    return true;

                }
                else if (!condition.Contains("=") && !condition.Contains(">") && condition.Contains("<"))
                {
                    conditionOperator = "<";
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string getOperator()
        {
            return conditionOperator;
        }

        /// <summary>
        /// Check for valid program commands
        /// </summary>
        /// <param name="command">program commands</param>
        /// <returns>true if valid and false if invalid</returns>
        public bool Check_command(string command)
        {

            string Draw_cmd = command.Split('(')[0].Trim();
            if (Draw_cmd.Equals("moveto") || Draw_cmd.Equals("drawto") || Draw_cmd.Equals("pen") || Draw_cmd.Equals("fill") || Draw_cmd.Equals("circle") || Draw_cmd.Equals("rectangle") || Draw_cmd.Equals("triangle") || Draw_cmd.Equals("polygon"))
            {
                string value_inside_brackets = null;
                string[] parameters = null;               
                if (command.Split('(', ')').Length > 1)
                {
                    value_inside_brackets = command.Split('(', ')')[1];
                    parameters = value_inside_brackets.Split(',');
                }
                else
                {
                    errors.Add("Parameters Missing");
                    return false;
                }
                //check moveto and Drawto command
                if (Draw_cmd.Equals("moveto") || Draw_cmd.Equals("drawto"))
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
                            errors.Add("X and Y points should be in numbers (0-9)");
                            return false;
                        }
                    }
                    else
                    {
                        errors.Add("2 parameters needed.");
                        return false;
                    }
                }
                //end of check moveto and Drawto command

                //check pen command
                if (Draw_cmd.Equals("pen"))
                {
                    if (!value_inside_brackets.Contains(','))
                    {
                        if (value_inside_brackets.Equals("red") || value_inside_brackets.Equals("green") || value_inside_brackets.Equals("blue") || value_inside_brackets.Equals("black") || value_inside_brackets.Equals("orange"))
                        {
                            return true;
                        }
                        else
                        {
                            errors.Add("Color not supported.");
                            return false;
                        }
                    }
                    else
                    {
                        errors.Add("1 parameter needed.");
                        return false;
                    }

                }
                //end of pen command

                //check fill command
                if (Draw_cmd.Equals("fill"))
                {

                    if (!value_inside_brackets.Contains(','))
                    {
                        if (value_inside_brackets.Equals("on") || value_inside_brackets.Equals("off"))
                        {
                            return true;
                        }
                        else
                        {
                            errors.Add("Invalid Option.");
                            return false;
                        }
                    }
                    else
                    {
                        errors.Add("Only 1 parameter allowed.");
                        return false;
                    }

                }
                //end fill command

                //check circle command
                if (Draw_cmd.Equals("circle"))
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
                            errors.Add("Radius should be in numbers (0-9).");
                            return false;
                        }
                    }
                    else
                    {
                        errors.Add("1 parameter needed.");
                        return false;
                    }
                }
                //end circle command

                //check circle command
                if (Draw_cmd.Equals("polygon"))
                {
                    if (parameters.Length % 2 == 0 && parameters.Length >= 4)
                    {
                        try
                        {
                            Array.ConvertAll(parameters, int.Parse);
                            return true;
                        }
                        catch (FormatException)
                        {
                            errors.Add("Points should be in numbers.");
                            return false;
                        }
                    }
                    else
                    {
                        errors.Add("Minimum 2 points (4 parameters) needed.");
                        return false;
                    }
                }
                //end circle command

                //check rectangle command
                if (Draw_cmd.Equals("rectangle"))
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
                            errors.Add("Length and breadth should be in numbers (0-9).");
                            return false;
                        }
                    }
                    else
                    {
                        errors.Add("2 parameters needed.");
                        return false;
                    }
                }
                //end rectangle command

                //check triangle command
                if (Draw_cmd.Equals("triangle"))
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
                            errors.Add("Sides should be in numbers (0-9).");
                            return false;
                        }
                    }
                    else
                    {
                        errors.Add("3 parameters needed.");
                        return false;
                    }
                }
                //end triangle command
            }
            else
            {
                errors.Add("Invalid Command.");
                return false;
            }
            return false;
        }

        /// <summary>
        /// Stores list of errors found during command validation
        /// </summary>
        /// <returns>returns list of errors</returns>
        public string error_list()
        {
            string error = null;
            foreach(string err in errors)
            {
                error = err;
            }
            return error;
        }

    }
}