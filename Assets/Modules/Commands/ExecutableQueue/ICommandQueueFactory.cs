namespace Commands.Project.Scripts.Modules.Commands.ExecutableQueue
{
    public interface ICommandQueueFactory
    {
        ICommandQueue Create();
    }
}