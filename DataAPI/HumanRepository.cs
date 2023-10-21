using DataAPI.DataBase.Tables;

namespace DataAPI.DataBase.Repositories
{
    public class HumanRepository
    {
        private readonly Context _context;

        public HumanRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<Human> GetAll()
        {
            return _context.Humans.ToList();
        }

        public Human GetById(int id)
        {
            return _context.Humans.Find(id);
        }

        public void Insert(Human human)
        {
            _context.Humans.Add(human);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var human = _context.Humans.Find(id);
            if (human == null) return;
            _context.Humans.Remove(human);
            _context.SaveChanges();
        }
    }
}