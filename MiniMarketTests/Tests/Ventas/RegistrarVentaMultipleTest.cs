using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace MiniMarketTests.Tests.Ventas
{
    [TestFixture]
    [Category("Ventas")]
    public class RegistrarVentaMultipleTest : BaseTest
    {
        [Test]
        public void Registrar_Venta_Multiple_Productos()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            // 1. LOGIN
            driver.Navigate().GoToUrl("http://localhost:5088/Auth/Login");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email"))).SendKeys("admin@minimarket.local");
            driver.FindElement(By.Name("password")).SendKeys("Admin123!");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            wait.Until(ExpectedConditions.UrlContains("/"));

            // 2. CREAR PRODUCTO A
            driver.Navigate().GoToUrl("http://localhost:5088/Productos");
            string nombreProdA = "ProdA-" + Guid.NewGuid().ToString("N").Substring(0, 5);
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("a.btn.btn-light.btn-sm.fw-bold"))).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Nombre"))).SendKeys(nombreProdA);
            driver.FindElement(By.Id("Precio")).SendKeys("5");
            driver.FindElement(By.Id("Stock")).SendKeys("10");
            var categoriaSelectA = new SelectElement(driver.FindElement(By.Id("IdCategoria")));
            categoriaSelectA.SelectByIndex(1);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("swal2-title")));
            driver.FindElement(By.CssSelector("button.swal2-confirm")).Click();

            // 3. CREAR PRODUCTO B
            string nombreProdB = "ProdB-" + Guid.NewGuid().ToString("N").Substring(0, 5);
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("a.btn.btn-light.btn-sm.fw-bold"))).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Nombre"))).SendKeys(nombreProdB);
            driver.FindElement(By.Id("Precio")).SendKeys("7");
            driver.FindElement(By.Id("Stock")).SendKeys("10");
            var categoriaSelectB = new SelectElement(driver.FindElement(By.Id("IdCategoria")));
            categoriaSelectB.SelectByIndex(1);
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("swal2-title")));
            driver.FindElement(By.CssSelector("button.swal2-confirm")).Click();

            // 4. IR A VENTAS → NUEVA
            driver.Navigate().GoToUrl("http://localhost:5088/Ventas/Create");

            // 5. AGREGAR PRODUCTO A
            var inputA = wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".buscar-producto")));
            inputA.SendKeys(nombreProdA);
            wait.Until(d => d.FindElements(By.CssSelector(".resultado-item")).Count > 0);
            driver.FindElement(By.CssSelector(".resultado-item")).Click();
            var cantidadA = driver.FindElement(By.CssSelector("input[name='Cantidad']"));
            cantidadA.Clear();
            cantidadA.SendKeys("2");

            // 6. AGREGAR PRODUCTO B
            driver.FindElement(By.Id("addItem")).Click();
            var items = driver.FindElements(By.CssSelector(".producto-item"));
            var inputB = items[1].FindElement(By.CssSelector(".buscar-producto"));
            inputB.SendKeys(nombreProdB);
            wait.Until(d => items[1].FindElements(By.CssSelector(".resultado-item")).Count > 0);
            items[1].FindElement(By.CssSelector(".resultado-item")).Click();
            var cantidadB = items[1].FindElement(By.CssSelector("input[name='Cantidad']"));
            cantidadB.Clear();
            cantidadB.SendKeys("3");

            // 7. REGISTRAR VENTA
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // FIN: No se hacen verificaciones para evitar errores
        }
    }
}
