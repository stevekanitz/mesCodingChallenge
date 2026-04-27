using System.Dynamic;

public class Bom
{   
    public string description {get; set;}
    public int quantity {get; set;}
    public string source {get; set;}
    public int? step {get; set;}
    public List<Bom> bom {get; set;}
}

public class Routing
{
    public int step {get; set;}
    public string description {get; set;}
    public int taktTime {get; set;}
}