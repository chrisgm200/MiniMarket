using MiniMarketTests.Tests;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace MiniMarketWebApp.MiniMarketTests.Tests.Categorias
{
    public class CrearCategoriaInvalidaTest : BaseTest
    {
        [Category("Categorias")]
        [Test]
        public void Crear_Categoria_Invalida_Nombre_Vacio()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 1. Ir a Login
            driver.Navigate().GoToUrl("http://localhost:5088/Auth/Login");

            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email")))
                .SendKeys("admin@minimarket.local");

            driver.FindElement(By.Name("password"))
                .SendKeys("Admin123!");

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // 2. Esperar redirección a "/"
            wait.Until(ExpectedConditions.UrlToBe("http://localhost:5088/"));

            // 3. Ir a Categorías
            driver.Navigate().GoToUrl("http://localhost:5088/Categorias");

            // 4. Ir a Nueva Categoría (selector correcto por texto)
            wait.Until(ExpectedConditions.ElementIsVisible(
                By.XPath("//a[contains(., 'Nueva Categoría')]")
            )).Click();

            // 5. Dejar nombre vacío y presionar Guardar
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Nombre")));
            driver.FindElement(By.Id("Nombre")).Clear();

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // 6. Validar que aparece mensaje de error de validación
            var mensajeError = wait.Until(ExpectedConditions
                .ElementIsVisible(By.CssSelector("span.text-danger"))).Text;

            Assert.That(
                !string.IsNullOrWhiteSpace(mensajeError),
                Is.True,
                "No se mostró el mensaje de error en el campo Nombre."
            );

            // 7. Validar que NO hubo redirección a Index
            Assert.That(
                driver.Url.Contains("/Categorias/Create"),
                Is.True,
                "El sistema no debería haber permitido crear la categoría."
            );
        }
    }
}
