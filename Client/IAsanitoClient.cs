using System.Collections.Generic;
using System.Threading.Tasks;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Category;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Customer;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Invoice;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Product;
using Tesla.Plugin.Widgets.CRM.Asanito.DTO.Warehouse;

namespace Tesla.Plugin.Widgets.CRM.Asanito.Client
{
    public interface IAsanitoClient
    {
        Task<CreateCustomerResponseDto> CreateCustomer(CreateCustomerRequestDto customer);

        Task<UpdateCustomerResponseDto> UpdateCustomer(UpdateCustomerRequestDto customer);

        Task<GetCustomersResponseDto> GetCustomers(GetCustomersRequestDto body);

        Task<bool> DeleteCustomer(int id);

        Task<CreateCustomerAddressResponseDto> CreateCustomerAddress(CreateCustomerAddressRequestDto address);

        Task<List<GetProvinceResponseDto>> GetProvince(GetProvinceAndCityRequestDto body);

        Task<List<GetCityResponseDto>> GetCity(GetProvinceAndCityRequestDto body, int id);

        Task<CreateCategoryResponseDto> CreateCategory(CreateCategoryRequestDto category);

        Task<UpdateCategoryResponseDto> UpdateCategory(UpdateCategoryRequestDto category);

        Task<bool> DeleteCategory(int id);

        Task<GetProductResponseDto> GetProducts(GetProductRequestDto body);

        Task<CreateProductResponseDto> CreateProduct(CreateProductRequestDto product);

        Task<UpdateProductResponseDto> UpdateProduct(UpdateProductRequestDto product);

        Task<bool> DeleteProduct(int id);

        Task<GetWarehouseResponseDto> GetWarehouse();

        Task<GetWarehouseStockResponseDto> GetWarehouseStock(int id);

        Task<bool> UpdateWarehouse(List<UpdateWarehouseRequestDto> warehouse, int id);

        Task<CreateInvoiceResponseDto> CreateInvoice(CreateInvoiceRequestDto invoice);

        Task<CreateSettledInvoiceResponseDto> CreateSettledInvoice(CreateSettledInvoiceRequestDto invoice);

        Task<UpdateInvoiceResponseDto> UpdateInvoice(UpdateInvoiceRequestDto invoice);

        Task<bool> UpdateInvoiceStatus(UpdateInvoiceStatusRequestDto invoiceStatus);

        Task<CreateOperationIncomeResponseDto> CreateOperationIncome(CreateOperationIncomeRequestDto income);
    }
}