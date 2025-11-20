using System;
using System.Data;
using System.Data.SqlClient;
using Application.Interfaces;

namespace Infrastructure.Data
{
    public class BadDb : IDatabase
    {
        private readonly string _connectionString;

        public BadDb()
        {
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD")
                ?? throw new InvalidOperationException("DB_PASSWORD environment variable not set");
            _connectionString = $"Server=localhost;Database=master;User Id=sa;Password={password};TrustServerCertificate=True";
        }

        public int ExecuteNonQuery(string sql, object parameters)
        {
            if (parameters is not SqlParameter[] sqlParams)
                throw new ArgumentException("parameters deben ser SqlParameter[]", nameof(parameters));
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddRange(sqlParams);
            conn.Open();
            return cmd.ExecuteNonQuery(); // <-- devuelve int
        }
     

        public string ConnectionString => _connectionString;
    }
}

