using System.Collections.Concurrent;

namespace ToDoList.Lib
{
    public class UserStatistics
    {
        public ConcurrentQueue<string> Statistics { get; set; } = new();
    }
}
