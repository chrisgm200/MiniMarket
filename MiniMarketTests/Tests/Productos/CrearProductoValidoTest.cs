using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace MiniMarketTests.Tests.Productos
{
    [TestFixture]
    [Category("Productos")]
    public class CrearProductoValidoTest : BaseTest
    {
        [Test]
        public void Registrar_Producto_Valido()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(12));

            // Login
            driver.Navigate().GoToUrl("http://localhost:5088/Auth/Login");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email"))).SendKeys("admin@minimarket.local");
            driver.FindElement(By.Name("password")).SendKeys("Admin123!");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(ExpectedConditions.UrlContains("/"));

            // Ir a productos
            driver.Navigate().GoToUrl("http://localhost:5088/Productos");

            // CLIC AL BOTÓN NUEVO PRODUCTO (tu botón real del Index)
            wait.Until(ExpectedConditions.ElementIsVisible(
                By.CssSelector("a.btn.btn-light.btn-sm.fw-bold"))).Click();

            // Llenar formulario
            string nombre = "Prod-" + Guid.NewGuid().ToString("N").Substring(0, 5);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Nombre"))).SendKeys(nombre);
            driver.FindElement(By.Id("Precio")).SendKeys("3.50");
            driver.FindElement(By.Id("Stock")).SendKeys("10");

            var categoriaSelect = new SelectElement(driver.FindElement(By.Id("IdCategoria")));
            categoriaSelect.SelectByIndex(1);

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // SweetAlert
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("swal2-title")));
            driver.FindElement(By.CssSelector("button.swal2-confirm")).Click();

            // Verificar en el listado
            bool existe = driver.PageSource.Contains(nombre);
            Assert.That(existe, "El producto no apareció en la tabla.");
        }
    }
}
