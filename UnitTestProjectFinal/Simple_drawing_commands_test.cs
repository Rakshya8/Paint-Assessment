using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assessment1;

namespace UnitTestProjectFinal
{
    /// <summary>
    /// Test Class
    /// </summary>
    [TestClass]
    public class Simple_drawing_commands_test
    {
        /// <summary>
        /// Check validitiy of executing commands
        /// </summary>
        [TestMethod]
        public void check_executing_command()
        {
            string[] commands = { "run", "clear", "reset" };
            foreach (string cmd in commands)
            {
                Assert.IsTrue(Check_Valid_Commands.GetInstance.Valid_execute_command(cmd));
            }
        }

        /// <summary>
        /// Check moveto command validity
        /// </summary>
        [TestMethod]
        public void check_moveto_command()
        {
            //valid command with valid parameters
            Assert.IsTrue(Check_Valid_Commands.GetInstance.Check_command("moveto(20,30)"));
            //invalid parameter
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("moveto(20,y)"));
            //invalid command name
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("movto(20,30)"));
        }

        /// <summary>
        /// Check drawto command validity
        /// </summary>
        [TestMethod]
        public void check_drawto_command()
        {
            //valid command with valid parameters
            Assert.IsTrue(Check_Valid_Commands.GetInstance.Check_command("drawto(20,30)"));
            //invalid parameter
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("drawto(20,y)"));
            //invalid command name
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("drwtos(20,30)"));
        }

        /// <summary>
        /// Check fill command validity
        /// </summary>
        [TestMethod]
        public void check_fill_command()
        {
            //valid command with valid parameter
            Assert.IsTrue(Check_Valid_Commands.GetInstance.Check_command("fill(on)"));
            //invalid parameter
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("fill(on,off)"));
            //invalid command name
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("filsl(on)"));

        }

        /// <summary>
        /// Check pen command validity
        /// </summary>
        [TestMethod]
        public void check_pen_command()
        {
            //valid command with valid parameter
            Assert.IsTrue(Check_Valid_Commands.GetInstance.Check_command("pen(red)"));
            //invalid parameter
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("pen(nocolor)"));
            //invalid command name
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("pn(red)"));
        }

        /// <summary>
        /// Check rectangle shape command validity
        /// </summary>
        [TestMethod]
        public void check_rectangle_command()
        {
            //valid command with valid parameter
            Assert.IsTrue(Check_Valid_Commands.GetInstance.Check_command("rectangle(20,30)"));
            //invalid parameter
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("rectangle(2t,30,40)"));
            //invalid command name
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("rectanglesc(20,30)"));
        }

        /// <summary>
        /// Check triangle shape command validity
        /// </summary>
        [TestMethod]
        public void check_triangle_command()
        {
            //valid command with valid parameter
            Assert.IsTrue(Check_Valid_Commands.GetInstance.Check_command("triangle(20,40,50)"));
            //invalid parameter
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("triangle(40,50)"));
            //invalid command name
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("traing(20,40,50)"));

        }

        /// <summary>
        /// Check Polygon shape command validity
        /// </summary>
        [TestMethod]
        public void check_polygon_command()
        {
            //valid command with valid parameter
            Assert.IsTrue(Check_Valid_Commands.GetInstance.Check_command("polygon(20,40,50,50)"));
            //Invalid parameter length
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("polygon(40,50)"));
            //invalid command name
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("polgon(20,40,50)"));
        }

        /// <summary>
        /// Check Rotate command validity
        /// </summary>
        [TestMethod]
        public void check_rotate_command()
        {
            //valid command with valid parameter
            Assert.IsTrue(Check_Valid_Commands.GetInstance.Check_command("rotate(20)"));
            //Invalid parameter
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("rotate(xy, 290)"));
            //Invalid command name
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("rotae(120)"));
        }

        /// <summary>
        /// Check Circle shape command validity
        /// </summary>
        [TestMethod]
        public void check_circle_command()
        {
            //valid command with valid parameter
            Assert.IsTrue(Check_Valid_Commands.GetInstance.Check_command("circle(20)"));
            //Invalid parameter
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("circle(20,3x)"));
            //Invalid command name
            Assert.IsFalse(Check_Valid_Commands.GetInstance.Check_command("circ(20)"));

        }

    }
}

