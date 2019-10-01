using System;
using System.Linq;
using MathGeneratorREST.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MathGeneratorTests
{
    [TestClass]
    public class MathServiceTest
    {
        [TestMethod]
        public void GenerateProblems()
        {
            var service = new MathService();

            var expectedProblems = 4;

            var expressions = service.Generate(expectedProblems, null, null);

            var numProblems = expressions.Split('\n').Length - 1;
            Assert.AreEqual(expectedProblems,numProblems);

        }

        [TestMethod]
        public void GenerateProblemsWithOptionalParameters()
        {
            var service = new MathService();

            var expectedProblems = 4;
            var minOperations = 2;
            var maxOperations = 3;

            var expressions = service.Generate(expectedProblems, minOperations, maxOperations);

            var problems = expressions.Split('\n');
            problems = problems.Take(problems.Length - 1).ToArray();

            foreach (var problem in problems)
            {
                var count1 = problem.Distinct().Count(x => x == '+');
                var count2 = problem.Distinct().Count(x => x == '-');
                var count3 = problem.Distinct().Count(x => x == '*');
                var count4 = problem.Distinct().Count(x => x == '/');
                var count = count1 + count2 + count3 + count4;

                Assert.IsTrue(minOperations <= count && maxOperations >= count);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GenerateNegativeProblems()
        {
            var service = new MathService();

            var expectedProblems = -4;

            var expressions = service.Generate(expectedProblems, null, null);
        }

        [TestMethod]
        public void GenerateZeroProblems()
        {
            var service = new MathService();

            var expressions = service.Generate(0, null, null);

            Assert.AreEqual("No problems to generate", expressions);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void OutOfRangeMinOperations()
        {
            var service = new MathService();

            var expectedProblems = 4;
            var minOperations = -2;

            var expressions = service.Generate(expectedProblems, minOperations, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void OutOfRangeMaxOperations()
        {
            var service = new MathService();

            var expectedProblems = 4;
            var maxOperations = -2;

            var expressions = service.Generate(expectedProblems, null, maxOperations);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MinOperationsGreaterThanMaxOperations()
        {
            var service = new MathService();

            var expectedProblems = 4;
            var minOperations = 3;
            var maxOperations = 1;

            var expressions = service.Generate(expectedProblems, minOperations, maxOperations);
        }
    }
}
