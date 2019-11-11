﻿using SpiceSharpParser.Common.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using SpiceSharp.Simulations;
using SpiceSharpBehavioral.Parsers;
using SpiceSharpParser.ModelReaders.Netlist.Spice.Context;

namespace SpiceSharpParser.ModelReaders.Netlist.Spice.Evaluation.Functions.Math
{
    public class TableFunction : Function<double, double>, IDerivativeFunction<double, double>
    {
        public TableFunction()
        {
            Name = "table";
            ArgumentsCount = -1;
        }

        public override double Logic(string image, double[] args, IEvaluator evaluator, ExpressionContext context, Simulation simulation = null, IReadingContext readingContext = null)
        {
            var parameterValue = args[0];

            var points = new List<Point>();

            for (var i = 1; i < args.Length - 1; i += 2)
            {
                var pointX = args[i];
                var pointY = args[i + 1];
                points.Add(new Point() {X = pointX, Y = pointY});

                if (pointX == parameterValue)
                {
                    return pointY;
                }
            }

            points.Sort((p1, p2) => p1.X.CompareTo(p2.X));
            if (points.Count == 1)
            {
                throw new Exception("There is only one point for table interpolation.");
            }

            // Get point + 1 line parameters for each segment of line
            LineDefinition[] linesDefinition = CreateLineParameters(points);

            int index = 0;

            while (index < points.Count && points[index].X < parameterValue)
            {
                index++;
            }

            if (index == points.Count)
            {
                return points[points.Count - 1].Y;
            }

            if (index == 0 && points[0].X > parameterValue)
            {
                return points[0].Y;
            }

            return (linesDefinition[index].A * parameterValue) + linesDefinition[index].B;
        }


        private static LineDefinition[] CreateLineParameters(List<Point> points)
        {
            List<LineDefinition> result = new List<LineDefinition>();

            for (var i = 0; i < points.Count - 1; i++)
            {
                double x1 = points[i].X;
                double x2 = points[i + 1].X;
                double y1 = points[i].Y;
                double y2 = points[i + 1].Y;

                double a = (y2 - y1) / (x2 - x1);

                result.Add(new LineDefinition()
                {
                    A = a,
                    B = y1 - (a * x1),
                });
            }

            result.Insert(0, result[0]);
            result.Add(result[result.Count - 1]);
            return result.ToArray();
        }

        public class LineDefinition
        {
            public double A { get; set; }

            public double B { get; set; }
        }

        public class Point
        {
            public double X { get; set; }

            public double Y { get; set; }
        }

        public Derivatives<Func<double>> Derivative(string image, double[] args, IEvaluator evaluator, ExpressionContext context,
            Simulation simulation = null, IReadingContext readingContext = null)
        {
            var parameterValue = args[0];
            var points = new List<Point>();
            Derivatives<Func<double>> derivatives;

            for (var i = 1; i < args.Length - 1; i += 2)
            {
                var pointX = args[i];
                var pointY = args[i + 1];
                points.Add(new Point() { X = pointX, Y = pointY });

                if (pointX == parameterValue)
                {
                    derivatives = new DoubleDerivatives(1);
                    derivatives[0] = () => pointY;
                    return derivatives;
                }
            }

            points.Sort((p1, p2) => p1.X.CompareTo(p2.X));
            if (points.Count == 1)
            {
                throw new Exception("There is only one point for table interpolation.");
            }

            // Get point + 1 line parameters for each segment of line
            LineDefinition[] linesDefinition = CreateLineParameters(points);

            int index = 0;

            while (index < points.Count && points[index].X < parameterValue)
            {
                index++;
            }

            if (index == points.Count)
            {
                derivatives = new DoubleDerivatives(2);
                derivatives[0] = () => points[points.Count - 1].Y;
                derivatives[1] = () => linesDefinition.Last().A;

                return derivatives;
            }

            if (index == 0 && points[0].X > parameterValue)
            {
                derivatives = new DoubleDerivatives(2);
                derivatives[0] = () => points[0].Y;
                derivatives[1] = () => linesDefinition.First().A;
            }
         
            derivatives = new DoubleDerivatives(2);
            derivatives[0] = () => linesDefinition[index].A * parameterValue + linesDefinition[index].B;
            derivatives[1] = () => linesDefinition[index].A;

            return derivatives;
        }
    }
}
