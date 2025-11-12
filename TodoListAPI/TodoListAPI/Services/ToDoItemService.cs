using TodoListAPI.Models;
using TodoListAPI.Repository;

namespace TodoListAPI.Services
{
    public class ToDoItemService : IToDoItemService
    {
        private readonly IToDoItemRepository _repo;
        public ToDoItemService(IToDoItemRepository repo)
        {
            _repo = repo;
        }

        public ToDoItem Add(ToDoItem item)
        {
            if (string.IsNullOrWhiteSpace(item.title))
                throw new ArgumentException("Title is empty!");
            _repo.Add(item);
            return item;
        }

        public bool Delete(int id)
        {
            var item = _repo.GetById(id);
            if (item == null)
            {
                return false;
            }
            _repo.Delete(item);
            return true;
        }

        public IEnumerable<ToDoItem> GetAll()
        {
            return _repo.GetAll();
        }

        public ToDoItem? GetById(int id)
        {
            return _repo.GetById(id);
        }

        public bool Update(int id, ToDoItem item)
        {
            var existingItem = _repo.GetById(id);
            if (existingItem == null)
            {
                return false;
            }
            existingItem.title = item.title;
            existingItem.isDone = item.isDone;
            _repo.Update(existingItem);
            return true;
        }
    }
}
