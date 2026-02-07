namespace NhjDotnetApi.Application.Contracts;

public interface IOrderExportService
{
    byte[] ExportOrdersToExcel(Guid userId, bool isAdmin);
}
