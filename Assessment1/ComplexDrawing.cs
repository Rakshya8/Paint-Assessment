using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections;

namespace Assessment1
{
    /// <summary>
    /// Contains If, Loop and Method command execution
    /// </summary>
    public class ComplexDrawing
    {
        //store method signatures i.e method name and number of parameters
        static IDictionary<string, ArrayList> methods = new Dictionary<string, ArrayList>();

        //store variables
        static IDictionary<string, int> variable = new Dictionary<string, int>();

        //Opeator used in condition of if command
        static string operators = "";

        //store variable name used during method declaration
        ArrayList method_parameter_variables = new ArrayList();

        private static ComplexDrawing instance = null;
        private ComplexDrawing() { }

        /// <summary>
        /// if instance is null then create a new object of class
        /// </summary>
        public static ComplexDrawing GetInstance
        {
            get
            {
                if (instance == null)
                    instance = new ComplexDrawing();
                return instance;
            }
        }

        /// <summary>
        /// Collection of key/pair i.e. name/parameter.count
        /// </summary>
        /// <returns>method signatures</returns>
        public static IDictionary<string, ArrayList> getMethodSignature()
        {
            return methods;
        }

        /// <summary>
        /// Clear methods, variables
        /// </summary>
        public void Clear_list()
        {
            methods.Clear();
            variable.Clear();
            method_parameter_variables.Clear();

        }

