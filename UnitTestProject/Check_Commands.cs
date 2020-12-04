using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assessment1;

namespace UnitTestProject
{
    [TestClass]
    public class Check_Commands
    {
        Check_Valid_Commands cv = new Check_Valid_Commands();
        [TestMethod]
        public void valid_command_parameter()
        {
            //valid commands
            Assert.IsTrue(cv.Valid_execute_command("run"));
            Assert.IsTrue(cv.Valid_execute_command("clear"));
            Assert.IsTrue(cv.Valid_execute_command("reset"));

            //valid command
            Assert.IsTrue(cv.Check_command("circle(20)"));
            Assert.IsTrue(cv.Check_command("rectangle(20,30)"));
            Assert.IsTrue(cv.Check_command("triangle(20,40,50)"));
            Assert.IsTrue(cv.Check_command("pen(red)"));
            Assert.IsTrue(cv.Check_command("fill(on)"));
            Assert.IsTrue(cv.Check_command("moveto(20,30)"));
            Assert.IsTrue(cv.Check_command("drawto(50,70)"));
        }
        [TestMethod]
        public void valid_parameter()
        {
            //command with wrong number of parameters or invalid parameters
            Assert.IsFalse(cv.Check_command("circle(20,3x)"));
            Assert.IsFalse(cv.Check_command("rectangle(2t,30,40)"));
            Assert.IsFalse(cv.Check_command("triangle(40,50)"));
            Assert.IsFalse(cv.Check_command("pen(nocolor)"));
            Assert.IsFalse(cv.Check_command("fill(on,off)"));
            Assert.IsFalse(cv.Check_command("moveto(20,y)"));
            Assert.IsFalse(cv.Check_command("drawto(xx,70)"));          
        }
        [TestMethod]
        public void valid_command_name()
        {
            //invalid command name
            Assert.IsFalse(cv.Check_command("circ(20)"));
            Assert.IsFalse(cv.Check_command("traing(20,40,50)"));
            Assert.IsFalse(cv.Check_command("rectanglesc(20,30)"));
            Assert.IsFalse(cv.Check_command("pn(red)"));
            Assert.IsFalse(cv.Check_command("filsl(on)"));
            Assert.IsFalse(cv.Check_command("movto(20,30)"));
            Assert.IsFalse(cv.Check_command("drwto(50,70)"));
        } 
        

        }
    }
