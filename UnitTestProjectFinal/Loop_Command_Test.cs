using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assessment1;

namespace UnitTestProjectFinal
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class Loop_Command_Test
    {
        Form1 fm = new Form1();

        /// <summary>
        /// Testing for Loop Commands
        /// </summary>
        [TestMethod]
        public void Check_loop_command()
        {
            //set variable
            string[] variable = { "count = 1", "radius = 20" };
            foreach (string var in variable)
            {
                Assert.IsTrue(Check_Valid_Commands.GetInstance.check_variable(var));
                Assert.IsTrue(ComplexDrawing.GetInstance.run_variable_command(var));
            }

            string text = "loop for count <=5\ncircle (radius)\nradius+10\ncount+1\nendloop";
            string[] list_commands = text.Split('\n');
            string loop_command = "loop for count <=5";
            int line_found_in = 1;
            //check if loop command is valid or not
            Assert.IsTrue(Check_Valid_Commands.GetInstance.check_loop(loop_command));
            //check if command run properly or not
            Assert.IsTrue(ComplexDrawing.GetInstance.run_loop_command(loop_command, list_commands, line_found_in, fm));

        }
    }
}

