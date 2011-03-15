namespace MIProgram.StylesParser
{
    public class StyleDefinition : IFieldDefinition
    {
        public int? MainStyleIdx { get; private set; }
        public int? SecondStyleIdx { get; private set; }
        public int? StyleAlterationIdx { get; private set; }

        public static readonly ParsedValueRepository MainStyles = new ParsedValueRepository();
        public static readonly ParsedValueRepository StyleAlterations = new ParsedValueRepository();

        public StyleDefinition(int? mainStyleIdx)
        {
            MainStyleIdx = mainStyleIdx;
        }

        public StyleDefinition(int? mainStyleIdx, int? secondStyleIdx) : this(mainStyleIdx)
        {
            SecondStyleIdx = secondStyleIdx;
        }

        public StyleDefinition(int? mainStyleIdx, int? secondStyleIdx, int? styleAlterationIdx) : this(mainStyleIdx, secondStyleIdx)
        {
            StyleAlterationIdx = styleAlterationIdx;
        }

        public string RebuildFromParsedValuesRepository()
        {
            var result = string.Empty;
            if (MainStyleIdx.HasValue)
            {
                result += MainStyles.Values[MainStyleIdx.Value];
            }
            if (MainStyleIdx.HasValue && SecondStyleIdx.HasValue)
            {
                result += " / ";
            }
            if (SecondStyleIdx.HasValue)
            {
                result += MainStyles.Values[SecondStyleIdx.Value];
            }
            if (StyleAlterationIdx.HasValue)
            {
                result += string.Format(" {0}", StyleAlterations.Values[StyleAlterationIdx.Value]);
            }

            return result;
        }
    }
}
