﻿using SpiceSharpParser.ModelsReaders.Netlist.Spice.Context;
using SpiceSharpParser.Models.Netlist.Spice.Objects;

namespace SpiceSharpParser.ModelsReaders.Netlist.Spice.Readers.Controls
{
    /// <summary>
    /// Reades .GLOBAL <see cref="Control"/> from spice netlist object model.
    /// </summary>
    public class GlobalControl : BaseControl
    {
        /// <summary>
        /// Gets name of Spice element
        /// </summary>
        public override string SpiceName => "global";

        /// <summary>
        /// Reades <see cref="Control"/> statement and modifies the context
        /// </summary>
        /// <param name="statement">A statement to process</param>
        /// <param name="context">A context to modify</param>
        public override void Read(Control statement, IReadingContext context)
        {
            foreach (var param in statement.Parameters)
            {
                if (param is Models.Netlist.Spice.Objects.Parameters.SingleParameter sp)
                {
                    if (sp is Models.Netlist.Spice.Objects.Parameters.WordParameter
                        || sp is Models.Netlist.Spice.Objects.Parameters.IdentifierParameter
                        || sp is Models.Netlist.Spice.Objects.Parameters.ValueParameter)
                    {
                        context.NodeNameGenerator.SetGlobal(sp.Image);
                    }
                }
            }
        }
    }
}
