﻿/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using magic.node;
using magic.signals.contracts;
using System.Threading.Tasks;
using magic.lambda.psql.helpers;
using hlp = magic.data.common.helpers;

namespace magic.lambda.psql
{
    /// <summary>
    /// [psql.transaction.create] slot for creating a new MySQL database transaction.
    /// </summary>
    [Slot(Name = "psql.transaction.create")]
    public class CreateTransaction : ISlot, ISlotAsync
    {
        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        public void Signal(ISignaler signaler, Node input)
        {
            signaler.Scope(
                "psql.transaction",
                new hlp.Transaction(signaler, signaler.Peek<PostgreSqlConnectionWrapper>("psql.connect").Connection),
                () => signaler.Signal("eval", input));
        }

        /// <summary>
        /// Handles the signal for the class.
        /// </summary>
        /// <param name="signaler">Signaler used to signal the slot.</param>
        /// <param name="input">Root node for invocation.</param>
        /// <returns>An awaitable task.</returns>
        public async Task SignalAsync(ISignaler signaler, Node input)
        {
            await signaler.ScopeAsync(
                "psql.transaction",
                new hlp.Transaction(signaler, signaler.Peek<PostgreSqlConnectionWrapper>("psql.connect").Connection),
                async () => await signaler.SignalAsync("eval", input));
        }
    }
}
