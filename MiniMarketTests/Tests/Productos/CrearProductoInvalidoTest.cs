using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace MiniMarketTests.Tests.Productos
{
    [TestFixture]
    [Category("Productos")]
    public class CrearProductoInvalidoTest : BaseTest
    {
        [Test]
        public void Registrar_Producto_Invalido()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 1. Login
            driver.Navigate().GoToUrl("http://localhost:5088/Auth/Login");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email")))
                .SendKeys("admin@minimarket.local");

            driver.FindElement(By.Name("password")).SendKeys("Admin123!");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(ExpectedConditions.UrlContains("/"));

            // 2. Ir a Productos
            driver.Navigate().GoToUrl("http://localhost:5088/Productos");

            // 3. Clic en "Nuevo Producto"
            var botonNuevo = wait.Until(ExpectedConditions
                .ElementIsVisible(By.CssSelector("a.btn.btn-light.btn-sm.fw-bold")));
            botonNuevo.Click();

            // 4. Dejar nombre vacío y precio negativo
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Precio"))).SendKeys("-5");
            driver.FindElement(By.Id("Stock")).SendKeys("10");

            // No seleccionar categoría para forzar validación
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // 5. Validación: encontrar errores de validación
            var errorNombre = wait.Until(ExpectedConditions
                .ElementIsVisible(By.CssSelector("span[data-valmsg-for='Nombre']")));

            string msgNombre = errorNombre.Text;

            Assert.That(msgNombre.Length > 0, "❌ No apareció la validación de Nombre.");
        }
    }
}
