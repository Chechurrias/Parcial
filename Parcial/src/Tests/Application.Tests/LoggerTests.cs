using System;
using Infrastructure.Logging;
using Xunit;

public class LoggerTests
{
    [Fact]
    public void Log_DoesNotThrow_AndWritesToConsole()
    {
        // Arrange
        var logger = new Logger();
        var testMessage = "Mensaje de prueba";
        using var sw = new System.IO.StringWriter();
        Console.SetOut(sw);

        // Act
        logger.Log(testMessage);

        // Assert
        var output = sw.ToString();
        Assert.Contains("LOG", output);
        Assert.Contains(testMessage, output);
        Assert.Contains(DateTime.Now.Year.ToString(), output); // Año actual
    }

    [Fact]
    public void LogError_DoesNotThrow_AndWritesToConsole()
    {
        // Arrange
        var logger = new Logger();
        var testMessage = "Problema crítico";
        var exception = new InvalidOperationException("Error simulado");
        using var sw = new System.IO.StringWriter();
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
}
