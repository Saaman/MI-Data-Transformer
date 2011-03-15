namespace MIProgram.Core.DataParsers
{
    public interface IFieldParser<T, TU>
        where T : IFieldDefinition
        where TU : IFieldDefinition
    {
        bool TryParse(string textToParse, int reviewId, ref T fieldDefinition);
        TU ConvertToDestFieldDefinition(T fieldDefinition);
    }
}