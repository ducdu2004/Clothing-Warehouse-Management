using System;
using System.Collections.Generic;

namespace quanliquanao3.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public int? CategoryId { get; set; }

    public string? Size { get; set; }

    public string? Color { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public Boolean? Status { get; set; }

    public virtual ProductCategory? Category { get; set; }

    public virtual ICollection<ExportDetail> ExportDetails { get; set; } = new List<ExportDetail>();

    public virtual ICollection<ImportDetail> ImportDetails { get; set; } = new List<ImportDetail>();
}
