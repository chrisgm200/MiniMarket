using NUnit.Framework;
using OpenQA.Selenium;
using System;

namespace MiniMarketTests.Tests.Error
{
    [TestFixture]
    public class ErrorGeneralTest : BaseTest
    {
        [Category("Error")]
        [Test]
        public void Verificar_Error_General()
        {
            // Forzar un error en el controlador
            driver.Navigate().GoToUrl("http://localhost:5088/Home/ErrorGeneral");

            // Esperar que se muestre la vista de error
            var errorTitle = driver.FindElement(By.CssSelector(".display-4")).Text;
            var errorMessage = driver.FindElement(By.CssSelector(".text-muted.mb-4")).Text;

      
        }
    }
}
