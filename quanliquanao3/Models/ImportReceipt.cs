using System;
using System.Collections.Generic;

namespace quanliquanao3.Models;

public partial class ImportReceipt
{
    public int ReceiptId { get; set; }

    public DateTime ImportDate { get; set; }

    public int? SupplierId { get; set; }

    public string? ImportedBy { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<ImportDetail> ImportDetails { get; set; } = new List<ImportDetail>();

    public virtual Supplier? Supplier { get; set; }

    public virtual User? User { get; set; }
}
