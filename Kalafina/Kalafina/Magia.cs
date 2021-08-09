using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace Kalafina
{
    [TestClass]
    public class Magia
    {
        private IWebDriver driver;
        private MainPage main;
        //variables for text fields in account creating page
        private string _FirstName = "James",_LastName = "Smith",_email = "12332@1233.com",_Password = "27120589",_Company ="EverGreen",_Address ="Somewhere 123";
        private string _AdressOptio = "IBelong 23",_City ="InMiddleOf",_ZipCode = "25123",_AddiInfo ="And World is turnin",_HomePhone ="534546",_Mobile ="3463434",_AddresRef = "iThinkNot";

        [TestInitialize]
        public void StartUp()
        {
            //things will run before every test
            driver = new ChromeDriver();
            driver.Url = "http://automationpractice.com/";
            main = new MainPage(driver);
            driver.Manage().Window.Maximize();
        }

        [TestMethod]
        public void TestMethod1()
        {
            //1.Enter Website Homepage and Press Sing in => will lead into AuthPage
            //2.At AuthPage insert text into email at create an account => will lead into account creating page(AccCreatePage)
            //3.in Account creating page fills all fields and click Register => account is created and will lead into Account page

            //Click on sing in
            AuthPage authPage= main.Singin();
            //fill email field with email
            WaitFindElement(driver, By.Id("email_create"));
            AccCreatePage accCreatePage = authPage.CreateAcc(_email);

            //PERSONAL INFORMATION Fields
            accCreatePage.ClickField("uniform-id_gender1");
            accCreatePage.Fillfield("customer_firstname", _FirstName);
            accCreatePage.Fillfield("customer_lastname", _LastName);

            accCreatePage.Fillfield("passwd", _Password);
            //Selecting from combobox options
            accCreatePage.SelectField("days", "16");
            accCreatePage.SelectField("months", "12");
            accCreatePage.SelectField("years", "1991");

            accCreatePage.ClickField("newsletter");
            accCreatePage.ClickField("uniform-optin");

            //YOUR ADDRESS fields 
            accCreatePage.Fillfield("firstname", _FirstName);
            accCreatePage.Fillfield("lastname", _LastName);
            accCreatePage.Fillfield("company", _Company);
            accCreatePage.Fillfield("address1", _Address);
            accCreatePage.Fillfield("address2", _AdressOptio);
            accCreatePage.Fillfield("city", _City);

            //Selecting from combobox options
            accCreatePage.SelectField("id_state", "9");

            accCreatePage.Fillfield("postcode", _ZipCode);

            //Selecting from combobox options
            accCreatePage.SelectField("id_country", "21");

            accCreatePage.Fillfield("other", _AddiInfo);
            accCreatePage.Fillfield("phone", _HomePhone);
            accCreatePage.Fillfield("phone_mobile", _Mobile);
            accCreatePage.Fillfield("alias", _AddresRef);

            //Clicks on Registrate Button
            accCreatePage.ClickField("submitAccount");

            //Assert on Account First and Last name gaven on registretion
            IWebElement AccPagecheck = driver.FindElement(By.CssSelector("a.account"));
            Assert.AreEqual(_FirstName +" "+ _LastName,AccPagecheck.Text);
        }
        public static IWebElement WaitFindElement(IWebDriver driver, By by, int timeoutInSeconds = 10)//this function waits for searched element if not found within given time gives error
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }
    }
}
