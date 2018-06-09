using System;
using Xunit;

namespace SpiceSharpParser.IntegrationTests
{
    public class ParamTest : BaseTest
    {
        [Fact]
        public void ParamCustomFunctionAdvancedTest()
        {
            var netlist = ParseNetlist(
                "PARAM custom function test",
                "V1 IN 0 10.0",
                "R1 IN OUT 10e3",
                "C1 OUT 0 10e-6",
                ".OP",
                ".SAVE VOUT_db V(OUT)",
                ".PARAM decibels_plus_param(value,x)={log10(value)*2+x} add(x,y)={x+y}",
                ".LET VOUT_db {add(decibels_plus_param(V(OUT),1), -0.5)}",
                ".END");

            double[] export = RunOpSimulation(netlist, new string[] { "VOUT_db", "V(OUT)" });

            Assert.Equal(2.5, export[0]);
            Assert.Equal(10, export[1]);
        }

        [Fact]
        public void ParamCustomFunctionManyArgumentsTest()
        {
            var netlist = ParseNetlist(
                "PARAM custom function test",
                "V1 IN 0 10.0",
                "R1 IN OUT 10e3",
                "C1 OUT 0 10e-6",
                ".OP",
                ".SAVE some_output_vector",
                ".PARAM somemagicfunction(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13) = { x1 + x2 + x3  + x4  + x5  + x6  + x7  + x8  + x9 + x10 + x11 + x12 + x13 }",
                ".LET some_output_vector {somemagicfunction(1,2,3,4,5,6,7,8,9,10,11,12,13)}",
                ".END");

            double[] export = RunOpSimulation(netlist, new string[] { "some_output_vector" });

            Assert.Equal(13 * (13+1)/2, export[0]);
        }

        [Fact]
        public void ParamWithoutArgumentsTest()
        {
            var netlist = ParseNetlist(
                "PARAM custom function test",
                "V1 OUT 0 10.0",
                "R1 OUT 0 {somefunction()}",
                ".OP",
                ".SAVE V(OUT) @R1[i]",
                ".PARAM somefunction() = {17}",
                ".END");

            double[] export = RunOpSimulation(netlist, new string[] { "V(OUT)", "@R1[i]" });

            Assert.Equal(10.0, export[0]);
            Assert.Equal(10.0 / 17.0, export[1]);
        }

        [Fact]
        public void ParamFactRecursiveFunctionTest()
        {
            var netlist = ParseNetlist(
                "PARAM recurisve custom function test",
                "V1 OUT 0 60.0",
                "R1 OUT 0 {fact(3)}",
                ".OP",
                ".SAVE V(OUT) @R1[i]",
                ".PARAM fact(x) = {x == 0 ? 1: x * lazy(#fact(x -1)#)}",
                ".END");

            double[] export = RunOpSimulation(netlist, new string[] { "V(OUT)", "@R1[i]" });

            Assert.Equal(60.0, export[0]);
            Assert.Equal(60.0 / 6, export[1]);
        }

        [Fact]
        public void ParamFactRecursiveFunctionCleanSyntaxTest()
        {
            var netlist = ParseNetlist(
                "PARAM recurisve custom function test",
                "V1 OUT 0 60.0",
                "R1 OUT 0 {fact(3)}",
                ".OP",
                ".SAVE V(OUT) @R1[i]",
                ".PARAM fact(x) = {x == 0 ? 1: x * fact(x -1)}",
                ".END");

            double[] export = RunOpSimulation(netlist, new string[] { "V(OUT)", "@R1[i]" });

            Assert.Equal(60.0, export[0]);
            Assert.Equal(60.0 / 6, export[1]);
        }
    }
}
