/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using magic.node;
using magic.signals.contracts;
using magic.data.common.helpers;
using magic.lambda.pgsql.helpers;

namespace magic.lambda.pgsql
{
    /// <summary>
    /// [pgsql.connect] slot for connecting to a PostgreSQL server instance.
    /// </summary>
    [Slot(Name = "pgsql.connect")]
    public class Connect : ISlot, ISlotAsync
    {
        readonly IConfiguration _configuration;

        /// <summary>
        /// Creates a new instance of your class.
        /// </summary>
        /// <param name="configuration">Configuration for your application.</param>
        public Connect(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            using (var connection = new PgSqlConnectionWrapper(
                Executor.GetConnectionString(
                    input,
                    "pgsql",
                    "postgres",
                    _configuration)))
            {
                signaler.Scope(
                    "pgsql.connect",
                    connection,
                    () => signaler.Signal("eval", input));
                input.Value = null;
            }
        }

        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            using (var connection = new PgSqlConnectionWrapper(
                Executor.GetConnectionString(
                    input,
                    "pgsql",
                    "postgres",
                    _configuration)))
            {
                await signaler.ScopeAsync(
                    "pgsql.connect",
                    connection,
                    async () => await signaler.SignalAsync("eval", input));
                input.Value = null;
            }
        }
    }
}
