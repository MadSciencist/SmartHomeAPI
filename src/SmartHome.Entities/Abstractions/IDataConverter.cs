namespace SmartHome.Core.Entities.Abstractions
{
    public interface IDataConverter
    {
        string Convert(string input);
        string ReverseConvert(string input);
    }
}