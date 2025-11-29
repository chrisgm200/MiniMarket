using MiniMarketTests.Tests;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Net;

namespace MiniMarketWebApp.MiniMarketTests.Tests.Auth
{
    public class LoginFailTest : BaseTest
    {
        [Category("Auth")]
        [Test]
        public void Login_Fallido_DebeMostrarMensajeError()
        {
            driver.Navigate().GoToUrl("http://localhost:5088/Auth/Login");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 1. Rellenar formulario con datos inválidos
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email")))
                .SendKeys("mal@correo.com");

            driver.FindElement(By.Name("password")).SendKeys("Error123");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // 2. Capturar swal2
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".swal2-html-container")));

            var rawText = driver.FindElement(By.CssSelector(".swal2-html-container")).Text;

            // Decodificar caracteres HTML
            var alertText = WebUtility.HtmlDecode(rawText);

            Console.WriteLine("Texto decodificado: >>> " + alertText + " <<<");

            // 3. Validar que contiene el mensaje esperado
            Assert.That(alertText.Contains("Credenciales inválidas"), Is.True);
        }
    }
}
