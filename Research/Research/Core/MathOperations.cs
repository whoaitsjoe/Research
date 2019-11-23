using System;
namespace Research.Core
{
    public class MathOperations
    {
        //currently for positive integers, change to long or uint as necessary 
        public static int GCD(int firstNumber, int secondNumber)
        {
            while (firstNumber != 0 && secondNumber != 0)
            {
                if (firstNumber > secondNumber)
                    firstNumber %= secondNumber;
                else
                    secondNumber %= firstNumber;
            }

            return firstNumber == 0 ? secondNumber : firstNumber;
        }
    }
}
