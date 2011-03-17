namespace MIProgram.Core.DataParsers
{
    public interface IFieldParser<T, TU> where T : IToDomainObject<TU>
    {
        bool TryParse(string textToParse, int reviewId, ref T fieldDefinition);
    }
}