namespace TestApp.Data.Interfaces
{
    public interface IDaoFactory
    {
        ICartDao CartDao { get; }
        ICartReportDao CartReportDao { get; }
        IDeliveryDao DeliveryDao { get; }
        IOrderDao OrderDao { get; }
        IProductDao ProductDao { get; }
        IUserLocationDao UserLocationDao { get; }
    }
}