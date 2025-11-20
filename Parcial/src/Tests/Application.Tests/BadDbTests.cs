using Xunit;
using Infrastructure.Data;
using System;
using System.Data.SqlClient;

namespace Application.Tests
{
    public class BadDbTests
    {
        [Fact]
        public void Constructor_Throws_WhenPasswordEnvVarNotSet()
        {
            // Arrange: Asegurarse de limpiar la variable de entorno
            Environment.SetEnvironmentVariable("DB_PASSWORD", null);

            // Act & Assert: Debe lanzar la excepción
            var ex = Assert.Throws<InvalidOperationException>(() => new BadDb());
            Assert.Contains("DB_PASSWORD environment variable not set", ex.Message);
        }

        [Fact]
        public void ExecuteNonQuery_Throws_WhenParametersIsNotSqlParameterArray()
        {
            // Arrange
            Environment.SetEnvironmentVariable("DB_PASSWORD", "dummy");
            var db = new BadDb();

            // Act & Assert: Lanza ArgumentException si los parámetros son incorrectos
            Assert.Throws<ArgumentException>(() => db.ExecuteNonQuery("SELECT 1", "no-es-array"));
        }

        [Fact]
        public void ConnectionString_ReturnsExpectedValue()
        {
            // Arrange
            Environment.SetEnvironmentVariable("DB_PASSWORD", "mysecret123");
            var db = new BadDb();

            // Act
            var cs = db.ConnectionString;

            // Assert: Debe contener datos configurados
            Assert.Contains("Password=mysecret123", cs);
            Assert.Contains("Server=localhost", cs);
            Assert.Contains("Database=master", cs);
        }
    }
}
