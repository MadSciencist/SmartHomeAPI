namespace SmartHome.Core.BusinessLogic
{
    public interface IRestTemplateBuilder
    {
        string BuildBody();
        string BuildUrl();
    }
}