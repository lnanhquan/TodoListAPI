using TodoListAPI.Data;
using TodoListAPI.Models;

namespace TodoListAPI.Repository
{
    public class ToDoItemRepository : IToDoItemRepository
    {
        private readonly ApplicationDbContext _db;

        public ToDoItemRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Add(ToDoItem item)
        {
            _db.ToDoItems.Add(item);
            _db.SaveChanges();
        }

        public void Delete(ToDoItem item)
        {
            _db.ToDoItems.Remove(item);
            _db.SaveChanges();
        }

        public IEnumerable<ToDoItem> GetAll()
        {
            return _db.ToDoItems.ToList();
        }

        public ToDoItem? GetById(int id)
        {
            var item = _db.ToDoItems.Find(id);
            return item;
        }

        public void Update(ToDoItem item)
        {
            _db.ToDoItems.Update(item);
            _db.SaveChanges();
        }
    }
}
