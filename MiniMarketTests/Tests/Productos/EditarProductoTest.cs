using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace MiniMarketTests.Tests.Productos
{
    [TestFixture]
    [Category("Productos")]
    public class EditarProductoTest : BaseTest
    {
        [Test]
        public void Editar_Producto_Valores_Actualizados()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 1. Login
            driver.Navigate().GoToUrl("http://localhost:5088/Auth/Login");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email"))).SendKeys("admin@minimarket.local");
            driver.FindElement(By.Name("password")).SendKeys("Admin123!");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(ExpectedConditions.UrlContains("/"));

            // 2. Ir a Productos
            driver.Navigate().GoToUrl("http://localhost:5088/Productos");

            // 3. Seleccionar el primer producto y entrar a Editar
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("a.btn-edit"))).Click();

            // Generar valores nuevos
            string nuevoPrecio = "7.99";
            string nuevoStock = "25";

            // 4. Editar campos
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Precio"))).Clear();
            driver.FindElement(By.Id("Precio")).SendKeys(nuevoPrecio);

            driver.FindElement(By.Id("Stock")).Clear();
            driver.FindElement(By.Id("Stock")).SendKeys(nuevoStock);

            // Guardar
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // 5. SweetAlert2
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".swal2-title")));
            driver.FindElement(By.CssSelector("button.swal2-confirm")).Click();

            // 6. Validar en el listado
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table")));

            bool precioActualizado = driver.PageSource.Contains(nuevoPrecio);
            bool stockActualizado = driver.PageSource.Contains(nuevoStock);

            Assert.That(precioActualizado);
            Assert.That(stockActualizado);
        }
    }
}
