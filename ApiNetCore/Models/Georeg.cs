using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNetCore.Models;

[Table("georeg")]
public partial class Georeg
{
    /// <summary>
    /// id registro
    /// </summary>
    [Key]
    [Column("regid")]
    public int Regid { get; set; }

    /// <summary>
    /// usuario registra
    /// </summary>
    [Column("regusu", TypeName = "char(10)")]
    [Required]
    [StringLength(10)]
    public string Regusu { get; set; } = null!;

    /// <summary>
    /// tipo transaccion 1=cobros 2=pedidos
    /// </summary>
    [Column("regtiptra")]
    public int Regtiptra { get; set; }

    /// <summary>
    /// numero 1
    /// </summary>
    [Column("regnum1")]
    public int Regnum1 { get; set; }

    /// <summary>
    /// numero 2
    /// </summary>
    [Column("regnum2")]
    public int Regnum2 { get; set; }

    /// <summary>
    /// serie 1
    /// </summary>
    [Column("regser1", TypeName = "char(30)")]
    [Required]
    [StringLength(30)]
    public string Regser1 { get; set; } = null!;

    /// <summary>
    /// serie 2
    /// </summary>
    [Column("regser2", TypeName = "char(30)")]
    [Required]
    [StringLength(30)]
    public string Regser2 { get; set; } = null!;

    /// <summary>
    /// serie 3
    /// </summary>
    [Column("regser3", TypeName = "char(30)")]
    [Required]
    [StringLength(30)]
    public string Regser3 { get; set; } = null!;

    /// <summary>
    /// latitu
    /// </summary>
    [Column("reglat", TypeName = "decimal(18,8)")]
    public decimal Reglat { get; set; }

    /// <summary>
    /// longitud
    /// </summary>
    [Column("reglog", TypeName = "decimal(11,8)")]
    public decimal Reglog { get; set; }

    /// <summary>
    /// fecha registro
    /// </summary>
    [Column("regfech", TypeName = "datetime")]
    public DateTime Regfech { get; set; }

    /// <summary>
    /// codigo cliente
    /// </summary>
    [Column("regcodcli", TypeName = "char(13)")]
    [Required]
    [StringLength(13)]
    public string Regcodcli { get; set; } = null!;

    /// <summary>
    /// nombre cleinte
    /// </summary>
    [Column("regnomcli", TypeName = "char(200)")]
    [Required]
    [StringLength(200)]
    public string Regnomcli { get; set; } = null!;

    /// <summary>
    /// direccion referencia
    /// </summary>
    [Column("regdirref", TypeName = "char(100)")]
    [Required]
    [StringLength(100)]
    public string Regdirref { get; set; } = null!;
}