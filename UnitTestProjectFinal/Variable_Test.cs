using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assessment1;

namespace UnitTestProjectFinal
{
    [TestClass]
    public class Variable_Test
    {
        Check_Valid_Commands cv = new Check_Valid_Commands();
        ComplexDrawing cd = new ComplexDrawing();
        Form1 fm = new Form1();
        [TestMethod]
        public void variable_command_and_operations_test()
        {
            string[] text = { "width = 10", "rectangle=50", "height= 5" };
            foreach (string check_text in text)
            {
                Assert.IsTrue(cv.check_variable(check_text));
                Assert.IsTrue(cd.run_variable_command(check_text));

            }
            string[] operation_text = { "width+10", "rectangle-20", "height*5", "height/5" };
            foreach (string check_text in operation_text)
            {
                Assert.IsTrue(cv.check_variable_operation(check_text));
                Assert.IsTrue(cd.runVariableOperation(check_text, fm));
            }
        }
    }
}
