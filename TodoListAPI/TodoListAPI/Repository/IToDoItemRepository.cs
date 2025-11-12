using TodoListAPI.Models;

namespace TodoListAPI.Repository
{
    public interface IToDoItemRepository
    {
        IEnumerable<ToDoItem> GetAll();
        ToDoItem? GetById(int id);
        void Add(ToDoItem item);
        void Update(ToDoItem item);
        void Delete(ToDoItem item);
    }
}
