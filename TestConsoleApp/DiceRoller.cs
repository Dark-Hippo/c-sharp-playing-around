using System;

namespace TestConsoleApp
{
    public static class DiceRoller
    {
        private static int minRoll = 1;

        public static int d6()
        {
            return roll(7);
        }

        public static int d8()
        {
            return roll(9);
        }

        public static int d10()
        {
            return roll(11);
        }

        public static int d12()
        {
            return roll(13);
        }

        public static int d20()
        {
            return roll(21);
        }

        private static int roll(int maxRoll)
        {
            return new Random().Next(minRoll, maxRoll);
        }
    }
}
