namespace RestApiWithDontNet.Data.Converter.Contracts
{
    public interface IParser<I, O>
    {
        O? Parse(I? input);
        List<O?> Parse(List<I?> input);
    }
}
