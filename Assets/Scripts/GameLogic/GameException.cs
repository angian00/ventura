using System;

namespace Ventura.GameLogic
{
    public class GameException: Exception
    {
        private string message;
        private string expected;
        private string actual;

        public GameException(string message, string expected, string actual)
        {
            this.message = message;
            this.expected = expected;
            this.actual = actual;
        }
    }
}
