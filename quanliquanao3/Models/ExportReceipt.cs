using System;
using System.Collections.Generic;

namespace quanliquanao3.Models;

public partial class ExportReceipt
{
    public int ReceiptId { get; set; }

    public DateTime ExportDate { get; set; }

    public string? ExportedBy { get; set; }

    public string? Note { get; set; }

    public int? CustomerId { get; set; }

    public int? UserId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<ExportDetail> ExportDetails { get; set; } = new List<ExportDetail>();

    public virtual User? User { get; set; }
}
