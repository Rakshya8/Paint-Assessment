using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assessment1;

namespace UnitTestProjectFinal
{
    [TestClass]
    public class If_Command_Test
    {
        Check_Valid_Commands cv = new Check_Valid_Commands();
        ComplexDrawing cd = new ComplexDrawing();
        Form1 fm = new Form1();
        [TestMethod]
        public void check_if_command()
        {
            //set variables
            string variable = "radius = 10";
            Assert.IsTrue(cv.check_variable(variable));
            Assert.IsTrue(cd.run_variable_command(variable));

            string[] text = { "if (radius==10)\nthen\ncircle (radius)", "if (radius==10)\ncircle (radius)\nendif" };
            foreach (string if_type in text)
            {
                string[] list_commands = if_type.Split('\n');
                string if_command = "if (radius==10)";
                int line_found_in = 1;
                //check if command is If command or not
                Assert.IsTrue(cv.check_if_command(if_command));
                //check if command run properly or not
                Assert.IsTrue(cd.run_if_command(if_command, list_commands, line_found_in, fm));
            }
        }
    }
}

