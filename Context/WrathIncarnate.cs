#region

using System;
using PixelPubApi.Models.Entities;
using MySql.Data.MySqlClient;

#endregion

namespace PixelPubApi.MySQL
{
    public class WrathIncarnateContext : IDisposable
    {
        public MySqlConnection connection;

        public WrathIncarnateContext(string connectionString) {
            connection = new MySqlConnection(connectionString);
        }

        public void Dispose() {
            connection.Close();
        }
    }
}