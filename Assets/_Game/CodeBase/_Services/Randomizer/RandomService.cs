using Random = UnityEngine.Random;

namespace CodeBase._Services.Randomizer
{
    public class RandomService : IService
    {
        public int Next(int min, int max) => Random.Range(min, max);
    }
}