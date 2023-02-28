namespace TramiteGov.Models
{
    public class VariableModification
    {
        public Dictionary<string, AtributeModification> modifications { get; set; }
        public void Add(string name, string value)
        {
            modifications.Add(name, new AtributeModification { value = value});
        }
    }
    public class AtributeModification
    {
        public string value { get; set; }
    }
}
