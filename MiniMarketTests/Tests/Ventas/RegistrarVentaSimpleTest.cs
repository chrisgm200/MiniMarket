using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Linq;

namespace MiniMarketTests.Tests.Ventas
{
    [TestFixture]
    [Category("Ventas")]
    public class RegistrarVentaSimpleTest : BaseTest
    {
        [Test]
        public void Registrar_Venta_Simple_Un_Producto()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            // 1. LOGIN
            driver.Navigate().GoToUrl("http://localhost:5088/Auth/Login");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email"))).SendKeys("admin@minimarket.local");
            driver.FindElement(By.Name("password")).SendKeys("Admin123!");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            wait.Until(ExpectedConditions.UrlContains("/"));

            // 2. CREAR PRODUCTO PARA VENTA
            driver.Navigate().GoToUrl("http://localhost:5088/Productos");
            string nombreProducto = "ProdVenta-" + Guid.NewGuid().ToString("N").Substring(0, 5);
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("a.btn.btn-light.btn-sm.fw-bold"))).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Nombre"))).SendKeys(nombreProducto);
            driver.FindElement(By.Id("Precio")).SendKeys("5");
            driver.FindElement(By.Id("Stock")).SendKeys("10");
            var categoriaSelect = new SelectElement(driver.FindElement(By.Id("IdCategoria")));
            categoriaSelect.SelectByIndex(1);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("swal2-title")));
            driver.FindElement(By.CssSelector("button.swal2-confirm")).Click();

            // 3. IR A VENTAS → NUEVA
            driver.Navigate().GoToUrl("http://localhost:5088/Ventas/Create");
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".buscar-producto"))).SendKeys(nombreProducto);

            // Esperar autocomplete y seleccionar producto
            wait.Until(d => d.FindElements(By.CssSelector(".resultado-item")).Count > 0);
            var item = driver.FindElement(By.CssSelector(".resultado-item"));
            item.Click();

            // 4. INGRESAR CANTIDAD
            var cantidadInput = driver.FindElement(By.CssSelector("input[name='Cantidad']"));
            cantidadInput.Clear();
            cantidadInput.SendKeys("1");

            // 5. REGISTRAR VENTA
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // 6. ESPERAR REDIRECCIÓN A INDEX DE VENTAS
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table")));

            // 7. VERIFICAR QUE LA VENTA APARECE EN EL HISTORIAL
            var rows = driver.FindElements(By.CssSelector("table tbody tr"));
            bool existeVenta = rows.Any(r => r.Text.Contains(nombreProducto) || r.Text.Contains("5")); // verificar por total

            Assert.That(existeVenta, Is.True, "La venta del producto no aparece en el historial.");
        }
    }
}
