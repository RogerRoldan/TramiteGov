namespace TramiteGov.Models
{
    public class VariableModification
    {
        public Dictionary<string, Atribute> modifications { get; set; }
        public void Add(string name, string value)
        {
            modifications.Add(name, new Atribute { value = value});
        }
    }
    public class Atribute
    {
        public string value { get; set; }
    }
}
