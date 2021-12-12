﻿/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using System.Threading.Tasks;
using magic.node;
using magic.signals.contracts;
using magic.data.common.helpers;
using magic.lambda.psql.helpers;

namespace magic.lambda.psql
{
    /// <summary>
    /// [psql.execute] slot for executing a non query SQL command.
    /// </summary>
    [Slot(Name = "psql.execute")]
    public class Execute : ISlot, ISlotAsync
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
                input.Value = cmd.ExecuteNonQuery();
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
                input.Value = await cmd.ExecuteNonQueryAsync();
            });
        }
    }
}
