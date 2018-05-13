﻿namespace SpiceSharpParser.Model.SpiceObjects
{
    /// <summary>
    /// Base class for all spice statements
    /// </summary>
    public abstract class Statement : SpiceObject
    {
        /// <summary>
        /// Gets or sets the line number of the statement
        /// </summary>
        public int? LineNumber { get; set; }
    }
}