        /// <summary>
        /// execute if command
        /// </summary>
        /// <param name="Draw">if command line</param>
        /// <param name="lines">all commands entered by user</param>
        /// <param name="line_found_in">if command found line</param>
        /// <param name="fm">Object of Form1</param>
        public bool run_if_command(string Draw, string[] lines, int line_found_in, Form1 fm)
        {
            //check for endif
            int if_end_tag_exist = 0;
            for (int a = line_found_in; a < lines.Length; a++)
            {
                if (lines[a].Equals("endif"))
                {
                    if_end_tag_exist++;
                }
            }
            if (if_end_tag_exist == 0 && !lines[line_found_in].Equals("then"))
            {
                fm.textBox2.AppendText("Error: If statement not closed.");
                return false;
            }

            //get conditional operator
            operators = Check_Valid_Commands.getOperator();
            //split line 
            string condition = Draw.Split('(', ')')[1].Trim();
            string[] splitCondition = condition.Split(new string[] {
                    operators
                  },
            StringSplitOptions.RemoveEmptyEntries);
            try
            {

                if (splitCondition.Length == 2)
                {
                    string conditionKey = splitCondition[0].Trim();
                    int conditionValue = int.Parse(splitCondition[1].Trim());
                    if (variable.ContainsKey(conditionKey))
                    {
                        int variableValue = 0;
                        variable.TryGetValue(conditionKey, out variableValue);

                        bool conditionStatus = false;

                        int value1 = variableValue; //variable
                        int value2 = conditionValue; // 10 20 30

                        if (operators == "<=")
                        {
                            if (value1 <= value2) conditionStatus = true;
                        }
                        else if (operators == ">=")
                        {
                            if (value1 >= value2) conditionStatus = true;
                        }
                        else if (operators == "==")
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

                        //if condition is true
                        if (conditionStatus)
                        {
                            if (lines[line_found_in].Equals("then"))
                            {
                                //check type of command
                                string command_type = Check_Valid_Commands.GetInstance.check_command_type(lines[line_found_in + 1]);
                                if (command_type.Equals("drawing_commands"))
                                {
                                    if (Check_Valid_Commands.GetInstance.Check_command(lines[line_found_in + 1]))
                                    {
                                        fm.draw_commands(lines[line_found_in + 1]);
                                    }
                                }
                            }
                            else
                            {
                                for (int i = (line_found_in); i < lines.Length; i++)
                                {
                                    if (!(lines[i].Equals("endif")))
                                    {
                                        string command_type = Check_Valid_Commands.GetInstance.check_command_type(lines[i]);
                                        if (command_type.Equals("drawing_commands"))
                                        {
                                            if (Check_Valid_Commands.GetInstance.Check_command(lines[i]))
                                            {
                                                fm.draw_commands(lines[i]);
                                            }
                                        }
                                        else if (command_type.Equals("variableoperation"))
                                        {
                                            if (Check_Valid_Commands.GetInstance.check_variable_operation(lines[i]))
                                            {
                                                runVariableOperation(lines[i], fm);
                                            }
                                        }
                                        else
                                        {
                                            fm.textBox2.AppendText("\n Command: (" + lines[i] + ") not supported.");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }

                                }
                            }

                        }
                    }
                    else
                    {
                        throw new VariableNotFoundException("Variable: " + conditionKey + " does not exist");
                    }

                }
                else
                {
                    throw new InvalidParameterException("Syntax error in if condition.");
                }
            }
            catch (InvalidParameterException e)
            {
                fm.textBox2.AppendText(e.Message);
                return false;
            }
            catch (VariableNotFoundException e)
            {
                fm.textBox2.AppendText(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// execute loop command
        /// </summary>
        /// <param name="Draw">loop command line</param>
        /// <param name="lines">all commands entered by user</param>
        /// <param name="loop_found_in_line">loop command found line</param>
        /// <param name="fm">Object of Form1</param>
        public bool run_loop_command(string Draw, string[] lines, int loop_found_in_line, Form1 fm)
        {
            int loop_end_tag_exist = 0;
            for (int a = loop_found_in_line; a < lines.Length; a++)
            {
                if (lines[a].Equals("endloop"))
                {
                    loop_end_tag_exist++;
                }
            }
            if (loop_end_tag_exist == 0)
            {
                fm.textBox2.AppendText("Error: Loop not closed.");
                return false;
            }

            string[] store_command = Draw.Split(new string[] { "for" }, StringSplitOptions.RemoveEmptyEntries);
            int loop_val = 0;
            int counter = 0;
            string[] loop_condition = store_command[1].Split(new string[] { "<=", ">=", "<", ">" }, StringSplitOptions.RemoveEmptyEntries);
            string variable_name = loop_condition[0].ToLower().Trim();
            int loopValue = int.Parse(loop_condition[1].Trim());
            ArrayList cmds = new ArrayList();
            if (variable.ContainsKey(variable_name))
            {
                variable.TryGetValue(variable_name, out loop_val);

                for (int i = (loop_found_in_line); i < lines.Length; i++)
                {
                    if (!(lines[i].Equals("endloop")))
                    {
                        cmds.Add(lines[i]);
                    }
                    else
                    {
                        break;
                    }
                    if ((lines[i].Contains(variable_name + "+") || lines[i].Contains(variable_name + "-") || lines[i].Contains(variable_name + "*") || lines[i].Contains(variable_name + "/")))
                    {

                        counter++;
                    }
                }

                if (counter == 0)
                {
                    fm.textBox2.AppendText("Counter variable not handled");
                    return false;
                }

                if (store_command[1].Contains("<="))
                {
                    if (loop_val >= loopValue)
                    {
                        fm.textBox2.AppendText("Variable " + variable_name + " should be smaller than " + loopValue);
                        return false;
                    }
                    while (loop_val <= loopValue)
                    {
                        foreach (string cmd in cmds)
                        {
                            string command_type = Check_Valid_Commands.GetInstance.check_command_type(cmd);

                            if (command_type.Equals("drawing_commands"))
                            {
                                if (Check_Valid_Commands.GetInstance.Check_command(cmd))
                                {
                                    fm.draw_commands(cmd);
                                }
                            }
                            else if (command_type.Equals("variableoperation"))
                            {
                                if (Check_Valid_Commands.GetInstance.check_variable_operation(cmd))
                                {
                                    runVariableOperation(cmd, fm);
                                }
                            }
                            else
                            {
                                fm.textBox2.AppendText("\n Command: (" + cmd + ") not supported.");
                                return false;
                            }
                        }
                        variable.TryGetValue(variable_name, out loop_val);
                    }
                }
                else if (store_command[1].Contains(">="))
                {
                    if (loop_val <= loopValue)
                    {
                        fm.textBox2.AppendText("Variable " + variable_name + " should be greater than " + loopValue);
                        return false;
                    }
                    while (loop_val >= loopValue)
                    {

                        foreach (string cmd in cmds)
                        {
                            string command_type = Check_Valid_Commands.GetInstance.check_command_type(cmd);

                            if (command_type.Equals("drawing_commands"))
                            {
                                if (Check_Valid_Commands.GetInstance.Check_command(cmd))
                                {
                                    fm.draw_commands(cmd);
                                }
                            }
                            else if (command_type.Equals("variableoperation"))
                            {
                                if (Check_Valid_Commands.GetInstance.check_variable_operation(cmd))
                                {
                                    runVariableOperation(cmd, fm);
                                }
                            }
                            else
                            {
                                fm.textBox2.AppendText("\n Command: (" + cmd + ") not supported.");
                                return false;
                            }
                        }
                        variable.TryGetValue(variable_name, out loop_val);
                    }
                }
                else if (store_command[1].Contains(">"))
                {
                    if (loop_val < loopValue)
                    {
                        fm.textBox2.AppendText("Variable " + variable_name + " should be greater than " + loopValue);
                        return false;
                    }
                    while (loop_val > loopValue)
                    {
                        foreach (string cmd in cmds)
                        {
                            string command_type = Check_Valid_Commands.GetInstance.check_command_type(cmd);

                            if (command_type.Equals("drawing_commands"))
                            {
                                if (Check_Valid_Commands.GetInstance.Check_command(cmd))
                                {
                                    fm.draw_commands(cmd);
                                }
                            }
                            else if (command_type.Equals("variableoperation"))
                            {
                                if (Check_Valid_Commands.GetInstance.check_variable_operation(cmd))
                                {
                                    runVariableOperation(cmd, fm);
                                }
                            }
                            else
                            {
                                fm.textBox2.AppendText("\n Command: (" + cmd + ") not supported.");
                                return false;
                            }
                        }
                        variable.TryGetValue(variable_name, out loop_val);
                    }
                }
                else if (store_command[1].Contains("<"))
                {
                    if (loop_val > loopValue)
                    {
                        fm.textBox2.AppendText("Variable " + variable_name + " should be smaller than " + loopValue);
                        return false;
                    }
                    while (loop_val < loopValue)
                    {
                        foreach (string cmd in cmds)
                        {
                            string command_type = Check_Valid_Commands.GetInstance.check_command_type(cmd);

                            if (command_type.Equals("drawing_commands"))
                            {
                                if (Check_Valid_Commands.GetInstance.Check_command(cmd))
                                {
                                    fm.draw_commands(cmd);
                                }
                            }
                            else if (command_type.Equals("variableoperation"))
                            {
                                if (Check_Valid_Commands.GetInstance.check_variable_operation(cmd))
                                {
                                    runVariableOperation(cmd, fm);
                                }
                            }
                            else
                            {
                                fm.textBox2.AppendText("\n Command: (" + cmd + ") not supported.");
                                return false;
                            }
                        }
                        variable.TryGetValue(variable_name, out loop_val);
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// execute method command
        /// </summary>
        /// <param name="Draw">method command line</param>
        /// <param name="lines">all commands entered by user</param>
        /// <param name="method_found_in_line">loop command found line</param>
        /// <param name="fm">Object of Form1</param>
        public bool run_method_command(string Draw, string[] lines, int method_found_in_line, Form1 fm)
        {
            int method_end_tag_exist = 0;
            for (int a = method_found_in_line; a < lines.Length; a++)
            {
                if (lines[a].Equals("endmethod"))
                {
                    method_end_tag_exist++;
                }
            }
            if (method_end_tag_exist == 0)
            {
                fm.textBox2.AppendText("Error: Method not closed.");
                return false;
            }

            //method          
            string[] command_part = Draw.Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries);
            string inside_brackets = command_part[1];
            inside_brackets = Regex.Replace(inside_brackets, @"\s+", "");
            string cmd = command_part[0] + "(" + inside_brackets;


            string[] command_parts = cmd.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string method_name = command_parts[1].Trim();
            method_name = Regex.Replace(method_name, @"\s+", "");
            int parameter_count = 0;
            string parameter_inside_method = command_parts[2].Trim().Split('(', ')')[1];
            ArrayList commands_inside_method = new ArrayList();
            for (int i = method_found_in_line; i < lines.Length; i++)
            {
                if (!lines[i].Equals("endmethod"))
                {
                    commands_inside_method.Add(lines[i]);
                }
                else
                {
                    break;
                }
            }
            if (parameter_inside_method.Contains(','))
            {
                parameter_count = parameter_inside_method.Split(',').Length;
                foreach (string variable_name in parameter_inside_method.Split(','))
                {
                    method_parameter_variables.Add(variable_name);
                }
            }
            else
            {
                if (parameter_inside_method.Length > 0)
                {
                    parameter_count = 1;
                    method_parameter_variables.Add(parameter_inside_method);
                }
                else
                {
                    parameter_count = 0;
                }
            }
            string signature = method_name + "," + parameter_count;
            if (!methods.ContainsKey(signature))
            {
                methods.Add(signature, commands_inside_method);
            }
            else
            {
                fm.textBox2.AppendText("Method already exist in line " + method_found_in_line);
            }
            return true;
        }


        /// <summary>
        /// Execute method call command
        /// </summary>
        /// <param name="Draw">method call line</param>
        /// <param name="fm">Object of Form1</param>
        public bool run_method_call(string Draw, Form1 fm)
        {
            string methodname = Draw.Split('(')[0];
            methodname = Regex.Replace(methodname, @"\s+", "");
            string parameter_inside_method = Draw.Trim().Split('(', ')')[1];
            int parameter_count = 0;
            if (parameter_inside_method.Contains(','))
            {
                parameter_count = parameter_inside_method.Split(',').Length;
                for (int i = 0; i < parameter_count; i++)
                {
                    if (!variable.ContainsKey((string)method_parameter_variables[i]))
                    {
                        variable.Add((string)method_parameter_variables[i], int.Parse(parameter_inside_method.Split(',')[i]));
                    }
                    else
                    {
                        variable[(string)method_parameter_variables[i]] = int.Parse(parameter_inside_method.Split(',')[i]);
                    }
                }
            }
            else
            {
                if (parameter_inside_method.Length > 0)
                {
                    parameter_count = 1;
                    if (!variable.ContainsKey((string)method_parameter_variables[0]))
                    {
                        variable.Add((string)method_parameter_variables[0], int.Parse(parameter_inside_method));
                    }
                    else
                    {
                        variable[(string)method_parameter_variables[0]] = int.Parse(parameter_inside_method);
                    }
                }
                else
                {
                    parameter_count = 0;
                }
            }
            string signature = methodname.Trim() + "," + parameter_count;

            ArrayList commands = new ArrayList();

            methods.TryGetValue(signature, out commands);
            foreach (string cmd in commands)
            {
                string command_type = Check_Valid_Commands.GetInstance.check_command_type(cmd);
                if (command_type.Equals("drawing_commands"))
                {
                    if (Check_Valid_Commands.GetInstance.Check_command(cmd))
                    {
                        fm.draw_commands(cmd);
                    }
                }
                else if (command_type.Equals("variableoperation"))
                {
                    if (Check_Valid_Commands.GetInstance.check_variable_operation(cmd))
                    {
                        runVariableOperation(cmd, fm);
                    }
                }
                else
                {
                    fm.textBox2.AppendText("\n Command: (" + cmd + ") not supported.");
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// To get the variables
        /// </summary>
        /// <returns>variables and their values</returns>
        public static IDictionary<string, int> getVariables()
        {
            return variable;
        }

        /// <summary>
        /// Execute variable command
        /// </summary>
        /// <param name="Draw">variable line</param>
        public bool run_variable_command(string Draw)
        {
            Draw = Regex.Replace(Draw, @"\s+", "");
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
            return true;
        }

        /// <summary>
        /// Execute variable operation
        /// </summary>
        /// <param name="line">variable operation line</param>
        /// <param name="fm">Object of Form1</param>
        /// <returns>true if operation run successfully otherwise false</returns>
        public bool runVariableOperation(string line, Form1 fm)
        {
            try
            {
                line = Regex.Replace(line, @"\s+", "");
                string[] variables = line.Split(new Char[] { '+', '-', '*', '/' }, StringSplitOptions.RemoveEmptyEntries);
                int number_of_operator = 0;
                if (variables.Length != 2)
                {
                    return false;
                }

                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i].Equals('+') || line[i].Equals('-') || line[i].Equals('*') || line[i].Equals('/'))
                    {
                        number_of_operator++;
                    }
                }
                if (number_of_operator > 1)
                {
                    return false;
                }
                string vrKey = variables[0].Trim();
                string vrValue = variables[1].Trim();
                int vrValuenum = Int32.Parse(vrValue);
                int dictValue = 0;

                if (variable.ContainsKey(vrKey))
                {
                    variable.TryGetValue(vrKey, out dictValue);
                    if (line.Contains("+"))
                    {
                        variable[vrKey] = dictValue + vrValuenum;
                    }
                    else if (line.Contains("-"))
                    {
                        variable[vrKey] = dictValue - vrValuenum;
                    }
                    else if (line.Contains("*"))
                    {
                        variable[vrKey] = dictValue * vrValuenum;
                    }
                    else if (line.Contains("/"))
                    {
                        variable[vrKey] = dictValue / vrValuenum;
                    }
                }
                else
                {
                    throw new VariableNotFoundException("Variable: " + vrKey + "doesnot exist");
                }
            }
            catch (VariableNotFoundException e)
            {
                fm.textBox2.AppendText(e.Message);
                return false;
            }
            return true;
        }
    }
}
