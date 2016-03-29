using System;
using System.Collections.Generic;
using System.Globalization;

namespace VilRan.Mathematics
{
    public class Formula
    {
        private Operation Root;

        /// <summary>
        /// Supported operations:
        ///     + Addition,
        ///     - Subtraction and Negation,
        ///     * Multiplication,
        ///     / Division,
        ///     % Modulo,
        ///     ^ Exponentiation
        ///     sin Sine
        ///     cos Cosine
        ///     tan Tangent
        ///     abs Absolute value
        /// </summary>
        /// <param name="formula"></param>
        public Formula(string formula)
        {
            Root = Operation.Construct(formula);
        }

        /// <summary>
        /// Use this overload if the equation uses only one variable (or if all variables have the same value.)
        /// </summary>
        /// <param name="variable">Value of the variable.</param>
        /// <returns></returns>
        public double Solve(double variable)
        {
            return Root.Solve(variable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables">Dictionary of variable names and values.</param>
        /// <returns></returns>
        public double Solve(Dictionary<string, double> variables)
        {
            return Root.Solve(variables);
        }

        /// <summary>
        /// Define one variable per parameter using the format "[variable] = [value]", for example "x = 7.4".
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        public double Solve(params string[] variables)
        {
            Dictionary<string, double> dictionary = new Dictionary<string, double>();
            foreach (string s in variables)
            {
                int i = s.IndexOf('=');
                string key = s.Substring(0, i - 1).Trim();
                double value = double.Parse(s.Substring(i + 1), CultureInfo.InvariantCulture);
                dictionary.Add(key, value);
            }
            return Root.Solve(dictionary);
        }

        private abstract class Operation
        {
            public abstract double Solve(double variable);
            public abstract double Solve(Dictionary<string, double> variables);

            public static Operation Construct(string formula)
            {
                formula = formula.Trim();

                while (true)
                {
                    int charIndex = -1, operatorPosition = -1, bracketLevel = 0;
                    char operation = '\0';
                    foreach (char charCurrent in formula)
                    {
                        charIndex++;
                        switch (charCurrent)
                        {
                            case '(':
                                bracketLevel++;
                                break;
                            case ')':
                                bracketLevel--;
                                break;
                            case '+':
                            case '-':
                            case '*':
                            case '/':
                            case '%':
                            case '^':
                                if (bracketLevel == 0 && charIndex > 0
                                    && !HasOperatorPrecedence(charCurrent, operation))
                                {
                                    operatorPosition = charIndex;
                                    operation = charCurrent;
                                }
                                break;
                        }
                    }

                    if (operation != '\0')
                    {
                        string a = formula.Substring(0, operatorPosition - 1);
                        string b = formula.Substring(operatorPosition + 1);
                        switch (operation)
                        {
                            case '+': return new Addition(Construct(a), Construct(b));
                            case '-': return new Subtraction(Construct(a), Construct(b));
                            case '*': return new Multiplication(Construct(a), Construct(b));
                            case '/': return new Division(Construct(a), Construct(b));
                            case '%': return new Modulo(Construct(a), Construct(b));
                            case '^': return new Exponentation(Construct(a), Construct(b));
                        }
                    }

                    if (formula[0] == '(' && formula[formula.Length - 1] == ')')
                        formula = formula.Substring(1, formula.Length - 2);
                    else
                        break;
                }

                if (formula[0] == '-')
                {
                    string a = formula.Substring(1);
                    return new Negation(Construct(a));
                }

                if (formula.Length > 3)
                {
                    string firstThree = formula.Substring(0, 3);
                    string a = formula.Substring(3);
                    switch (firstThree)
                    {
                        case "sin":
                            return new Sine(Construct(a));
                        case "cos":
                            return new Cosine(Construct(a));
                        case "tan":
                            return new Tangent(Construct(a));
                        case "abs":
                            return new Absolute(Construct(a));
                    }

                }

                double constant;
                if (double.TryParse(formula, NumberStyles.Any, CultureInfo.InvariantCulture, out constant))
                {
                    return new Constant(constant);
                }

                return new Variable(formula);
            }

            private static bool HasOperatorPrecedence(char newOperation, char oldOperation)
            {
                switch (oldOperation)
                {
                    case '+':
                    case '-':
                        switch (newOperation)
                        {
                            case '*':
                            case '/':
                            case '%':
                            case '^':
                                return true;
                        }
                        break;
                    case '*':
                    case '/':
                    case '%':
                        if (newOperation == '^')
                            return true;
                        break;
                    case '^':
                        return false;
                    default:
                        return false;
                }

                return false;
            }
        }
        private class Constant : Operation
        {
            public double Value;
            public Constant(double value) { Value = value; }
            public override double Solve(double variable) { return Value; }
            public override double Solve(Dictionary<string, double> variables) { return Value; }
        }
        private class Variable : Operation
        {
            public string Key;
            public Variable(string value) { Key = value; }
            public override double Solve(double variable) { return variable; }
            public override double Solve(Dictionary<string, double> variables) { return variables[Key]; }
        }
        private class Negation : Operation
        {
            private Operation A;
            public Negation(Operation a) { A = a; }
            public override double Solve(double variable) { return -A.Solve(variable); }
            public override double Solve(Dictionary<string, double> variables) { return -A.Solve(variables); }
        }
        private class Addition : Operation
        {
            private Operation A, B;
            public Addition(Operation a, Operation b) { A = a; B = b; }
            public override double Solve(double variable) { return A.Solve(variable) + B.Solve(variable); }
            public override double Solve(Dictionary<string, double> variables) { return A.Solve(variables) + B.Solve(variables); }
        }
        private class Subtraction : Operation
        {
            private Operation A, B;
            public Subtraction(Operation a, Operation b) { A = a; B = b; }
            public override double Solve(double variable) { return A.Solve(variable) - B.Solve(variable); }
            public override double Solve(Dictionary<string, double> variables) { return A.Solve(variables) - B.Solve(variables); }
        }
        private class Multiplication : Operation
        {
            private Operation A, B;
            public Multiplication(Operation a, Operation b) { A = a; B = b; }
            public override double Solve(double variable) { return A.Solve(variable) * B.Solve(variable); }
            public override double Solve(Dictionary<string, double> variables) { return A.Solve(variables) * B.Solve(variables); }
        }
        private class Division : Operation
        {
            private Operation A, B;
            public Division(Operation a, Operation b) { A = a; B = b; }
            public override double Solve(double variable) { return A.Solve(variable) / B.Solve(variable); }
            public override double Solve(Dictionary<string, double> variables) { return A.Solve(variables) / B.Solve(variables); }
        }
        private class Modulo : Operation
        {
            private Operation A, B;
            public Modulo(Operation a, Operation b) { A = a; B = b; }
            public override double Solve(double variable) { return A.Solve(variable) % B.Solve(variable); }
            public override double Solve(Dictionary<string, double> variables) { return A.Solve(variables) % B.Solve(variables); }
        }
        private class Exponentation : Operation
        {
            private Operation A, B;
            public Exponentation(Operation a, Operation b) { A = a; B = b; }
            public override double Solve(double variable) { return Math.Pow(A.Solve(variable), B.Solve(variable)); }
            public override double Solve(Dictionary<string, double> variables) { return Math.Pow(A.Solve(variables), B.Solve(variables)); }
        }
        private class Sine : Operation
        {
            private Operation A;
            public Sine(Operation a) { A = a; }
            public override double Solve(double variable) { return Math.Sin(variable); }
            public override double Solve(Dictionary<string, double> variables) { return -A.Solve(variables); }
        }
        private class Cosine : Operation
        {
            private Operation A;
            public Cosine(Operation a) { A = a; }
            public override double Solve(double variable) { return Math.Cos(variable); }
            public override double Solve(Dictionary<string, double> variables) { return -A.Solve(variables); }
        }
        private class Tangent : Operation
        {
            private Operation A;
            public Tangent(Operation a) { A = a; }
            public override double Solve(double variable) { return Math.Tan(variable); }
            public override double Solve(Dictionary<string, double> variables) { return -A.Solve(variables); }
        }
        private class Absolute : Operation
        {
            private Operation A;
            public Absolute(Operation a) { A = a; }
            public override double Solve(double variable) { return Math.Abs(variable); }
            public override double Solve(Dictionary<string, double> variables) { return -A.Solve(variables); }
        }
    }
}
