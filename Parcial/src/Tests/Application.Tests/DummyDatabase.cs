using Application.Interfaces;

namespace Application.Tests // Usa el mismo namespace en ProgramTests.cs
{
    public class DummyDatabase : IDatabase
    {
        public int ExecuteNonQuery(string sql, object parameters) => 1;
        public string ConnectionString => "Dummy";
    }
}
