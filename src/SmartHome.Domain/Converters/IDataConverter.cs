namespace SmartHome.Core.Entities.Converters
{
    public interface IDataConverter
    {
        string Convert(string input);
        string ReverseConvert(string input);
    }
}