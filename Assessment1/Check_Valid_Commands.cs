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
        //store if statement operator
        static string conditionOperator = null;

        //store variables
        IDictionary<string,
        int> variable = new Dictionary<string,
        int>();

        //store method signature
        IDictionary<string,
        ArrayList> method_signature = new Dictionary<string,
        ArrayList>();

        //store errors
        ArrayList errors = new ArrayList();


        /// <summary>
        /// Default Constructor
        /// </summary>
        public Check_Valid_Commands()
        {

        }

        /// <summary>
        /// clear error list
        /// </summary>
        public void clear_error_list()
        {
            errors.Clear();
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
        /// check type of command
        /// </summary>
        /// <param name="cmd">command to be checked</param>
        /// <returns>type of command</returns>
        public string check_command_type(string cmd)
        {
            string type = null;
            if (cmd.Contains("if") && !cmd.Contains("endif"))
            {
                type = "if";
            }
            else if (cmd.Contains("then"))
            {
                type = "singleif";
            }
            else if (cmd.Contains("loop") && cmd.Contains("for"))
            {
                type = "loop";
            }
            else if (cmd.Contains("method") && !cmd.Contains("endmethod"))
            {
                if (cmd.Split(' ')[0].Equals("method"))
                {
                    type = "method";
                }
                else
                {
                    type = "methodcall";
                }
            }
            else if (cmd.Contains("moveto") || cmd.Contains("drawto") || cmd.Contains("pen") || cmd.Contains("fill") || cmd.Contains("circle") || cmd.Contains("rectangle") || cmd.Contains("triangle") || cmd.Contains("polygon"))
            {
                type = "drawing_commands";
            }
            else if (cmd.Contains("endif") || cmd.Contains("endloop") || cmd.Contains("endmethod"))
            {
                type = "end_tag";
            }
            else
            {
                if (cmd.Contains("="))
                {
                    if (cmd.Split('=').Length == 2)
                    {
                        type = "variable";
                    }
                }
                else if (cmd.Contains("+") || cmd.Contains("-") || cmd.Contains("*") || cmd.Contains("/"))
                {
                    type = "variableoperation";
                }
                else if (cmd.Contains("(") && cmd.Contains(")"))
                {
                    type = "methodcall";
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
            command = Regex.Replace(command, @"\s+", "");
            string variable_name = command.Split('=')[0];
            string first_char = variable_name.Substring(0, 1);
            try
            {
                if (Regex.IsMatch(first_char, @"^[a-zA-Z]+$"))
                {
                    if (Regex.IsMatch(variable_name, @"^[a-zA-Z0-9]+$"))
                    {
                        int.Parse(command.Split('=')[1]);
                        return true;
                    }
                    else
                    {
                        throw new InvalidVariableNameException("Variable name is invalid");
                    }
                }
                else
                {
                    throw new InvalidVariableNameException("Variable name should start with alphabet");
                }
            }
            catch (InvalidVariableNameException e)
            {
                errors.Add(e.Message);
                return false;
            }
            catch (FormatException)
            {
                errors.Add("Variable value should be in number format.");
                return false;
            }
        }

        /// <summary>
        /// check variable operations validity
        /// </summary>
        /// <param name="cmd">command to be checked</param>
        /// <returns>true if variable exist else false</returns>
        public bool check_variable_operation(string cmd)
        {
            variable = ComplexDrawing.getVariables();
            cmd = Regex.Replace(cmd, @"\s+", "");
            string[] parameter = cmd.Trim().Split(new Char[] { '+', '-', '*', '/' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                if (variable.ContainsKey(parameter[0]))
                {
                    return true;
                }
                else
                {
                    throw new InvalidCommandException("Variable not found");
                }
            }
            catch (InvalidCommandException e)
            {
                errors.Add(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Check validity of if command
        /// </summary>
        /// <param name="command">command to be checked</param>
        /// <returns>true if command is valid else false</returns>
        public bool check_if_command(string command)
        {
            command = Regex.Replace(command, @"\s+", "");
            string[] command_parts = command.Trim().Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries);
            string condition;
            try
            {
                if (command_parts[0].Equals("if"))
                {
                    if (command_parts.Length == 2)
                    {
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
                        else if (condition.Contains("==") && !condition.Contains(">") && !condition.Contains("<"))
                        {
                            conditionOperator = "==";
                            return true;
                        }
                        else if (!condition.Contains("==") && condition.Contains(">") && !condition.Contains("<"))
                        {
                            conditionOperator = ">";
                            return true;

                        }
                        else if (!condition.Contains("==") && !condition.Contains(">") && condition.Contains("<"))
                        {
                            conditionOperator = "<";
                            return true;
                        }
                        else
                        {
                            throw new InvalidParameterException("Invalid Operator Used.");
                        }
                    }
                    else
                    {
                        throw new InvalidCommandException("Invalid If Command Syntax");
                    }

                }
                else
                {
                    throw new InvalidCommandException("Invalid Command Name.");
                }
            }
            catch (InvalidCommandException e)
            {
                errors.Add(e.Message);
                return false;
            }
            catch (InvalidParameterException e)
            {
                errors.Add(e.Message);
                return false;
            }
        }

        /// <summary>
        /// get if condition operator
        /// </summary>
        /// <returns>operator used in if command</returns>
        public static string getOperator()
        {
            return conditionOperator;
        }

        /// <summary>
        /// check loop command validity
        /// </summary>
        /// <param name="command">command to be checked</param>
        /// <returns>true if command is valid else false</returns>
        public bool check_loop(string command)
        {
            variable = ComplexDrawing.getVariables();
            command = Regex.Replace(command, @"\s+", "");
            string[] check_cmd = command.Split(new string[] {
        "for"
      },

            StringSplitOptions.RemoveEmptyEntries);
            string[] loopCondition = { };
            try
            {
                if (!check_cmd[0].Equals("loop"))
                {
                    throw new InvalidCommandException("Invalid Command Name");
                }

                if (check_cmd.Length != 2)
                {
                    throw new InvalidCommandException("Invalid Loop Command Syntax");
                }

                loopCondition = check_cmd[1].Split(new string[] { "<=", ">=", "<", ">" }, StringSplitOptions.RemoveEmptyEntries);
                if (loopCondition.Length == 1)
                {
                    throw new InvalidParameterException("Invalid loop statement. Operator should be <= or => or < or >");
                }

                if (!Regex.IsMatch(check_cmd[1], @"^[0-9]+$"))
                {
                    string variable_name = loopCondition[0].ToLower().Trim();
                    if (!variable.ContainsKey(variable_name))
                    {
                        throw new VariableNotFoundException("Variable: " + variable_name + " not found.");
                    }
                }
            }
            catch (InvalidCommandException e)
            {
                errors.Add(e.Message);
                return false;
            }
            catch (VariableNotFoundException e)
            {
                errors.Add(e.Message);
                return false;
            }
            catch (InvalidParameterException e)
            {
                errors.Add(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// check validity of method command
        /// </summary>
        /// <param name="command">command to be checked</param>
        /// <returns>true if command is valid else false</returns>
        public bool check_method(string command)
        {

            string[] command_parts = command.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string parameter_inside_method = "";
            try
            {
                if (command_parts[0].Equals("method"))
                {
                    if (command_parts.Length == 3) //method 1asdc ()
                    {
                        if (Regex.IsMatch(command_parts[1], @"^[a-zA-Z0-9]+$"))
                        {
                            string first_char = command_parts[1].Substring(0, 1);

                            if (Regex.IsMatch(first_char, @"^[a-zA-Z]+$"))
                            {
                                string check_param = command_parts[2];
                                if (!check_param[0].Equals('(') || !check_param[check_param.Length - 1].Equals(')'))
                                {
                                    throw new InvalidMethodNameException("Invalid Method Command Syntax");
                                }
                                else
                                {
                                    parameter_inside_method = command_parts[2].Trim().Split('(', ')')[1];
                                    if (parameter_inside_method.Length > 0)
                                    {
                                        //check for alphanumeric or , inside ()
                                        for (int i = 1; i < check_param.Length - 1; i++)
                                        {
                                            if (!(Char.IsLetter(check_param[i]) || check_param[i].Equals(',')))
                                            {
                                                throw new InvalidParameterException("Invalid parameters");
                                            }
                                        }

                                    }
                                    else
                                    {
                                        return true;
                                    }
                                }
                            }
                            else
                            {
                                throw new InvalidMethodNameException("Method name cannot start with number");
                            }
                        }
                        else
                        {
                            throw new InvalidMethodNameException("Method name cannot contain special characters");
                        }

                    }
                    else
                    {
                        throw new InvalidCommandException("Invalid Method Command Syntax");
                    }
                }
                else
                {
                    throw new InvalidCommandException("Invalid Command Name");
                }
            }
            catch (InvalidCommandException e)
            {
                errors.Add(e.Message);
                return false;
            }
            catch (InvalidMethodNameException e)
            {
                errors.Add(e.Message);
                return false;
            }
            catch (InvalidParameterException e)
            {
                errors.Add(e.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool check_methodcall(string command)
        {
            method_signature = ComplexDrawing.getMethodSignature();
            string methodname = command.Split('(')[0];
            methodname = Regex.Replace(methodname, @"\s+", "");
            string parameter_inside_method = command.Trim().Split('(', ')')[1];
            int parameter_count = 0;
            if (parameter_inside_method.Contains(','))
            {
                parameter_count = parameter_inside_method.Split(',').Length;
            }
            else
            {
                parameter_count = parameter_inside_method.Length;
            }

            string signature = methodname + "," + parameter_count;
            //check for method signature;
            try
            {
                if (method_signature.ContainsKey(signature))
                {
                    return true;
                }
                else
                {
                    throw new MethodNotFoundException("method not found");
                }

            }
            catch (MethodNotFoundException e)
            {
                errors.Add(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Check for valid program commands
        /// </summary>
        /// <param name="command">program commands</param>
        /// <returns>true if valid and false if invalid</returns>
        public bool Check_command(string command)
        {
            variable = ComplexDrawing.getVariables();
            string Draw_cmd = command.Split('(')[0].Trim();
            try
            {
                if (Draw_cmd.Equals("moveto") || Draw_cmd.Equals("drawto") || Draw_cmd.Equals("pen") || Draw_cmd.Equals("fill") || Draw_cmd.Equals("circle") || Draw_cmd.Equals("rectangle") || Draw_cmd.Equals("triangle") || Draw_cmd.Equals("polygon"))
                {
                    string value_inside_brackets = null;
                    string[] parameters = null;
                    try
                    {
                        if (command.Split('(', ')').Length > 1)
                        {
                            value_inside_brackets = command.Split('(', ')')[1];
                            value_inside_brackets = Regex.Replace(value_inside_brackets, @"\s+", "");
                            parameters = value_inside_brackets.Split(',');
                        }
                        else
                        {
                            throw new InvalidParameterException("Parameters Missing");
                        }
                    }
                    catch (InvalidParameterException e)
                    {
                        errors.Add(e.Message);
                        return false;
                    }

                    //check moveto and Drawto command
                    if (Draw_cmd.Equals("moveto") || Draw_cmd.Equals("drawto"))
                    {
                        try
                        {
                            if (parameters.Length == 2)
                            {
                                if (!Regex.IsMatch(parameters[0], @"^[0-9]+$"))
                                {
                                    if (!variable.ContainsKey(parameters[0]))
                                    {
                                        throw new VariableNotFoundException("Variable: " + parameters[0] + " doesnot exist");
                                    }
                                }
                                else
                                {
                                    int.Parse(parameters[0]);
                                }

                                if (!Regex.IsMatch(parameters[1], @"^[0-9]+$"))
                                {
                                    if (!variable.ContainsKey(parameters[1]))
                                    {
                                        throw new VariableNotFoundException("Variable: " + parameters[1] + " doesnot exist");
                                    }
                                }
                                else
                                {
                                    int.Parse(parameters[1]);
                                }
                            }
                            else
                            {
                                throw new InvalidParameterException("2 parameters required.");
                            }
                        }
                        catch (FormatException)
                        {
                            errors.Add("X and Y should be in numbers");
                            return false;
                        }
                        catch (InvalidParameterException e)
                        {
                            errors.Add(e.Message);
                            return false;
                        }
                        catch (VariableNotFoundException e)
                        {
                            errors.Add(e.Message);
                            return false;
                        }
                        return true;

                    }
                    //end of check moveto and Drawto command

                    //check pen command
                    if (Draw_cmd.Equals("pen"))
                    {
                        try
                        {
                            if (!value_inside_brackets.Contains(','))
                            {
                                if (value_inside_brackets.Equals("red") || value_inside_brackets.Equals("green") || value_inside_brackets.Equals("blue") || value_inside_brackets.Equals("black") || value_inside_brackets.Equals("orange"))
                                {
                                    return true;
                                }
                                else
                                {
                                    throw new InvalidParameterException("Color not supported.");
                                }
                            }
                            else
                            {
                                throw new InvalidParameterException("Only 1 parameter required.");
                            }
                        }
                        catch (InvalidParameterException e)
                        {
                            errors.Add(e.Message);
                            return false;
                        }
                    }
                    //end of pen command

                    //check fill command
                    if (Draw_cmd.Equals("fill"))
                    {
                        try
                        {
                            if (!value_inside_brackets.Contains(','))
                            {
                                if (value_inside_brackets.Equals("on") || value_inside_brackets.Equals("off"))
                                {
                                    return true;
                                }
                                else
                                {
                                    throw new InvalidParameterException("Invalid fill option.");
                                }
                            }
                            else
                            {
                                throw new InvalidParameterException("Only 1 parameter required.");
                            }
                        }
                        catch (InvalidParameterException e)
                        {
                            errors.Add(e.Message);
                            return false;
                        }
                    }
                    //end fill command

                    //check circle command
                    if (Draw_cmd.Equals("circle"))
                    {
                        try
                        {
                            if (parameters.Length == 1)
                            {
                                if (!Regex.IsMatch(parameters[0], @"^[0-9]+$"))
                                {
                                    if (variable.ContainsKey(parameters[0]))
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        throw new VariableNotFoundException("Variable: " + parameters[0] + " does not exist");
                                    }
                                }
                                else
                                {
                                    int.Parse(parameters[0]);
                                    return true;
                                }
                            }
                            else
                            {
                                throw new InvalidParameterException("Only 1 parameter required.");
                            }

                        }
                        catch (FormatException)
                        {
                            errors.Add("Radius should be in numbers (0-9).");
                            return false;
                        }
                        catch (InvalidParameterException e)
                        {
                            errors.Add(e.Message);
                            return false;
                        }
                        catch (VariableNotFoundException e)
                        {
                            errors.Add(e.Message);
                            return false;
                        }

                    }
                    //end circle command

                    //check circle command
                    if (Draw_cmd.Equals("polygon"))
                    {
                        try
                        {
                            if (parameters.Length % 2 == 0 && parameters.Length >= 4)
                            {
                                foreach (string param in parameters)
                                {
                                    if (!Regex.IsMatch(param, @"^[0-9]+$"))
                                    {
                                        if (!variable.ContainsKey(param))
                                        {
                                            throw new VariableNotFoundException("Variable: " + param + " does not exist");
                                        }
                                    }
                                    else
                                    {
                                        int.Parse(param);
                                    }
                                }
                            }
                            else
                            {
                                throw new InvalidParameterException("Minimum 2 points (4 parameters) needed.");
                            }
                        }
                        catch (FormatException)
                        {
                            errors.Add("Points should be in numbers.");
                            return false;
                        }
                        catch (InvalidParameterException e)
                        {
                            errors.Add(e.Message);
                            return false;
                        }
                        catch (VariableNotFoundException e)
                        {
                            errors.Add(e.Message);
                            return false;
                        }

                        return true;
                    }
                    //end circle command

                    //check rectangle command
                    if (Draw_cmd.Equals("rectangle"))
                    {
                        try
                        {
                            if (parameters.Length == 2)
                            {
                                if (!Regex.IsMatch(parameters[0], @"^[0-9]+$"))
                                {
                                    if (!variable.ContainsKey(parameters[0]))
                                    {
                                        throw new VariableNotFoundException("Variable: " + parameters[0] + " does not exist");
                                    }
                                }
                                else
                                {
                                    int.Parse(parameters[0]);
                                }

                                if (!Regex.IsMatch(parameters[1], @"^[0-9]+$"))
                                {
                                    if (!variable.ContainsKey(parameters[1]))
                                    {
                                        throw new VariableNotFoundException("Variable: " + parameters[1] + " does not exist"); ;
                                    }
                                }
                                else
                                {
                                    int.Parse(parameters[1]);
                                }
                            }
                            else
                            {
                                throw new InvalidParameterException("2 parameters required");
                            }
                        }
                        catch (FormatException)
                        {
                            errors.Add("Width and height should be in numbers.");
                            return false;
                        }
                        catch (InvalidParameterException e)
                        {
                            errors.Add(e.Message);
                            return false;
                        }
                        catch (VariableNotFoundException e)
                        {
                            errors.Add(e.Message);
                            return false;
                        }
                        return true;
                    }
                    //end rectangle command

                    //check triangle command
                    if (Draw_cmd.Equals("triangle"))
                    {
                        try
                        {
                            if (parameters.Length == 3)
                            {
                                foreach (string param in parameters)
                                {
                                    if (!Regex.IsMatch(param, @"^[0-9]+$"))
                                    {
                                        if (!variable.ContainsKey(param))
                                        {
                                            throw new VariableNotFoundException("Variable: " + param + " does not exist");
                                        }
                                    }
                                    else
                                    {
                                        int.Parse(param);
                                    }
                                }
                            }
                            else
                            {
                                throw new InvalidParameterException("3 parameters required");
                            }

                        }
                        catch (FormatException)
                        {
                            errors.Add("Triangle sides should be in numbers.");
                            return false;
                        }
                        catch (InvalidParameterException e)
                        {
                            errors.Add(e.Message);
                            return false;
                        }
                        catch (VariableNotFoundException e)
                        {
                            errors.Add(e.Message);
                            return false;
                        }

                        return true;
                    }
                }
                else
                {
                    throw new InvalidCommandException("Command not valid");
                }
            }
            catch (InvalidCommandException e)
            {
                errors.Add(e.Message);
                return false;
            }
            return false;
        }

        /// <summary>
        /// Stores list of errors found during command validation
        /// </summary>
        /// <returns>returns list of errors</returns>
        public ArrayList error_list()
        {
            return errors;
        }

    }
}
