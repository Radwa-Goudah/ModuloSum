using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Problem
{

    public class Problem : ProblemBase, IProblem
    {
        #region ProblemBase Methods
        public override string ProblemName { get { return "ModuloSum"; } }

        public override void TryMyCode()
        {
            int M = 0;
            int N = 0;
            bool outputVal, expectedVal;
            int[] outputNumbers;

            {
                M = 2;
                int[] items = { 1000000000 };
                N = items.Length;
                expectedVal = true;
                outputVal = ModuloSum.SolveValue(items, N, M);
                outputNumbers = ModuloSum.ConstructSolution(items, N, M);
                PrintCase(M, items, outputVal, outputNumbers, expectedVal);
            }
            {
                M = 6;
                int[] items = { 5 };
                N = items.Length;
                expectedVal = false;
                outputVal = ModuloSum.SolveValue(items, N, M);
                outputNumbers = ModuloSum.ConstructSolution(items, N, M);
                PrintCase(M, items, outputVal, outputNumbers, expectedVal);
            }
            {
                M = 6;
                int[] items = { 3, 1, 1, 3 };
                N = items.Length;
                expectedVal = true;
                outputVal = ModuloSum.SolveValue(items, N, M);
                outputNumbers = ModuloSum.ConstructSolution(items, N, M);
                PrintCase(M, items, outputVal, outputNumbers, expectedVal);
            }
            {
                M = 6;
                int[] items = { 6, 6, 6, 6, 6, 6 };
                N = items.Length;
                expectedVal = true;
                outputVal = ModuloSum.SolveValue(items, N, M);
                outputNumbers = ModuloSum.ConstructSolution(items, N, M);
                PrintCase(M, items, outputVal, outputNumbers, expectedVal);
            }
            {
                M = 1;
                int[] items = { 7, 6, 5, 4, 3 };
                N = items.Length;
                expectedVal = true;
                outputVal = ModuloSum.SolveValue(items, N, M);
                outputNumbers = ModuloSum.ConstructSolution(items, N, M);
                PrintCase(M, items, outputVal, outputNumbers, expectedVal);
            }
        }



        Thread tstCaseThr;
        bool caseTimedOut;
        bool caseException;

        protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
        {
            int testCases;
            int M = 0, N = 0;
            int[] numbers = null;
            bool output = false;
            bool actualResult = false;
            int j = 0;

            Stream s = new FileStream(fileName, FileMode.Open);
            BinaryReader br = new BinaryReader(s);

            testCases = br.ReadInt32();

            int totalCases = testCases;
            int[] correctCases = new int[2];
            int[] wrongCases = new int[2];
            int[] timeLimitCases = new int[2];
            for (int i = 0; i < 2; i++)
            {
                correctCases[i] = 0;
                wrongCases[i] = 0;
                timeLimitCases[i] = 0;
            }
            bool readTimeFromFile = false;
            if (timeOutInMillisec == -1)
            {
                readTimeFromFile = true;
            }
            float maxTime = float.MinValue;
            float avgTime = 0;
            for (int i = 1; i <= testCases; i++)
            {
                M = br.ReadInt32();
                N = br.ReadInt32();
                numbers = new int[N];

                for (j = 0; j < N; j++)
                {
                    numbers[j] = br.ReadInt32();
                }
                actualResult = br.ReadBoolean();
                if (readTimeFromFile)
                {
                    timeOutInMillisec = int.Parse(br.ReadString().Split(':')[1]);
                }

                Console.WriteLine("\n===========================");
                Console.WriteLine("CASE#{0}: M = {1}, N = {2}", i, M, N);
                Console.WriteLine("===========================");

                for (int c = 0; c < 2; c++)
                {
                    caseTimedOut = true;
                    Stopwatch sw = null;
                    caseException = false;
                    int[] outputVals = null;
                    {
                        tstCaseThr = new Thread(() =>
                        {
                            try
                            {
                                sw = Stopwatch.StartNew();
                                if (c == 0)
                                {
                                    output = ModuloSum.SolveValue(numbers, N, M);
                                }
                                else
                                {
                                    outputVals = ModuloSum.ConstructSolution(numbers, N, M);
                                    if (outputVals != null)
                                        output = true;
                                    else
                                        output = false;
                                }
                                sw.Stop();
                                Console.WriteLine("time = {0} ms", sw.ElapsedMilliseconds);
                                //Console.WriteLine("output = {0}", output);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                caseException = true;
                                output = false;
                                outputVals = null;
                            }
                            caseTimedOut = false;
                        });

                        //StartTimer(timeOutInMillisec);
                        tstCaseThr.Start();
                        tstCaseThr.Join(timeOutInMillisec);
                    }

                    if (caseTimedOut)       //Timedout
                    {
                        tstCaseThr.Abort();
                        Console.WriteLine("Time Limit Exceeded in Case {0} [FUNCTION#{1}].", i, c + 1);
                        timeLimitCases[c]++;
                    }
                    else if (caseException) //Exception 
                    {
                        Console.WriteLine("Exception in Case {0} [FUNCTION#{1}].", i, c + 1);
                        wrongCases[c]++;
                    }
                    else if (output == actualResult)    //Passed
                    {
                        if (c == 0)
                        {
                            Console.WriteLine("Test Case {0} [FUNCTION#{1}] Passed!", i, c + 1);
                            correctCases[c]++;
                        }
                        else if (CheckOutput(numbers, outputVals, M, output, actualResult))
                        {
                            Console.WriteLine("Test Case {0} [FUNCTION#{1}] Passed!", i, c + 1);
                            correctCases[c]++;
                        }
                        else
                        {
                            Console.WriteLine("Wrong Answer in Case {0} [FUNCTION#{1}].", i, c + 1);
                            wrongCases[c]++;
                        }
                        //maxTime = Math.Max(maxTime, sw.ElapsedMilliseconds);
                        //avgTime += sw.ElapsedMilliseconds;
                    }
                    else                    //WrongAnswer
                    {
                        Console.WriteLine("Wrong Answer in Case {0} [FUNCTION#{1}].", i, c + 1);
                        if (level == HardniessLevel.Easy)
                        {
                            PrintCase(M, numbers, output, outputVals, actualResult, false);
                        }
                        wrongCases[c]++;
                    }
                }
            }
            s.Close();
            br.Close();
            for (int c = 0; c < 2; c++)
            {
                Console.WriteLine("EVALUATION OF FUNCTION#{0}:", c + 1);
                Console.WriteLine("# correct = {0}", correctCases[c]);
                Console.WriteLine("# time limit = {0}", timeLimitCases[c]);
                Console.WriteLine("# wrong = {0}", wrongCases[c]);
                //Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0));
                //Console.WriteLine("AVERAGE EXECUTION TIME (ms) = {0}", Math.Round(avgTime / (float)correctCases, 2));
                //Console.WriteLine("MAX EXECUTION TIME (ms) = {0}", maxTime); 
            }
            Console.WriteLine("\nFINAL EVALUATION: FUNCTION#1 (%), FUNCTION#2 (%) = {0} {1}", Math.Round((float)correctCases[0] / totalCases * 100, 0), Math.Round((float)correctCases[1] / totalCases * 100, 0));

        }



        protected override void OnTimeOut(DateTime signalTime)
        {
        }

        public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helper Methods
        private static void PrintNums(int[] X)
        {

            int N = X.Length;

            for (int i = 0; i < N; i++)
            {
                Console.Write(X[i] + "  ");
            }
            Console.WriteLine();
        }

        private void PrintCase(int M, int[] items, bool outputVal, int[] outputNumbers, bool expectedVal, bool check = true)
        {
            Console.WriteLine("B = {0}", M);
            Console.Write("numbers = "); PrintNums(items);
            Console.WriteLine("expected value = {0}", expectedVal);
            Console.WriteLine("output value = {0}", outputVal);
            if (outputNumbers != null)
            {
                Console.Write("output numbers = "); PrintNums(outputNumbers);
            }

            if (check)
            {
                if (outputVal != expectedVal)
                {
                    Console.WriteLine("WRONG");
                }
                else
                {
                    if (CheckOutput(items, outputNumbers, M, outputVal, expectedVal)) Console.WriteLine("CORRECT");
                    else Console.WriteLine("WRONG");
                }
            }
            Console.WriteLine();
        }

        private bool CheckOutput(int[] items, int[] outputNumbers, int M, bool output, bool expectedResult)
        {
            if (outputNumbers == null)
            {
                if (expectedResult == true)
                    return false;
                else
                    return true;
            }

            int L = outputNumbers.Length;
            int N = items.Length;
            if (L < 1 || L > N)
            {
                Console.WriteLine("WRONG: invalid length for the returned subsequence");
                return false;
            }


            if (output != expectedResult)
            {
                Console.WriteLine("WRONG");
                return false;
            }
            bool[] taken = new bool[N];
            for (int i = 0; i < N; i++)
            {
                taken[i] = false;
            }
            for (int i = 0; i < L; i++)
            {
                bool found = false;
                for (int j = 0; j < N; j++)
                {
                    if (outputNumbers[i] == items[j] && !taken[j])
                    {
                        found = true;
                        taken[j] = true;
                        break;
                    }
                }
                if (!found)
                {
                    Console.WriteLine("WRONG: {0} not exists in the given array of numbers OR taken more than one time", outputNumbers[i]);
                    return false;
                }
            }
            int sum = 0;
            for (int i = 0; i < L; i++)
            {
                sum += outputNumbers[i];
                sum %= M;
            }
            if (sum != 0)
            {
                Console.WriteLine("WRONG: sum of returned numbers NOT DIVISIBLE BY given value (M = {0})", M);
                return false;
            }

            return true;
        }
        #endregion

    }
}