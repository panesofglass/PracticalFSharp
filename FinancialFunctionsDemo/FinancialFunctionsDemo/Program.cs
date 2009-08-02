namespace FinancialFunctionsDemo
{
    using System;
    using System.Diagnostics;
    using System.Numeric;

    using Microsoft.Office.Interop.Excel;

    internal class Program
    {
        /// <summary>
        /// Test out some cool Excel financial functions in C# via F#.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <seealso href="http://office.microsoft.com/en-us/excel/CH062528251033.aspx"/>
        private static void Main(string[] args)
        {
            Console.WriteLine("Testing accrued interest with F# ...");
            TestAccrInt();

            Console.WriteLine("Testing accrued interest with Excel ...");
            TestExcelAccrInt();
        }

        /// <summary>
        /// Tests the AccrInt function.
        /// </summary>
        /// <seealso href="http://office.microsoft.com/en-us/excel/HP052089791033.aspx"/>
        private static void TestAccrInt()
        {
            var issue = new DateTime(2008, 3, 1);
            var firstInterest = new DateTime(2008, 8, 31);
            var settlement = new DateTime(2008, 5, 1);
            var rate = 0.10;
            var par = 1000;
            var frequency = Frequency.SemiAnnual;
            var basis = DayCountBasis.UsPsa30_360;

            var sw = Stopwatch.StartNew();
            double interest = Financial.AccrInt(issue, firstInterest, settlement, rate, par, frequency, basis);
            Console.WriteLine("Calculated accrued interest of {0:c} in {1} milliseconds", interest, sw.ElapsedMilliseconds);
            sw.Stop();
        }

        /// <summary>
        /// Tests the Excel AccrInt function.
        /// </summary>
        private static void TestExcelAccrInt()
        {
            var issue = new DateTime(2008, 3, 1);
            var firstInterest = new DateTime(2008, 8, 31);
            var settlement = new DateTime(2008, 5, 1);
            var rate = 0.10;
            var par = 1000;
            var frequency = Frequency.SemiAnnual;
            var basis = DayCountBasis.UsPsa30_360;

            var excel = new ApplicationClass();
            var worksheetFunction = excel.WorksheetFunction;

            var sw = Stopwatch.StartNew();
            double interest = worksheetFunction.AccrInt(issue, firstInterest, settlement, rate, par, frequency, basis);
            Console.WriteLine("Calculated accrued interest of {0:c} in {1} milliseconds", interest, sw.ElapsedMilliseconds);
            sw.Stop();
        }
    }
}