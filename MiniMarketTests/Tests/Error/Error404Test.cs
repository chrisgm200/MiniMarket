using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace MiniMarketTests.Tests.Error
{
    [TestFixture]
    [Category("Error")]
    public class Error404Test : BaseTest
    {
        [Test]
        public void Verificar_Pagina_404_Personalizada()
        {
            // Navegar a una ruta inexistente
            driver.Navigate().GoToUrl("http://localhost:5088/no-existe");

            // Esperar que la página cargue y que se muestre la vista 404 personalizada
            var errorTitle = driver.FindElement(By.CssSelector(".display-4")).Text;
            var errorMessage = driver.FindElement(By.CssSelector("h3.mb-3")).Text;

            // Verificar que el mensaje de error y el título son los correctos
            Assert.That(errorTitle, Is.EqualTo("404"));
            Assert.That(errorMessage, Is.EqualTo("La página que buscas no existe"));

            // Verificar que el botón para volver al inicio está presente
            var backButton = driver.FindElement(By.CssSelector("a.btn-primary"));
            Assert.That(backButton.Displayed);

            // Simular hacer clic en el botón "Volver al inicio"
            backButton.Click();

            // Esperar que la página de inicio se cargue
            var currentUrl = driver.Url;
            Assert.That(currentUrl, Is.EqualTo("http://localhost:5088/"));
        }
    }
}
