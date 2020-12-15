using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assessment1;

namespace UnitTestProjectFinal
{
    [TestClass]
    public class Loop_Command_Test
    {
        Check_Valid_Commands cv = new Check_Valid_Commands();
        ComplexDrawing cd = new ComplexDrawing();
        Form1 fm = new Form1();
        [TestMethod]
        public void check_loop_command()
        {
            //set variable
            string[] variable = { "count = 1", "radius = 20" };
            foreach (string var in variable)
            {
                Assert.IsTrue(cv.check_variable(var));
                Assert.IsTrue(cd.run_variable_command(var));
            }

            string text = "loop for count <=5\ncircle (radius)\nradius+10\ncount+1\nendloop";
            string[] list_commands = text.Split('\n');
            string loop_command = "loop for count <=5";
            int line_found_in = 1;
            //check if loop command is valid or not
            Assert.IsTrue(cv.check_loop(loop_command));
            //check if command run properly or not
            Assert.IsTrue(cd.run_loop_command(loop_command, list_commands, line_found_in, fm));

        }
    }
}

