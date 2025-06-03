using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Entities;

[Table("tbl_category")]
public partial class Category
{
    [Key]
    [Column("id_cat")]
    public int IdCat { get; set; }

    [Column("name_cat")]
    [StringLength(100)]
    [Unicode(false)]
    public string NameCat { get; set; } = null!;

    [Column("status_cat")]
    [StringLength(1)]
    [Unicode(false)]
    public string? StatusCat { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Cat")]
    public virtual ICollection<CategoryProduct> TblCategoryProducts { get; set; } = new List<CategoryProduct>();
}
