# EfLight 🪶

## Goal

**EfLight**'s goal is to provide a simple API to use EF Core in order to avoid the boilerplate code required for most basic operations with EF Core.

It provides two base classes:

- CrudRepository : which as its name implies, provides most basic operations for CRUD.

- PagingAndSortingRepository: it extends **CrudRepository** and provides **pagination** features on top of existing features of CrudRepository.

## Example

Suppose that we have an entity class named _Student_ and our DbContext is already configured.

```cs

[Table("Student")]
class Student
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }
}

```

To interact with the table, just create a _repository_ class which extends whether **CrudRepository** or **PagingAndSortingRepository** (the choice depends on your requirements) with its corresponding interface that will be injected in other classes.

```cs

public interface IStudentRepositor : ICrudRepository<Student, int>
{

}


class StudentRepository : CrudRepository<Student, int>, IStudentRepository
{
    public StudentRepository(YourDbContext context) : base(context)
    {

    }
}

```

Then, you can register your repository class in the **IoC** container in order to use it from anywhere.

```cs

builder.Services.AddScoped<IStudentRepository, StudentRepository>();

```

All you have to do is to inject it where it is needed. For example:

```cs

public class StudentService
{
   private readonly IStudentRepository _repository;

   public StudentService(IStudentRepository repository)
   {
        _repository = repository;
   }

   public void CheckIfAllAboveAge(int age)
   {
       bool result =  _repository.ExistsWhere(x => x.BirthData.Year > age);

       // Your business logic
   }

   public Student? FindByName(string name)
   {
        Student? student;

        Optional<Student> result = _repository.FindWhere(x => x.LastName == name);

        // Throws the exception if the result is null
        student = result.OrElseThrow(() => new Exception());

        // Instead of throwing an exception, you can also do
        student  = result.IfNullThen(() => {
            student = _repository.FindWhere(x => x.FirstName == name);
        });

        return student;
   }
}

```

## Adding your features

You can also provide more features to your repository classes depending on your use cases. Just use an interface defining the contract and provide your own implementation of that use case.

```cs

public interface IStudentRepositor : ICrudRepository<Student, int>
{
    void MakeLastNameNull(int studentId);
}

```

In the repository class, you'll have:

```cs

class StudentRepository : CrudRepository<Student, int>, IStudentRepository
{
    public StudentRepository(YourDbContext context) : base(context)
    {

    }

    public void MakeLastNameNull(int studentId)
    {
        // Your implementation
    }
}

```

## PS

Want to contribute ? Keep me in touch ;-)
