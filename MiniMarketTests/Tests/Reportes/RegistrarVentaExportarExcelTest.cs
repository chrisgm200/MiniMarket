using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Threading;

namespace MiniMarketTests.Tests.Reportes
{
    [TestFixture]
    [Category("Reportes")]
    public class RegistrarVentaExportarExcelTest
    {
        private IWebDriver _driver;
        private string _downloadDirectory;

        [SetUp]
        public void Setup()
        {
            // Configura el directorio de descarga
            _downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Downloads");
            if (!Directory.Exists(_downloadDirectory))
            {
                Directory.CreateDirectory(_downloadDirectory);
            }

            // Configura el navegador
            var options = new ChromeOptions();
            options.AddUserProfilePreference("download.default_directory", _downloadDirectory);
            _driver = new ChromeDriver(options);
        }

        [Test]
        public void Exportar_Venta_A_Excel()
        {
            // Inicia sesión si es necesario
            _driver.Navigate().GoToUrl("http://localhost:5088/Auth/Login");
            _driver.FindElement(By.Name("email")).SendKeys("admin@minimarket.local");
            _driver.FindElement(By.Name("password")).SendKeys("Admin123!");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Navega al reporte de ventas
            _driver.Navigate().GoToUrl("http://localhost:5088/Reportes");

            // Establecer filtros para la exportación
            _driver.FindElement(By.Name("fechaInicio")).SendKeys("2025-11-01");
            _driver.FindElement(By.Name("fechaFin")).SendKeys("2025-11-30");

            // Hacer clic en el botón de exportar a Excel
            _driver.FindElement(By.CssSelector("a.btn.btn-success")).Click();

            // Esperar unos segundos para que el archivo sea descargado
            Thread.Sleep(5000);

            // Verificar si el archivo fue descargado
            var downloadedFile = Directory.GetFiles(_downloadDirectory, "Reporte_Ventas_*.xlsx");
            Assert.That(downloadedFile.Length, Is.GreaterThan(0), "El archivo Excel no se descargó correctamente.");

            // Limpieza de archivos descargados
            foreach (var file in downloadedFile)
            {
                File.Delete(file);
            }
        }

        [TearDown]
        public void Teardown()
        {
            _driver.Quit();
        }
    }
}
