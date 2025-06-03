namespace ApiNetCore.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string entity, object id) 
        : base(404, "ENTITY_NOT_FOUND", $"{entity} with ID {id} was not found")
    {
    }

    public NotFoundException(string message) 
        : base(404, "NOT_FOUND", message)
    {
    }
}