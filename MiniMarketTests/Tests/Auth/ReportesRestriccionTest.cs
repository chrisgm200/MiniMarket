using MiniMarketTests.Tests;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace MiniMarketWebApp.MiniMarketTests.Tests.Auth
{
    public class ReportesRestriccionTest : BaseTest
    {
        [Category("Auth")]
        [Test]
        public void Cajero_NoDebe_Acceder_A_Reportes()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 1. Ir a Login
            driver.Navigate().GoToUrl("http://localhost:5088/Auth/Login");

            // 2. Iniciar sesión como CAJERO
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email")))
                .SendKeys("cajero@minimarket.local");

            driver.FindElement(By.Name("password"))
                .SendKeys("Cajero123!");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // 3. Intentar acceder a Reportes directamente
            driver.Navigate().GoToUrl("http://localhost:5088/Reportes");

            // 4. Validar que NO accede y es redirigido al AccessDenied
            wait.Until(ExpectedConditions.UrlContains("/Auth/AccessDenied"));

            string currentUrl = driver.Url;

            Assert.That(
                currentUrl.Contains("/Auth/AccessDenied"),
                Is.True,
                "El usuario Cajero NO fue redirigido a AccessDenied como debería."
            );
        }
    }
}
