using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Entities;

[Table("tbl_category_product")]
public partial class CategoryProduct
{
    [Key]
    [Column("id_ca_pr")]
    public int IdCaPr { get; set; }

    [Column("pro_id")]
    public int? ProId { get; set; }

    [Column("cat_id")]
    public int? CatId { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CatId")]
    [InverseProperty("TblCategoryProducts")]
    public virtual Category? Cat { get; set; }

    [ForeignKey("ProId")]
    [InverseProperty("TblCategoryProducts")]
    public virtual Product? Pro { get; set; }
}
