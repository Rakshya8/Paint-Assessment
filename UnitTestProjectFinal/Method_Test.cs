using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assessment1;

namespace UnitTestProjectFinal
{
    [TestClass]
    public class Method_Test
    {
        Check_Valid_Commands cv = new Check_Valid_Commands();
        ComplexDrawing cd = new ComplexDrawing();
        Form1 fm = new Form1();
        [TestMethod]
        public void check_method_command()
        {
            string[] method_text = { "method test ()\ncircle (100)\nendmethod", "method test1 (radius, width, height)\ncircle (radius)\nrectangle (width, height)\nendmethod" };
            string method_line = "";
            int method_found_in = 1;
            for (int i = 0; i < method_text.Length; i++)
            {
                if (i == 0)
                {
                    method_line = "method test ()";
                }
                else
                {
                    method_line = "method test1 (radius, width, height)";
                }
                string[] commands = method_text[i].Split('\n');
                Assert.IsTrue(cv.check_method(method_line));
                Assert.IsTrue(cd.run_method_command(method_line, commands, method_found_in, fm));

                //method call                
            }
            string[] method_call = { "test ()", "test1 (10,20,30)" };
            foreach (string cmd in method_call)
            {
                Assert.IsTrue(cv.check_methodcall(cmd));
                Assert.IsTrue(cd.run_method_call(cmd, fm));
            }

        }
    }
}
