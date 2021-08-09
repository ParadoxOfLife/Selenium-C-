using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AnotherLifeCycle
{
    [TestClass]
    public class HappyHappyJoyJoy
    {
        public IWebDriver driver; //variable for all class to use

        // we will use this User for Testing ==> UserName(email) = Buffy@VampireSlayer.com  Password = 27120589

        [TestMethod]
        public void LoginValid()//This test is to validate Login is working for baseline
        {
            login("Buffy@VampireSlayer.com","27120589");
            loginCheck();
            Cleanup();
        }

        [TestMethod]
        public void LoginWrongPass()//Login1 is test with incorrect password (Username(email) & user password)
        {

            login("Buffy@VampireSlayer.com", "27121111");
            
            bool A = InvalidLogIn("").Contains("Authentication failed");
            Assert.IsTrue(A,"There is Problem with Invalid Log in");
            Cleanup();

        }
        [TestMethod]
        public void LoginEmptyPass()//Login2 is test without password  (Username(Email) only)
        {
            login("Buffy@VampireSlayer.com","");
            bool A = InvalidLogIn("").Contains("Password is required.");
            Assert.IsTrue(A, "There is Problem with Invalid Log in");
            Cleanup();
        }

        public void login(string email,string password) //login is Basic function used to save space with less mistakes
        {
            driver = new ChromeDriver();
            driver.Url = "http://automationpractice.com/";
            //Find log in element and click on it
            IWebElement singIn = driver.FindElement(By.ClassName("login"));
            singIn.Click();
            //Find Email field and insert 'Username'(Email) into it
            IWebElement userName = driver.FindElement(By.Id("email"));
            userName.SendKeys(email);
            //find Password field and insert 'Password' into it
            IWebElement userPassword = driver.FindElement(By.Id("passwd"));
            userPassword.SendKeys(password);
            //find log in button and click on it
            IWebElement buttonlogin = driver.FindElement(By.Id("SubmitLogin"));
            buttonlogin.Click();

        }
        public void loginCheck()//this function check if login was successful
        {
                    IWebElement AccountPage = driver.FindElement(By.Id("center_column"));
                    IWebElement AccountPage1 = AccountPage.FindElement(By.ClassName("page-heading"));
                    Assert.AreEqual("MY ACCOUNT", AccountPage1.Text, "Valid Log in failed");
                    this.driver.Quit();
        }
        public string InvalidLogIn(string error)//used this function to pull text of error
        {
                IWebElement Singin1 = driver.FindElement(By.Id("center_column"));
                IWebElement Singin2 = Singin1.FindElement(By.ClassName("alert-danger"));

                error = Singin2.Text;
                return error;
        }

        public void Cleanup()//just to close browser after test
        {
            this.driver.Quit();
        }
    }
}
