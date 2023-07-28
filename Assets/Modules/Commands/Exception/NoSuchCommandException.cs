namespace Commands.Project.Scripts.Modules.Commands.Exception
{
    public class NoSuchCommandException : System.Exception
    {
        public override string Message { get; } = "No such command";
    }
}