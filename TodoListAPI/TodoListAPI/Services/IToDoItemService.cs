using TodoListAPI.Models;

namespace TodoListAPI.Services
{
    public interface IToDoItemService
    {
        IEnumerable<ToDoItem> GetAll();
        ToDoItem? GetById(int id);
        ToDoItem Add(ToDoItem item);
        bool Update(int id, ToDoItem item);
        bool Delete(int id);
    }
}
