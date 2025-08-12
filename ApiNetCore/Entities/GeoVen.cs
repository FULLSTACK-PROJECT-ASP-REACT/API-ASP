using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiNetCore.Entities;

[Table("tbl_geocerca_vendedor")]
public class GeoVen
{
    [Key]
    [Column("id_g_v")]
    public int IdGeoVen { get; set; }

    [Column("id_vendedor")]
    public int? IdVendedor { get; set; }

    [Column("id_geocerca")]
    public int? IdGeocerca { get; set; }

    [Column("creado_en", TypeName = "timestamp")]
    public DateTime? CreadoEn { get; set; }

    [Column("actualizado_en", TypeName = "timestamp")]
    public DateTime? ActualizadoEn { get; set; }

    public virtual Geocerca? IdGeocercaNavigation { get; set; }

    public virtual Vendedor? IdVendedorNavigation { get; set; }
}