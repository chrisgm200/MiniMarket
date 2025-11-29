using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;

namespace MiniMarketTests.Tests.Reportes
{
    [TestFixture]
    [Category("Reportes")]
    public class GenerarReporteTest : BaseTest
    {
        [Test]
        public void Generar_Reporte_Filtro_Fecha()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            // Login
            driver.Navigate().GoToUrl("http://localhost:5088/Auth/Login");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email"))).SendKeys("admin@minimarket.local");
            driver.FindElement(By.Name("password")).SendKeys("Admin123!");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(ExpectedConditions.UrlContains("/"));

            // Ir a Reportes
            driver.Navigate().GoToUrl("http://localhost:5088/Reportes");

            // Ingresar fecha de inicio y fin para el filtro
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("fechaInicio"))).SendKeys("2023-01-01");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("fechaFin"))).SendKeys("2023-12-31");

            // Aplicar el filtro
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Verificar que se muestren las ventas filtradas
            var rows = driver.FindElements(By.CssSelector("table tbody tr"));
            Assert.That(rows.Count, Is.GreaterThan(0), "No se encontraron ventas en el rango de fechas.");

            // Comprobar que el total es correcto, o cualquier otra validación
        }
    }
}
