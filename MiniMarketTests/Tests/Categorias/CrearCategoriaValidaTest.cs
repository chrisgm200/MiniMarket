using MiniMarketTests.Tests;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace MiniMarketWebApp.MiniMarketTests.Tests.Categorias
{
    public class CrearCategoriaValidaTest : BaseTest
    {
        [Category("Categorias")]
        [Test]
        public void Crear_Categoria_Valida()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 1. Ir a Login
            driver.Navigate().GoToUrl("http://localhost:5088/Auth/Login");

            // 2. Esperar campos
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email")))
                .SendKeys("admin@minimarket.local");

            driver.FindElement(By.Name("password"))
                .SendKeys("Admin123!");

            // 3. Click en iniciar sesión
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // 4. Esperar redirección a "/"
            wait.Until(ExpectedConditions.UrlToBe("http://localhost:5088/"));

            // 5. Ir a Categorías
            driver.Navigate().GoToUrl("http://localhost:5088/Categorias");

            // 6. Click en “Nueva Categoría” (selector correcto)
            wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//a[contains(., 'Nueva Categoría')]")
            )).Click();

            // 7. Completar formulario
            string nombreCategoria = "Bebidas-" + Guid.NewGuid().ToString("N").Substring(0, 5);

            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Nombre")))
                .SendKeys(nombreCategoria);

            // 8. Guardar
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // 9. Validación de SweetAlert
            try
            {
                wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("swal2-title")));
                driver.FindElement(By.CssSelector("button.swal2-confirm")).Click();
            }
            catch { }

            // 10. Confirmar que aparece en el listado
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("table")));

            bool existe = driver.PageSource.Contains(nombreCategoria);

            Assert.That(existe, Is.True,
                $"La categoría '{nombreCategoria}' no apareció en el listado.");
        }
    }
}
