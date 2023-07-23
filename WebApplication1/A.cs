using EfLight.Abstractions;
using EfLight.Attributes;
using EfLight.Core;

using Microsoft.EntityFrameworkCore;

namespace WebApplication1
{

    public interface ITestRepository: ICrudRepository<object, int>
    {
    }

    public class ATestClass : CrudRepository<object, int>,  ITestRepository
    {
        public ATestClass(MyContext context) :base(context) { }
    }
}
