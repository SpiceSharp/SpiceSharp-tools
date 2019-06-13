﻿using SpiceSharpParser.Common.Evaluation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpiceSharpParser.ModelReaders.Netlist.Spice.Evaluation.Functions.Math
{
    public class LimitFunction : Function<double, double>
    {
        public LimitFunction()
        {
            Name = "limit";
            VirtualParameters = false;
            ArgumentsCount = 3;
        }

        public override double Logic(string image, double[] args, IEvaluator evaluator, ExpressionContext context)
        {
            if (args.Length != 3)
            {
                throw new ArgumentException("limit() function expects 3 arguments");
            }

            double x = args[0];
            double xMin = args[1];
            double xMax = args[2];

            if (x < xMin)
            {
                return xMin;
            }

            if (x > xMax)
            {
                return xMax;
            }

            return x;
        }
    }
}
