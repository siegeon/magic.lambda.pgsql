/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Threading.Tasks;
using magic.node;
using magic.data.common;
using magic.signals.contracts;
using magic.data.common.helpers;
using magic.lambda.psql.helpers;

namespace magic.lambda.psql
{
    /// <summary>
    /// [psql.scalar] slot for executing a scalar type of SQL command.
    /// </summary>
    [Slot(Name = "psql.scalar")]
    public class Scalar : ISlot, ISlotAsync
    {
        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            Executor.Execute(
                input,
                signaler.Peek<PostgreSqlConnectionWrapper>("psql.connect").Connection,
                signaler.Peek<Transaction>("psql.transaction"),
                (cmd, _) =>
            {
                input.Value = Converter.GetValue(cmd.ExecuteScalar());
            });
        }

        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            await Executor.ExecuteAsync(
                input,
                signaler.Peek<PostgreSqlConnectionWrapper>("psql.connect").Connection,
                signaler.Peek<Transaction>("psql.transaction"),
                async (cmd, _) =>
            {
                input.Value = await cmd.ExecuteScalarAsync();
            });
        }
    }
}
