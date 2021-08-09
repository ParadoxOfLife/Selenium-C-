using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;

namespace Massive_Attack
{
    [TestClass]
    public class Teardrop
    {
        //Global variable so all functions can access them
        public IWebDriver driver;

        string objectForSearch = "summer";
        string userName = "Buffy@VampireSlayer.com";
        string password = "27120589";
        string dress_Name = "Printed Chiffon Dress";
        bool found = false;         //Used for Dress search if not found will give error
        bool payment_method = true; // true = for bank, False = for Check Options
        string Total_Price = "";    //Used to check prices of dress at later stages
        string Shipment_Cost = "";  //Used to check shipment prices later

        [TestInitialize]
        public void Testini()//will run before every test run
        {
            driver = new ChromeDriver();
            driver.Url = "http://automationpractice.com/";
            driver.Manage().Window.Maximize();
            WaitFindElement(driver, By.Id("htmlcontent_home")); //waits for homepage to load
        }

        [TestCleanup]
        public void CleanUp()//will close Chrome after test complete disable if need to keep chrome open
        {
            this.driver.Quit();
        }

        [TestMethod]
        public void Dress_Purchase_valid()
        {
            Search_Query(objectForSearch);  //Step #1 Select Search box and insert Search object(objectForSearch) after moves into Search page
            DressArray(dress_Name);         //Step #2 Make array from results and Select wanted Dress(dress_Name) iframe will open

                driver.SwitchTo().Frame(driver.FindElement(By.ClassName("fancybox-iframe")));           //Switching to ifram 
                IWebElement iframedresscheck = driver.FindElement(By.ClassName("pb-center-column"));    
                IWebElement namecheck = iframedresscheck.FindElement(By.TagName("h1"));
                Assert.IsTrue(namecheck.Text.Contains(dress_Name));                                     //Checking if Dress exist in iframe and if its open

            iFrame_addtocard();             //Step #3  select Add to cart after Newly created iframe closes and return to Default iframe

                IWebElement Total = driver.FindElement(By.Id("layer_cart"));
                IWebElement TotalCost = Total.FindElement(By.ClassName("ajax_block_cart_total"));
                IWebElement TotalShipment = Total.FindElement(By.ClassName("ajax_cart_shipping_cost"));
                Total_Price = TotalCost.Text;                                                           //Holding Total price for later use
                Shipment_Cost = TotalShipment.Text;                                                     //Holding Shipment price for later use
                Assert.IsTrue(Total.Displayed, "layer of cart was not loaded");                         //verify Layer of cart is loaded

            Procced_to_cart();              //Step #4 Pop up will be displayed choose proceed to check out(will take us into Cart)

                IWebElement TotalPriceCheck = driver.FindElement(By.Id("total_price_container"));
                Assert.IsTrue(TotalPriceCheck.Displayed, "Summery Page Failed to load");
                Assert.AreEqual(Total_Price, TotalPriceCheck.Text,"Total price of items did not match");    //verify Price of total dress are correct


            Procced_to_checkout();          //Step #5 inside Shopping cart Press Proceed to checkout(will take us into sing in screen)
            login(userName,password);       //Step #6 Sing in screen will open , insret account details and log in
            Procced_to_checkout();          //Step #7 Adrress and billing verification  will be open Proceed to checkout(Shipping Screen will open)

                IWebElement ShippingPageCheck = driver.FindElement(By.Id("form"));
                IWebElement PriceCheck = ShippingPageCheck.FindElement(By.ClassName("delivery_option_price"));
                Assert.AreEqual(Shipment_Cost, PriceCheck.Text,"Shipment Prices does not add up");               //verify  Shipment cost is correct

            ShippingBoxSelect();            //Step #8 Check box that Agrees to term of service (Box will be marked with v)

                IWebElement BoxCheck = driver.FindElement(By.Id("cgv"));
                Assert.IsTrue(BoxCheck.Selected, "Term of Service Box is not Selected");                        //verify Box is Selected

            Procced_to_checkout();          //Step #9 Only after Step #8 you can click on Proceed to checkout(will take you into Payment page)

                WaitFindElement(driver, By.Id("cart_summary"));
                IWebElement PaymentPageCheck = driver.FindElement(By.Id("cart_summary"));
                IWebElement PriceCheckShipment = PaymentPageCheck.FindElement(By.Id("total_shipping"));
                IWebElement PriceChecktotal = PaymentPageCheck.FindElement(By.Id("total_price_container"));

                Assert.AreEqual(Total_Price, PriceChecktotal.Text,"Total Price of items did not match");            //verify total Price is correct
                Assert.AreEqual(Shipment_Cost, PriceCheckShipment.Text, "Total Shipment Price did not match");      //verify Shipment price is correct

            SelectPayMethod(payment_method);//Step 10 Select payment method Bank or Cheque (will open order summery page)

                WaitFindElement(driver, By.Id("amount"));
                IWebElement OrderSummeryPage = driver.FindElement(By.Id("amount"));
                Assert.IsTrue(OrderSummeryPage.Displayed, "Order Summery Page Did not Load");       //verify Order Summer page loaded
                Assert.AreEqual(Total_Price,OrderSummeryPage.Text,"Total Price Did not match");     //verify Total price is correct


            Procced_to_checkout();          //Step #11 Order details page review details and select confirm my order(Order confirmation page will open)

                WaitFindElement(driver, By.Id("order-confirmation"));
                IWebElement OrderConfirmPage = driver.FindElement(By.Id("order-confirmation"));
                IWebElement PriceCheckConfirm = OrderConfirmPage.FindElement(By.ClassName("price"));

                string priceholder = OrderConfirmPage.Text;
                bool OrderMoney = priceholder.Contains(Total_Price);
                Assert.IsTrue(OrderMoney, "Order Confirm page total price does not match");             //Verify Total price is correct
        }



