namespace TramitesGov.Models
{
    public class VariableInitial
    {
        public Dictionary<string, AtributeInitial> variables { get; set; }
        public void Add(string name, string value, string type)
        {
            variables.Add(name, new AtributeInitial { value = value, type = type });
        }
    }
    public class AtributeInitial
    {
        public string value { get; set; }
        public string type { get; set; }

    }
}
