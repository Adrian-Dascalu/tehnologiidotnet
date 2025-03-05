using tehnologiinet.Interfaces;

namespace tehnologiinet;

public class MyFirstService: IMyFirstServiceInterface
{
    public MyFirstService()
    {
        Console.WriteLine("Salut din MyFirstService Constructor!");
    }
    
    public void Hello()
    {
        Console.WriteLine("salut din Scoped!");
    }
}