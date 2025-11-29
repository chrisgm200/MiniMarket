using MiniMarketTests.Tests;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace MiniMarketWebApp.MiniMarketTests.Tests.Auth
{
    public class LoginTest : BaseTest
    {
        [Category("Auth")]
        [Test]
        public void Login_Administrador_DebeIngresarCorrectamente()
        {
            // 1. Ir a la pantalla de login
            driver.Navigate().GoToUrl("http://localhost:5088/Auth/Login");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 2. Completar formulario
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email")))
                .SendKeys("admin@minimarket.local");

            driver.FindElement(By.Name("password"))
                .SendKeys("Admin123!");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // 3. Validar ingreso
            wait.Until(ExpectedConditions.UrlContains("/"));

            Assert.That(driver.Url.Contains("/"), Is.True);
        }
    }
}
