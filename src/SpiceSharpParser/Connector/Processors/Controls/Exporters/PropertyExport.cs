﻿using SpiceSharp;
using SpiceSharp.Simulations;
using SpiceSharpParser.Connector.Exceptions;

namespace SpiceSharpParser.Connector.Processors.Controls.Exporters
{
    /// <summary>
    /// Property export.
    /// </summary>
    public class PropertyExport : Export
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyExport"/> class.
        /// </summary>
        /// <param name="simulation">A simulation</param>
        /// <param name="source">A identifier of component</param>
        /// <param name="property">Name of property for export</param>
        public PropertyExport(Simulation simulation, Identifier source, string property)
            : base(simulation)
        {
            if (simulation == null)
            {
                throw new System.ArgumentNullException(nameof(simulation));
            }

            Source = source ?? throw new System.NullReferenceException(nameof(source));

            ExportRealImpl = new RealPropertyExport(simulation, source, property);
            Name = string.Format("@{0}[{1}]", Source, property);
        }

        /// <summary>
        /// Gets the main node
        /// </summary>
        public Identifier Source { get; }

        /// <summary>
        /// Gets the type name
        /// </summary>
        public override string TypeName => string.Empty;

        /// <summary>
        /// Gets the quantity unit
        /// </summary>
        public override string QuantityUnit => string.Empty;

        /// <summary>
        /// Gets the real exporter
        /// </summary>
        protected RealPropertyExport ExportRealImpl { get; }

        /// <summary>
        /// Extracts the current value
        /// </summary>
        /// <returns>
        /// A current value at the source
        /// </returns>
        public override double Extract()
        {
            if (!ExportRealImpl.IsValid)
            {
                throw new GeneralConnectorException($"Property export {Name} is invalid");
            }

            return ExportRealImpl.Value;
        }
    }
}
