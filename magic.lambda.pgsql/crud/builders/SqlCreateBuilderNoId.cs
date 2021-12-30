﻿/*
 * Magic Cloud, copyright Aista, Ltd. See the attached LICENSE file for details.
 */

using magic.node;
using builder = magic.data.common.builders;

namespace magic.lambda.pgsql.crud.builders
{
    /// <summary>
    /// Specialised insert SQL builder, to create an insert MySQL SQL statement
    /// by semantically traversing an input node, that does not return the ID
    /// of the newly created record.
    /// </summary>
    public class SqlCreateBuilderNoId : builder.SqlCreateBuilder
    {
        /// <summary>
        /// Creates an insert SQL statement
        /// </summary>
        /// <param name="node">Root node to generate your SQL from.</param>
        public SqlCreateBuilderNoId(Node node)
            : base(node, "\"")
        { }
    }
}
