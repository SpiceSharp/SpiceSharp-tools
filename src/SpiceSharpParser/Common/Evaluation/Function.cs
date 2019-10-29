﻿using SpiceSharp.Simulations;
using SpiceSharpParser.ModelReaders.Netlist.Spice.Context;

namespace SpiceSharpParser.Common.Evaluation
{
    public abstract class Function<TInputArgumentType, TOutputType> : IFunction<TInputArgumentType, TOutputType>
    {
        /// <summary>
        /// Gets or sets the name of function.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets arguments count.
        /// </summary>
        public int ArgumentsCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether function is infix.
        /// </summary>
        public bool Infix { get; set; }

        /// <summary>
        /// Computes the value of the function for given arguments.
        /// </summary>
        public abstract TOutputType Logic(string image, TInputArgumentType[] args, IEvaluator evaluator, ExpressionContext context, Simulation simulation = null, IReadingContext readingContext = null);
    }
}
