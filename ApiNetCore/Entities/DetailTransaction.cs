using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Entities;

[Table("tbl_detail_transaction")]
public partial class DetailTransaction
{
    [Key]
    [Column("id_d_t")]
    public int IdDT { get; set; }

    [Column("pro_id")]
    public int? ProId { get; set; }

    [Column("tra_id")]
    public int? TraId { get; set; }

    [Column("code_stub")]
    [StringLength(250)]
    [Unicode(false)]
    public string? CodeStub { get; set; }

    [Column("price_unit", TypeName = "decimal(10, 2)")]
    public decimal PriceUnit { get; set; }

    [Column("subtotal", TypeName = "decimal(18, 2)")]
    public decimal Subtotal { get; set; }

    [Column("total", TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; }

    [Column("amount")]
    public int Amount { get; set; }

    [Column("description_tra")]
    [StringLength(250)]
    [Unicode(false)]
    public string? DescriptionTra { get; set; }

    [ForeignKey("ProId")]
    [InverseProperty("TblDetailTransactions")]
    public virtual Product? Pro { get; set; }

    [ForeignKey("TraId")]
    [InverseProperty("TblDetailTransactions")]
    public virtual Transaction? Tra { get; set; }
}
