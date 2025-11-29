using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace MiniMarketTests.Tests.Productos
{
    [TestFixture]
    [Category("Productos")]
    public class EliminarProductoTest
    {
        private IWebDriver driver;
        private WebDriverWait wait;
        private readonly string baseUrl = "http://localhost:5088";

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        [Test]
        public void Eliminar_Producto_Valido()
        {
            // LOGIN
            driver.Navigate().GoToUrl(baseUrl + "/Auth/Login");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email"))).SendKeys("admin@minimarket.local");
            driver.FindElement(By.Name("password")).SendKeys("Admin123!");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // ESPERAR HOME
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("a[href='/Productos']")));

            // IR A PRODUCTOS
            driver.Navigate().GoToUrl(baseUrl + "/Productos");
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector("div.card-body")));

            // VALIDAR QUE HAYA PRODUCTOS
            bool sinProductos = driver.PageSource.Contains("No hay productos");
            Assert.That(sinProductos, Is.False, "No hay productos para eliminar.");

            // TOMAR PRIMER PRODUCTO
            var firstRow = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table tbody tr")));
            string idProducto = firstRow.FindElement(By.CssSelector("td:nth-child(1)")).Text;

            // CLICK EN ELIMINAR
            firstRow.FindElement(By.CssSelector(".btn.btn-danger")).Click();

            // ESPERAR VISTA DELETE
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".card.border-danger")));

            // CLICK EN BOTÓN DE CONFIRMAR
            driver.FindElement(By.CssSelector("button.btn.btn-danger")).Click();

            // ESPERAR REDIRECCIÓN A INDEX
            wait.Until(ExpectedConditions.UrlContains("/Productos"));

            // ESPERAR SWEETALERT
            try
            {
                wait.Until(ExpectedConditions.ElementExists(By.Id("swal-success-container")));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("swal-success-container")));
            }
            catch
            {
                // si no aparece el alert, seguimos
            }

            // VALIDAR TABLA O MENSAJE VACÍO
            var tablas = driver.FindElements(By.CssSelector("table"));
            if (tablas.Count > 0)
            {
                var rows = driver.FindElements(By.CssSelector("table tbody tr"));
                bool existe = false;
                foreach (var row in rows)
                {
                    string id = row.FindElement(By.CssSelector("td:nth-child(1)")).Text;
                    if (id == idProducto)
                    {
                        existe = true;
                        break;
                    }
                }
                Assert.That(existe, Is.False, $"El producto ID {idProducto} sigue en la tabla.");
            }
            else
            {
                // no hay tabla → lista vacía, pasó
                Assert.Pass("No hay tabla, todos los productos eliminados correctamente.");
            }

        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
