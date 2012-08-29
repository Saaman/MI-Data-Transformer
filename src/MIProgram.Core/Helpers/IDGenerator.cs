namespace MIProgram.Core.Helpers
{
    public class IDGenerator
    {
        private int _curentId;

        public int NewID()
        {
            return ++_curentId;
        }
    }
}