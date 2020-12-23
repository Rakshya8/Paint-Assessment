using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assessment1;

namespace UnitTestProjectFinal
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class Variable_Test
    {
        Form1 fm = new Form1();

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void Variable_command_and_operations_test()
        {
            string[] text = { "width = 10", "rectangle=50", "height= 5" };
            foreach (string check_text in text)
            {
                Assert.IsTrue(Check_Valid_Commands.GetInstance.check_variable(check_text));
                Assert.IsTrue(ComplexDrawing.GetInstance.run_variable_command(check_text));

            }
            string[] operation_text = { "width+10", "rectangle-20", "height*5", "height/5" };
            foreach (string check_text in operation_text)
            {
                Assert.IsTrue(Check_Valid_Commands.GetInstance.check_variable_operation(check_text));
                Assert.IsTrue(ComplexDrawing.GetInstance.runVariableOperation(check_text, fm));
            }
        }
    }
}
