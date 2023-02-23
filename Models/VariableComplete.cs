namespace TramiteGov.Models
{
    public class VariableComplete
    {
        public Dictionary<string, AtributeComplete> variables { get; set; }
        public void Add(string name, string value, string type)
        {
            variables.Add(name, new AtributeComplete { value = value, });
        }
    }
    public class AtributeComplete
    {
        public string value { get; set; }

    }
}
