namespace Falcon.Tools.Interfaces
{
    public interface IToolOutputFilter
    {
        ToolType ToolType { get; set; }

        string ToolName { get; set; }

        string AttributeType { get; set; }

        string Apply(string input);
    }
}