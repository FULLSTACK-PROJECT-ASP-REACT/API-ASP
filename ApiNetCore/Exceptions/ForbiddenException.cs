namespace ApiNetCore.Exceptions;

public class ForbiddenException(string message) : Exception(message);