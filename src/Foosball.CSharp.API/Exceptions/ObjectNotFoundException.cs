namespace Foosball.CSharp.API.Exceptions;

public class ObjectNotFoundException : Exception
{
    public ObjectNotFoundException(string objectName, params object[] objectIds)
        : base($"Could not find {objectName} with provided ID(s): {string.Join(", ", objectIds)}")
    {

    }
}
