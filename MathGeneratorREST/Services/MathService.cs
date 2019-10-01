using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MathGeneratorREST.Services
{
    public class MathService : IMathService
    {
        private readonly List<char> _operationsList;
        private const int DEFAULT_MAX_NUM = 20;
        private const int DEFAULT_MIN_NUM = 0;
        private const int DEFAULT_NUM_OPERATIONS = 10;

        public MathService()
        {
            _operationsList = new List<char>
            {
                '+',
                '-',
                '/',
                '*'
            };
        }

        public string Generate(int numProblems, int? minOperations, int? maxOperations)
        {
            if (!ValidateInputs(numProblems, minOperations, maxOperations, out var result))
                return result;

            string problems = string.Empty;

            for (int i = 0; i < numProblems; i++)
            {
                var operationsArray = GenerateOperationsArray(minOperations, maxOperations);
                var numbersArray = GenerateNumbersArray(operationsArray.Count + 1);
                var expression = MergeArrays(operationsArray, numbersArray);
                expression += "\n";

                problems += expression;

            }

            return problems;
        }

        private bool ValidateInputs(in int numProblems, in int? minOperations, in int? maxOperations, out string result)
        {
            if (numProblems == 0)
            {
                result = "No problems to generate";
                return false;
            }

            if (numProblems < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numProblems));
            }

            //validate min operations
            if (minOperations.HasValue)
            {
                if (minOperations.Value < 0 || minOperations.Value > _operationsList.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(minOperations));
                }
            }

            //validate max operations
            if (maxOperations.HasValue)
            {
                if (maxOperations.Value < 0 || maxOperations.Value > _operationsList.Count)
                    throw new ArgumentOutOfRangeException(nameof(maxOperations));
            }

            //check if min > max
            if (maxOperations.HasValue && minOperations.HasValue)
                if (minOperations > maxOperations)
                    throw new ArgumentException("Min number of operations cannot be less than max");

            //all checks pass
            result = String.Empty;
            return true;


        }

        private string MergeArrays(List<char> operations, List<int> numbers)
        {
            string expression = $"{numbers.FirstOrDefault()} ";

            for (int i = 0; i < operations.Count; i++)
            {
                expression += operations[i] + " ";
                expression += numbers[i+1] + " ";
            }

            var cleanExpression = DivideByZeroCheck(expression);

            return cleanExpression;
        }

        private string DivideByZeroCheck(string expression)
        {
            var substitue = RandomNumberGenerator.GetInt32(DEFAULT_MIN_NUM, DEFAULT_MAX_NUM);
            return expression.Replace("/ 0", $"/ {substitue}");
        }

        private List<char> GenerateOperationsArray(int? minOperations, int? maxOperations)
        {
            Random rng = new Random();
            var operationsArray = new List<char>();
            var usedOperations = new List<char>();
            var validateFlag = false;

            //regenerate new problem until it meets conditions
            while (!validateFlag)
            {
                operationsArray.Clear();
                usedOperations.Clear();

                //difficulty set
                var length = rng.Next(1, DEFAULT_NUM_OPERATIONS);

                for (int i = 0; i < length; i++)
                {
                    var operation = _operationsList[rng.Next(0, _operationsList.Count)];
                    operationsArray.Add(operation);
                    if (!usedOperations.Contains(operation))
                        usedOperations.Add(operation);

                }
                validateFlag = ValidateOperationsArray(minOperations, maxOperations, usedOperations);
            }

            return operationsArray;
        }

        private bool ValidateOperationsArray(int? minOperations, int? maxOperations, List<char> usedOperations)
        {
            var minFlag = true;
            var maxFlag = true;

            if (minOperations.HasValue)
                if (minOperations > usedOperations.Count)
                    minFlag = false;

            if (maxOperations.HasValue)
                if (maxOperations < usedOperations.Count)
                    maxFlag = false;

            return minFlag && maxFlag;
        }

        private List<int> GenerateNumbersArray(int size)
        {
            var numbersArray = new List<int>();

            for (int i = 0; i < size; i++)
                numbersArray.Add(RandomNumberGenerator.GetInt32(DEFAULT_MIN_NUM, DEFAULT_MAX_NUM));

            return numbersArray;
        }
    }
}
