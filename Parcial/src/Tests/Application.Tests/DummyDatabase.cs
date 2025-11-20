using Application.Interfaces;

namespace Application.Tests
{
    public class DummyDatabase : IDatabase
    {
        public int ExecuteNonQuery(string sql, object parameters) => 1;

        // Mantener no estática para cumplir interfaz
        public static string ConnectionString => "Dummy";

    }
}
