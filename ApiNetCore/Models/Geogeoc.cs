using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNetCore.Models;

[Table("geogeoc")]
public partial class Geogeoc
{
    /// <summary>
    /// codigo geocerca
    /// </summary>
    [Key]
    [Column("geoccod", TypeName = "char(10)")]
    [Required]
    [StringLength(10)]
    public string Geoccod { get; set; } = null!;

    /// <summary>
    /// nombre de geocerca
    /// </summary>
    [Column("geocnom", TypeName = "char(200)")]
    [Required]
    [StringLength(200)]
    public string Geocnom { get; set; } = null!;

    /// <summary>
    /// sector 
    /// </summary>
    [Column("geocsec", TypeName = "char(50)")]
    [Required]
    [StringLength(50)]
    public string Geocsec { get; set; } = null!;

    /// <summary>
    /// direccion referencia
    /// </summary>
    [Column("geocdirre", TypeName = "char(100)")]
    [Required]
    [StringLength(100)]
    public string Geocdirre { get; set; } = null!;

    /// <summary>
    /// ciudad 
    /// </summary>
    [Column("geocciud", TypeName = "char(30)")]
    [Required]
    [StringLength(30)]
    public string Geocciud { get; set; } = null!;

    /// <summary>
    /// provincia
    /// </summary>
    [Column("geocprov", TypeName = "char(30)")]
    [Required]
    [StringLength(30)]
    public string Geocprov { get; set; } = null!;

    /// <summary>
    /// pais 
    /// </summary>
    [Column("geocpais", TypeName = "char(30)")]
    [Required]
    [StringLength(30)]
    public string Geocpais { get; set; } = null!;

    /// <summary>
    /// latitud referencia 
    /// </summary>
    [Column("geoclat", TypeName = "decimal(18,8)")]
    public decimal Geoclat { get; set; }

    /// <summary>
    /// longitud referencia 
    /// </summary>
    [Column("geoclon", TypeName = "decimal(11,8)")]
    public decimal Geoclon { get; set; }

    /// <summary>
    /// coordenadas 
    /// </summary>
    [Column("geoccoor", TypeName = "json")]
    public string? Geoccoor { get; set; }

    /// <summary>
    /// area en metros
    /// </summary>
    [Column("geocarm", TypeName = "decimal(15,2)")]
    public decimal Geocarm { get; set; }

    /// <summary>
    /// perimetro
    /// </summary>
    [Column("geocperm", TypeName = "decimal(10,2)")]
    public decimal Geocperm { get; set; }

    /// <summary>
    /// estado de geocerca
    /// </summary>
    [Column("geocest", TypeName = "char(1)")]
    [Required]
    [StringLength(1)]
    public string Geocest { get; set; } = null!;

    /// <summary>
    /// estado activo=1 inactivo=0
    /// </summary>
    [Column("geocact", TypeName = "tinyint(1)")]
    public bool? Geocact { get; set; }

    /// <summary>
    /// prioridad
    /// </summary>
    [Column("geocpri")]
    public int Geocpri { get; set; }

    /// <summary>
    /// descripcion
    /// </summary>
    [Column("geocdesc", TypeName = "varchar(250)")]
    [Required]
    [StringLength(250)]
    public string Geocdesc { get; set; } = null!;

    /// <summary>
    /// usuario crea
    /// </summary>
    [Column("geocuscre", TypeName = "char(10)")]
    [Required]
    [StringLength(10)]
    public string Geocuscre { get; set; } = null!;

    /// <summary>
    /// fecha hora creacion
    /// </summary>
    [Column("geocfcre", TypeName = "datetime")]
    public DateTime Geocfcre { get; set; }

    /// <summary>
    /// equipo crea
    /// </summary>
    [Column("geoceqcre", TypeName = "char(50)")]
    [Required]
    [StringLength(50)]
    public string Geoceqcre { get; set; } = null!;

    /// <summary>
    /// usuario edita
    /// </summary>
    [Column("geocusedi", TypeName = "char(10)")]
    [Required]
    [StringLength(10)]
    public string Geocusedi { get; set; } = null!;

    /// <summary>
    /// fecha y hora edicion
    /// </summary>
    [Column("geocfedi", TypeName = "datetime")]
    public DateTime Geocfedi { get; set; }


    
    
    /// <summary>
    /// 
    /// </summary>
    [Column("geoceqedi", TypeName = "char(50)")]
    [Required]
    [StringLength(50)]
    public string Geoceqedi { get; set; } = null!;
    

    /// <summary>
    /// Relación con geogyu
    /// </summary>
    public virtual ICollection<Geogyu> Geogyus { get; set; } = new List<Geogyu>();
}