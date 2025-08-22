using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNetCore.Models;

[Table("geogyu")]
public partial class Geogyu
{
    /// <summary>
    /// id relacion
    /// </summary>
    [Key]
    [Column("geugid")]
    public int Geugid { get; set; }

    /// <summary>
    /// id usuario
    /// </summary>
    [Column("geugidv", TypeName = "char(10)")]
    [Required]
    [StringLength(10)]
    public string Geugidv { get; set; } = null!;

    /// <summary>
    /// id geocerca
    /// </summary>
    [Column("geugidg", TypeName = "char(10)")]
    [Required]
    [StringLength(10)]
    public string Geugidg { get; set; } = null!;

    /// <summary>
    /// usuario crea
    /// </summary>
    [Column("geuguscre", TypeName = "char(10)")]
    [Required]
    [StringLength(10)]
    public string Geuguscre { get; set; } = null!;

    /// <summary>
    /// fecha hora creacion
    /// </summary>
    [Column("geugfcre", TypeName = "datetime")]
    public DateTime Geugfcre { get; set; }

    /// <summary>
    /// equipo de creacion
    /// </summary>
    [Column("geugeqcre", TypeName = "char(50)")]
    [Required]
    [StringLength(50)]
    public string Geugeqcre { get; set; } = null!;

    /// <summary>
    /// usuario edita
    /// </summary>
    [Column("geugusedi", TypeName = "char(10)")]
    [Required]
    [StringLength(10)]
    public string Geugusedi { get; set; } = null!;

    /// <summary>
    /// fecha y hora edicion
    /// </summary>
    [Column("geugfedi", TypeName = "datetime")]
    public DateTime Geugfedi { get; set; }

    /// <summary>
    /// equipo de edicion
    /// </summary>
    [Column("geugeqedi", TypeName = "char(50)")]
    [Required]
    [StringLength(50)]
    public string Geugeqedi { get; set; } = null!;
    
    /// <summary>
    /// Latitud del usuario
    /// </summary>
    [Column("geuglat" , TypeName = "decimal(10,8)")]
    public decimal Geuglat { get; set; }
    
    /// <summary>
    /// Longitud del usuario
    /// </summary>
    [Column("geuglon" , TypeName = "decimal(11,8)")]
    public decimal Geuglon { get; set; }
    
    /// <summary>
    /// Relación con geogeoc (clave foránea)
    /// </summary>
    [ForeignKey(nameof(Geugidg))]
    public virtual Geogeoc GeugidgNavigation { get; set; } = null!;
    
}
