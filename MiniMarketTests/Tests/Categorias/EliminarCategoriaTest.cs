using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace MiniMarketTests.Tests.Categorias
{
    [TestFixture]
    [Category("Categorias")]
    public class EliminarCategoriaTest : BaseTest
    {
        [Test]
        public void Eliminar_Categoria_Existente()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            // Login
            driver.Navigate().GoToUrl("http://localhost:5088/Auth/Login");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email")))
                .SendKeys("admin@minimarket.local");

            driver.FindElement(By.Name("password")).SendKeys("Admin123!");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            driver.Navigate().GoToUrl("http://localhost:5088/Categorias");

            // Crear categoría de prueba
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[contains(., 'Nueva Categoría')]"))).Click();

            string nombre = "Eliminar-" + Guid.NewGuid().ToString("N").Substring(0, 5);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Nombre"))).SendKeys(nombre);

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            wait.Until(ExpectedConditions.ElementExists(By.CssSelector(".alert-success, table")));

            // Volver a Index
            driver.Navigate().GoToUrl("http://localhost:5088/Categorias");
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table")));

            // Buscar fila
            var fila = wait.Until(d =>
            {
                try
                {
                    return d.FindElement(By.XPath($"//td[normalize-space()='{nombre}']/parent::tr"));
                }
                catch { return null; }
            });

            Assert.That(fila, Is.Not.Null, $"No se encontró la categoría '{nombre}' después de crearla.");

            // Click en Eliminar
            var btnEliminar = fila.FindElement(By.XPath(".//a[contains(@href,'Delete')]"));
            btnEliminar.Click();

            // Click en botón que abre SweetAlert
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("btnConfirmarEliminar"))).Click();

            // Confirmar SweetAlert
            wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(".swal2-confirm"))).Click();

            // Esperar mensaje de éxito
            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".alert-success")));

            // Validar que ya no existe
            driver.Navigate().GoToUrl("http://localhost:5088/Categorias");

            wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("table")));

            bool aunExiste = driver.PageSource.Contains(nombre);

            Assert.That(aunExiste, Is.False, $"La categoría '{nombre}' aún aparece después de eliminarla.");
        }
    }
}
