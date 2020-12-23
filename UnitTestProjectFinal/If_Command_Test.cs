using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assessment1;

namespace UnitTestProjectFinal
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class If_Command_Test
    {
        Form1 fm = new Form1();

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void check_if_command()
        {
            //set variables
            string variable = "radius = 10";
            Assert.IsTrue(Check_Valid_Commands.GetInstance.check_variable(variable));
            Assert.IsTrue(ComplexDrawing.GetInstance.run_variable_command(variable));

            string[] text = { "if (radius==10)\nthen\ncircle (radius)", "if (radius==10)\ncircle (radius)\nendif" };
            foreach (string if_type in text)
            {
                string[] list_commands = if_type.Split('\n');
                string if_command = "if (radius==10)";
                int line_found_in = 1;
                //check if command is If command or not
                Assert.IsTrue(Check_Valid_Commands.GetInstance.check_if_command(if_command));
                //check if command run properly or not
                Assert.IsTrue(ComplexDrawing.GetInstance.run_if_command(if_command, list_commands, line_found_in, fm));
            }
        }
    }
}

