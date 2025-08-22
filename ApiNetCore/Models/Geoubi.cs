using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNetCore.Models;

/// <summary>
/// Representa la ubicación geográfica registrada por un usuario.
/// </summary>
[Table("Geoubi")]
public partial class Geoubi
{
    /// <summary>
    /// ID del registro.
    /// </summary>
    [Key]
    [Column("geubid")]
    public int Geubid { get; set; }

    /// <summary>
    /// Usuario que crea el registro.
    /// </summary>
    [Required]
    [Column("geubusu")]
    [StringLength(100)]
    public string Geubusu { get; set; } = null!;

    /// <summary>
    /// Fecha del registro.
    /// </summary>
    [Required]
    [Column("geubfech", TypeName = "datetime")]
    public DateTime Geubfech { get; set; }

    /// <summary>
    /// Latitud de la ubicación.
    /// </summary>
    [Required]
    [Column("geublat", TypeName = "decimal(10, 8)")]
    public decimal Geublat { get; set; }

    /// <summary>
    /// Longitud de la ubicación.
    /// </summary>
    [Required]
    [Column("geublon", TypeName = "decimal(11, 8)")]
    public decimal Geublon { get; set; }
}
