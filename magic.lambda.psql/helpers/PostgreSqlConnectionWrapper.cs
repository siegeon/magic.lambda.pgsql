/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System;
using MySql.Data.MySqlClient;

namespace magic.lambda.psql.helpers
{
    /*
     * Internal helper class to create a MySqlConnection lazy, such that it is not actuall created
     * before it's actually de-referenced.
     */
    internal sealed class PostgreSqlConnectionWrapper : IDisposable
    {
        readonly Lazy<MySqlConnection> _connection;

        public PostgreSqlConnectionWrapper(string connectionString)
        {
            _connection = new Lazy<MySqlConnection>(() =>
            {
                var connection = new MySqlConnection(connectionString);
                connection.Open();
                return connection;
            });
        }

        /*
         * Property to retrieve underlying MySQL connection.
         */
        public MySqlConnection Connection => _connection.Value;

        public void Dispose()
        {
            if (_connection.IsValueCreated)
                _connection.Value.Dispose();
        }
    }
}
