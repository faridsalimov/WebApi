using System.Linq.Expressions;
using WebApiTask1.Entities;
using WebApiTask1.Repositories.Abstract;
using WebApiTask1.Services.Abstract;

namespace WebApiTask1.Services.Concrete
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public void Add(Student entity)
        {
            _studentRepository.Add(entity); 
        }

        public void Delete(int id)
        {
           var item = _studentRepository.Get(s => s.Id == id);
            _studentRepository.Delete(item);    
        }

        public Student Get(Expression<Func<Student, bool>> expression)
        {
            return _studentRepository.Get(expression);  
        }

        public IEnumerable<Student> GetAll()
        {
            return _studentRepository.GetAll();
        }

        public void Update(Student entity)
        {
            _studentRepository.Update(entity);  
        }
    }
}
