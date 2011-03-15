using MIProgram.WorkingModel;

namespace MIProgram.StylesParser
{
    public interface IFieldParser<T> where T : IFieldDefinition
    {
        bool TryParse(Review review, ref T fieldDefinition);
    }
}