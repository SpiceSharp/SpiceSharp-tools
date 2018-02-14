﻿using System;
using System.Collections.Generic;
using SpiceNetlist.SpiceObjects;
using SpiceSharp.Simulations;

namespace SpiceNetlist.SpiceSharpConnector.Processors.Controls
{
    public class DCControl : SingleControlProcessor
    {
        public override void Process(Control statement, ProcessingContext context)
        {
            int count = statement.Parameters.Count / 4;
            switch (statement.Parameters.Count - (4 * count))
            {
                case 0:
                    if (statement.Parameters.Count == 0)
                    {
                        throw new Exception("Source st.Name expected");
                    }
                    break;
                case 1: throw new Exception("Start value expected");
                case 2: throw new Exception("Stop value expected");
                case 3: throw new Exception("Step value expected");
            }

            // Format: .DC SRCNAM VSTART VSTOP VINCR [SRC2 START2 STOP2 INCR2]
            List<SweepConfiguration> sweeps = new List<SweepConfiguration>();

            for (int i = 0; i < count; i++)
            {
                SweepConfiguration sweep = new SweepConfiguration(
                    statement.Parameters.GetString(4 * i).ToLower(),
                    context.ParseDouble(statement.Parameters.GetString((4 * i) + 1).ToLower()),
                    context.ParseDouble(statement.Parameters.GetString((4 * i) + 2).ToLower()),
                    context.ParseDouble(statement.Parameters.GetString((4 * i) + 3).ToLower()));

                sweeps.Add(sweep);
            }

            DC dc = new DC("DC " + (context.SimulationsCount + 1), sweeps);
            context.AddSimulation(dc);
        }
    }
}
