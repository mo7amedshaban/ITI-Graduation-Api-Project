namespace Core.Interfaces;

public interface IUnitOfWork:IDisposable
{
    //IAuthorRepository Authors { get; } //property for Author repository
    //IBookRepository Books { get; }  //property for Book repository
    int Complete();
}


