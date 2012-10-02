namespace MIProgram.Core.Helpers
{
    public class IDGenerator
    {
        private int _currentId;

        public int NewID()
        {
            return ++_currentId;
        }

        public IDGenerator() {}
        public IDGenerator(int startIdx)
        {
            _currentId = startIdx;
        }
    }
}