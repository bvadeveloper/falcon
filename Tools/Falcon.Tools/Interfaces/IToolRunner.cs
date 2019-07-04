namespace Falcon.Tools.Interfaces
{
    public interface IToolRunner
    {
        IToolRunner Run(string command);

        string GetOutput();
    }
}