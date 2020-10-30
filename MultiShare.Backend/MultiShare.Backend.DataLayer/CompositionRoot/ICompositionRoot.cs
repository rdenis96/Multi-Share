namespace MultiShare.Backend.DataLayer.CompositionRoot
{
    public interface ICompositionRoot
    {
        T GetImplementation<T>();

        T GetImplementation<T>(string name);
    }
}