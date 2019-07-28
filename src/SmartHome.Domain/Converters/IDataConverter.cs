namespace SmartHome.Core.Domain.Converters
{
    public interface IDataConverter
    {
        string Convert(string input);
        string ReverseConvert(string input);
    }
}