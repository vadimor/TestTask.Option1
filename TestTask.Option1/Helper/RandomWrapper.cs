using TestTask.Option1.Helper.Interfaces;

namespace TestTask.Option1.Helper
{
    // This class provides a wrapper for working with random numbers 

    public class RandomWrapper : IRandomWrapper
    {
        private readonly Random _random;

        public RandomWrapper()
        {
            _random = new Random();
        }

        public float NextSingle()
        {
            return _random.NextSingle();
        }
    }
}
