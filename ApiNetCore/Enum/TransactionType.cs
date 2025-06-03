namespace ApiNetCore.Enum;

public enum TransactionType
{
    Purchase = 'C', // Compra
    Sale = 'V'      // Venta
}

public enum TransactionStatus
{
    Active = 'A',     // Activo
    Inactive = 'I',   // Inactivo
    Cancelled = 'C'   // Cancelado
}

public static class TransactionExtensions
{
    public static string GetDescription(this TransactionType type)
    {
        return type switch
        {
            TransactionType.Purchase => "Compra",
            TransactionType.Sale => "Venta",
            _ => "Desconocido"
        };
    }

    public static string GetDescription(this TransactionStatus status)
    {
        return status switch
        {
            TransactionStatus.Active => "Activo",
            TransactionStatus.Inactive => "Inactivo",
            TransactionStatus.Cancelled => "Cancelado",
            _ => "Desconocido"
        };
    }

    public static bool IsValidTransactionType(string type)
    {
        return type == "C" || type == "V";
    }

    public static bool IsValidTransactionStatus(string status)
    {
        return status == "A" || status == "I" || status == "C";
    }
}