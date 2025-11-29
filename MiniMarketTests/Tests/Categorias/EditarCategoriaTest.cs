using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace MiniMarketTests.Tests.Categorias
{
    [TestFixture]
    [Category("Categorias")]
    public class EditarCategoriaTest : BaseTest
    {
        [Test]
        public void Editar_Categoria_Existente()
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // 1. Login
            driver.Navigate().GoToUrl("http://localhost:5088/Auth/Login");
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("email")))
                .SendKeys("admin@minimarket.local");
            driver.FindElement(By.Name("password")).SendKeys("Admin123!");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // 2. Esperar redirección a "/"
            wait.Until(ExpectedConditions.UrlToBe("http://localhost:5088/"));

            // 3. Ir a Categorías
            driver.Navigate().GoToUrl("http://localhost:5088/Categorias");

            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("table")));

            // 4. Click en editar de la primera categoría
            var btnEditar = wait.Until(ExpectedConditions.ElementToBeClickable(
                By.XPath("//a[contains(@href,'Edit')]")
            ));
            btnEditar.Click();

            // 5. Editar nombre
            var input = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("Nombre")));
            input.Clear();

            string nuevoNombre = "EDIT-" + Guid.NewGuid().ToString("N").Substring(0, 5);
            input.SendKeys(nuevoNombre);

            driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // 6. Validar alerta Bootstrap (NO SweetAlert)
            var alerta = wait.Until(ExpectedConditions
                .ElementIsVisible(By.CssSelector(".alert.alert-success")));

            Assert.That(
                alerta.Text.ToLower().Contains("actualizada"),
                "No apareció el mensaje de categoría actualizada."
            );

            // 7. Verificar que aparece en la tabla
            wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName("table")));

            bool existe = driver.PageSource.Contains(nuevoNombre);

            Assert.That(
                existe,
                Is.True,
                $"El nuevo nombre '{nuevoNombre}' no aparece en la tabla."
            );
        }
    }
}
