using System;
using Infrastructure.Logging;
using Xunit;

namespace Application.Tests // <--- AGREGA ESTE BLOQUE
{
    public class LoggerTests
    {
        [Fact]
        public void Log_DoesNotThrow_AndWritesToConsole()
        {
            // Arrange
            var logger = new Logger();
            var testMessage = "Mensaje de prueba";
            var originalOut = Console.Out;
            using var sw = new System.IO.StringWriter();
            try
            {
                Console.SetOut(sw);
                // Act
                logger.Log(testMessage);
                // Assert
                var output = sw.ToString();
                Assert.Contains("LOG", output);
                Assert.Contains(testMessage, output);
                Assert.Contains(DateTime.Now.Year.ToString(), output); // Año actual
            }
            finally
            {
                Console.SetOut(originalOut); // SIEMPRE restáuralo
            }
        }

        [Fact]
        public void LogError_DoesNotThrow_AndWritesToConsole()
        {
            // Arrange
            var logger = new Logger();
            var testMessage = "Problema crítico";
            var exception = new InvalidOperationException("Error simulado");
            var originalOut = Console.Out;
            using var sw = new System.IO.StringWriter();
            try
            {
                Console.SetOut(sw);
                // Act
                logger.LogError(exception, testMessage);
                // Assert
                var output = sw.ToString();
                Assert.Contains("ERROR", output);
                Assert.Contains(testMessage, output);
                Assert.Contains(exception.GetType().Name, output);
                Assert.Contains(DateTime.Now.Year.ToString(), output);
            }
            finally
            {
                Console.SetOut(originalOut); // SIEMPRE restáuralo
            }
        }
    }
}
