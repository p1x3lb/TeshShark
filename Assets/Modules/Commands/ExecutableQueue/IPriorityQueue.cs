namespace Commands.Project.Scripts.Modules.Commands.ExecutableQueue
{
    public interface IPriorityQueue<TPriority, TValue>
    {
        bool IsEmpty { get; }
        int Count { get; }
        void Enqueue(TPriority priority, TValue value);
        TValue Dequeue();
        TValue Peek();
        void Clear();
    }
}