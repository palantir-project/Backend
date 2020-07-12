namespace Palantir.Domain.Adapters
{
    using Palantir.Domain.Configurations;

    public interface IWidgetConfigurationAdapter
    {
        WidgetConfiguration GetWidgetConfiguration();

        void ValidateWidgetConfiguration(RestApiConfiguration adaptee);
    }
}