using ClosedXML.Excel;
using NhjDotnetApi.Application.Contracts;
using NhjDotnetApi.Persistence;
using Microsoft.EntityFrameworkCore;

namespace NhjDotnetApi.Application.Services;

public class OrderExportService : IOrderExportService
{
    private readonly AppDbContext _db;

    public OrderExportService(AppDbContext db)
    {
        _db = db;
    }

    public byte[] ExportOrdersToExcel(Guid userId, bool isAdmin)
    {
        var ordersQuery = _db.Orders
            .Include(o => o.Items)
            .AsQueryable();

        if (!isAdmin)
            ordersQuery = ordersQuery.Where(o => o.UserId == userId);

        var orders = ordersQuery.ToList();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Orders");

        // Header
        worksheet.Cell(1, 1).Value = "Order ID";
        worksheet.Cell(1, 2).Value = "Customer";
        worksheet.Cell(1, 3).Value = "Total Amount";
        worksheet.Cell(1, 4).Value = "Status";
        worksheet.Cell(1, 5).Value = "Created At";

        var row = 2;
        foreach (var order in orders)
        {
            worksheet.Cell(row, 1).Value = order.Id.ToString();
            worksheet.Cell(row, 2).Value = order.CustomerName;
            worksheet.Cell(row, 3).Value = order.TotalAmount;
            worksheet.Cell(row, 4).Value = order.Status.ToString();
            worksheet.Cell(row, 5).Value = order.CreatedAt;

            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        return stream.ToArray();
    }
}