        public void DressArray(string Dress_Name)//builds array from search object and clicks on it
        {
            WaitFindElement(driver,By.Id("center_column"));//will wait until element by id will load
            IWebElement Collection = driver.FindElement(By.Id("center_column"));
            ReadOnlyCollection<IWebElement> dresses = Collection.FindElements(By.ClassName("ajax_block_product"));

            for (int i = 0; i < dresses.Count; i++)
            {

                IWebElement name = dresses[i].FindElement(By.ClassName("product-name"));
                WaitFindElement(driver, By.ClassName("product-name"));

                if (name.Text == Dress_Name)
                {

                    found = true;
                    IWebElement DressClick = dresses[i].FindElement(By.ClassName("product_img_link"));
                    DressClick.Click();
                    break;

                }

            }
            Assert.IsTrue(found, "Dress was not found");

        }
        public void iFrame_addtocard()//will switch iframes to access cart and click on Add to cart
        {
            
            WaitFindElement(driver, By.Id("add_to_cart"));//will wait until element by id will load
            string PageCheck = driver.FindElement(By.ClassName("pb-center-column")).Text;
            bool check = PageCheck.Contains(dress_Name);
            Assert.IsTrue(check, "Dress name does not match");

            driver.FindElement(By.Id("add_to_cart")).Click();
            WaitFindElement(driver, By.Id("layer_cart"));
            driver.SwitchTo().DefaultContent();
        }
        public void login(string email, string password) //send email and password into login input to sing in
        {
            WaitFindElement(driver, By.Id("email"));//will wait until element by id will load
            IWebElement userName = driver.FindElement(By.Id("email"));
            userName.SendKeys(email);
            IWebElement userPassword = driver.FindElement(By.Id("passwd"));
            userPassword.SendKeys(password);
            driver.FindElement(By.Id("SubmitLogin")).Click();
        }
        public void Search_Query(string name)//this function will search specififed name at search block takes in string
        {
            WaitFindElement(driver, By.Id("search_block_top"));//will wait until element by id will load
            IWebElement Search = driver.FindElement(By.Id("search_block_top"));
            IWebElement SearchKeys = Search.FindElement(By.Id("search_query_top"));
            SearchKeys.SendKeys(name);

            IWebElement ClickSearch = Search.FindElement(By.ClassName("button-search"));
            ClickSearch.Click();
        }
        public void Procced_to_cart()//Function will search and click on Proccesed button outside of cart page
        {
            WaitFindElement(driver, By.Id("layer_cart"));//will wait until element by id will load
            IWebElement CartLayer = driver.FindElement(By.Id("layer_cart"));
            IWebElement InsideCartLayer = CartLayer.FindElement(By.ClassName("icon-chevron-right"));
            InsideCartLayer.Click();
        }
        public void Procced_to_checkout()//this function search for procced button within page only inside a cart
        {
            //store element inside variable to work with them 
            WaitFindElement(driver, By.Id("page"));//will wait until element by id will load
            IWebElement page = driver.FindElement(By.Id("page"));
            IWebElement cartnav = page.FindElement(By.ClassName("cart_navigation"));
            IWebElement icon_chev = cartnav.FindElement(By.ClassName("icon-chevron-right"));
            icon_chev.Click();

        }
        public void ShippingBoxSelect()//will check Terms of Service box in checkout(Shipping tab)
        {
            WaitFindElement(driver,By.Id("cgv"));//will wait until element by id will load
            IWebElement boxcheck = driver.FindElement(By.Id("cgv"));
            boxcheck.Click();
        }
        public void SelectPayMethod(bool payment)
        {
            WaitFindElement(driver, By.Id("HOOK_PAYMENT"));//will wait until element by id will load
            if (payment == true)
            {
                IWebElement bank = driver.FindElement(By.Id("HOOK_PAYMENT")).FindElement(By.ClassName("bankwire"));
                bank.Click();
            }
            else
            {
                IWebElement cheque = driver.FindElement(By.Id("HOOK_PAYMENT")).FindElement(By.ClassName("cheque"));
                cheque.Click();
            }

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
