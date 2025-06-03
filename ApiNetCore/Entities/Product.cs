using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Entities;

[Table("tbl_product")]
public partial class Product
{
    [Key]
    [Column("id_pro")]
    public int IdPro { get; set; }

    [Column("code_pro")]
    [StringLength(100)]
    [Unicode(false)]
    public string CodePro { get; set; } = null!;

    [Column("name_pro")]
    [Unicode(false)]
    public string NamePro { get; set; } = null!;

    [Column("description_pro", TypeName = "text")]
    public string? DescriptionPro { get; set; }

    [Column("price_unit_pro", TypeName = "decimal(10, 2)")]
    public decimal PriceUnitPro { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("update_at", TypeName = "datetime")]
    public DateTime? UpdateAt { get; set; }

    [Column("status_pro")]
    [StringLength(1)]
    [Unicode(false)]
    public string? StatusPro { get; set; }

    [Column("stock_pro")]
    public int StockPro { get; set; }

    [Column("image_pro")]
    [Unicode(false)]
    public string? ImagePro { get; set; }

    [InverseProperty("Pro")]
    public virtual ICollection<CategoryProduct> TblCategoryProducts { get; set; } = new List<CategoryProduct>();

    [InverseProperty("Pro")]
    public virtual ICollection<DetailTransaction> TblDetailTransactions { get; set; } = new List<DetailTransaction>();
}
