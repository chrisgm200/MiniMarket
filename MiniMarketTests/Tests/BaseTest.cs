using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MiniMarketTests.Tests
{
    public class BaseTest
    {
        protected IWebDriver driver;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--ignore-certificate-errors"); // Por si acaso Chrome intenta validar SSL
            options.AddArgument("--disable-web-security");

            driver = new ChromeDriver(options);

            // URL correcta según tu ejecución de dotnet run
            driver.Navigate().GoToUrl("http://localhost:5088");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
