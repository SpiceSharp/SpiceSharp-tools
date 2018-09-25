﻿using SpiceSharp.Simulations;
using SpiceSharpParser.ModelReaders.Netlist.Spice.Context;
using SpiceSharpParser.ModelReaders.Netlist.Spice.Readers.Controls.Exporters;
using SpiceSharpParser.ModelReaders.Netlist.Spice.Registries;
using SpiceSharpParser.Models.Netlist.Spice.Objects;
using SpiceSharpParser.Models.Netlist.Spice.Objects.Parameters;

namespace SpiceSharpParser.ModelReaders.Netlist.Spice.Readers.Controls
{
    public abstract class ExportControl : BaseControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportControl"/> class.
        /// </summary>
        /// <param name="mapper">The exporter mapper.</param>
        public ExportControl(IMapper<Exporter> mapper)
        {
            Mapper = mapper ?? throw new System.ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets the exporter mapper.
        /// </summary>
        protected IMapper<Exporter> Mapper { get; }

        /// <summary>
        /// Generates a new export.
        /// </summary>
        protected Export GenerateExport(Parameter parameter, Simulation simulation, INodeNameGenerator nodeNameGenerator, IObjectNameGenerator objectNameGenerator)
        {
            if (parameter is BracketParameter bp)
            {
                string type = bp.Name.ToLower();

                if (Mapper.Contains(type))
                {
                    return Mapper.Get(type).CreateExport(type, bp.Parameters, simulation, nodeNameGenerator, objectNameGenerator);
                }
            }

            if (parameter is ReferenceParameter rp)
            {
                string type = "@";

                if (Mapper.Contains(type))
                {
                    var parameters = new ParameterCollection();
                    parameters.Add(new WordParameter(rp.Name));
                    parameters.Add(new WordParameter(rp.Argument));

                    return Mapper.Get(type).CreateExport(type, parameters, simulation, nodeNameGenerator, objectNameGenerator);
                }
            }

            throw new System.Exception("Unsuported export: " + parameter.Image);
        }
    }
}
