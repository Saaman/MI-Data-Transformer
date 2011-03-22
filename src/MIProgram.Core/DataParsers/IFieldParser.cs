namespace MIProgram.Core.DataParsers
{
    public interface IFieldParser<T>
    {
        bool TryParse(string textToParse, int reviewId, ref T fieldDefinition);
    }
}