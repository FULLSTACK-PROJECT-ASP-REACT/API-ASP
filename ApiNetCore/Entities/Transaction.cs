using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Entities;

[Table("tbl_transaction")]
public partial class Transaction
{
    [Key]
    [Column("id_tra")]
    public int IdTra { get; set; }

    [Column("emission_date_tra", TypeName = "datetime")]
    public DateTime? EmissionDateTra { get; set; }
    
    [Column("code_stub_tra", TypeName = "varchar(50)")]
    public string? CodeStub { get; set; }

    [Column("type_tra")]
    [StringLength(1)]
    [Unicode(false)]
    public string TypeTra { get; set; } = null!;

    [Column("price_unit_tra")]
    public int PriceUnitTra { get; set; }

    [Column("total_amount_tra")]
    public int TotalAmountTra { get; set; }

    [Column("status_tra")]
    [StringLength(1)]
    [Unicode(false)]
    public string? StatusTra { get; set; }

    [Column("message_tra", TypeName = "text")]
    public string? MessageTra { get; set; }

    [InverseProperty("Tra")]
    public virtual ICollection<DetailTransaction> TblDetailTransactions { get; set; } = new List<DetailTransaction>();
}
