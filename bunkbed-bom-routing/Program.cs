using System.Text.Json;
using System.Text;

string root = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;

string bomPath = Path.Combine(root, "bunkbed-bom.json");
string routingPath = Path.Combine(root, "bunkbed-routing.json");
string outputPath = Path.Combine(root, "output.csv");

var bomJson = File.ReadAllText(bomPath);
var routingJson = File.ReadAllText(routingPath);

var bom = JsonSerializer.Deserialize<Bom>(bomJson);
var routing = JsonSerializer.Deserialize<List<Routing>>(routingJson);

var providedItems = new Dictionary<string, int>();
var providedItemSteps = new List<int?>();

int totalTakt = 0;

searchBom(bom);

var sb = new StringBuilder();
sb.AppendLine("component,quantity");

foreach (var item in providedItems)
    sb.AppendLine($"{item.Key},{item.Value}");

File.WriteAllText(outputPath, sb.ToString());

foreach (var step in routing)
{ 

    bool hasProvided = providedItemSteps.Contains(step.step);

    if (!hasProvided)
    {
        Console.WriteLine($"Step {step.step} '{step.description}' has no provided components added.");
    }

    totalTakt += step.taktTime;
}

Console.WriteLine($"Overall takt time: {totalTakt} minutes.");

void searchBom(Bom item)
{
    if (item.source == "Provided")
    {
        if (!providedItems.ContainsKey(item.description))
        {
            providedItems[item.description] = 0;
        }
        providedItemSteps.Add(item.step);
        providedItems[item.description] += item.quantity;
    }

    if (item.bom != null)
    {
        foreach (var subItem in item.bom)
            searchBom(subItem);
    }
}


