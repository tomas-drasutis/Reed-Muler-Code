using NUnit.Framework;
using Reed_Muler_Code.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    public class Experiments
    {

        [TestCase(0.10)]
        [TestCase(0.11)]
        [TestCase(0.12)]
        [TestCase(0.13)]
        [TestCase(0.14)]
        [TestCase(0.15)]
        [TestCase(0.16)]
        [TestCase(0.17)]
        [TestCase(0.18)]
        [TestCase(0.19)]
        [TestCase(0.20)]
        [Test]
        public void StringService_M5R1_Succeeds(double errorProbability)
        {
            int m = 5;
            int r = 1;
            int errorRate = 0;
            string message = "ThisIsATestMessage1234567890!@#$"; //32
            StringService stringService = new StringService();

            for (int i = 0; i < 100; i++)
            {
                string response = stringService.HandleStringWithEncoding(message, m, r, errorProbability);

                if (!message.Equals(response))
                    errorRate++;
            }

            Console.WriteLine($"M={m}, R={r}, Error Probability={errorProbability}, Error Rate={errorRate}");
        }

        [TestCase(0.10)]
        [TestCase(0.11)]
        [TestCase(0.12)]
        [TestCase(0.13)]
        [TestCase(0.14)]
        [TestCase(0.15)]
        [TestCase(0.16)]
        [TestCase(0.17)]
        [TestCase(0.18)]
        [TestCase(0.19)]
        [TestCase(0.20)]
        [Test]
        public void StringService_M5R2_Succeeds(double errorProbability)
        {
            int m = 5;
            int r = 2;
            int errorRate = 0;
            string message = "ThisIsATestMessage1234567890!@#$"; //32
            StringService stringService = new StringService();

            for (int i = 0; i < 100; i++)
            {
                string response = stringService.HandleStringWithEncoding(message, m, r, errorProbability);

                if (!message.Equals(response))
                    errorRate++;
            }

            Console.WriteLine($"M={m}, R={r}, Error Probability={errorProbability}, Error Rate={errorRate}");
        }

        [TestCase(0.10)]
        [TestCase(0.11)]
        [TestCase(0.12)]
        [TestCase(0.13)]
        [TestCase(0.14)]
        [TestCase(0.15)]
        [TestCase(0.16)]
        [TestCase(0.17)]
        [TestCase(0.18)]
        [TestCase(0.19)]
        [TestCase(0.20)]
        [Test]
        public void StringService_M6R1_Succeeds(double errorProbability)
        {
            int m = 6;
            int r = 1;
            int errorRate = 0;
            string message = "ThisIsATestMessage1234567890!@#$"; //32
            StringService stringService = new StringService();

            for (int i = 0; i < 100; i++)
            {
                string response = stringService.HandleStringWithEncoding(message, m, r, errorProbability);

                if (!message.Equals(response))
                    errorRate++;
            }

            Console.WriteLine($"M={m}, R={r}, Error Probability={errorProbability}, Error Rate={errorRate}");
        }

        [TestCase(0.10)]
        [TestCase(0.11)]
        [TestCase(0.12)]
        [TestCase(0.13)]
        [TestCase(0.14)]
        [TestCase(0.15)]
        [TestCase(0.16)]
        [TestCase(0.17)]
        [TestCase(0.18)]
        [TestCase(0.19)]
        [TestCase(0.20)]
        [Test]
        public void StringService_M6R2_Succeeds(double errorProbability)
        {
            int m = 6;
            int r = 2;
            int errorRate = 0;
            string message = "ThisIsATestMessage1234567890!@#$"; //32
            StringService stringService = new StringService();

            for (int i = 0; i < 100; i++)
            {
                string response = stringService.HandleStringWithEncoding(message, m, r, errorProbability);

                if (!message.Equals(response))
                    errorRate++;
            }

            Console.WriteLine($"M={m}, R={r}, Error Probability={errorProbability}, Error Rate={errorRate}");
        }

        

    }
}
