using Newtonsoft.Json;
using Serilog;

namespace WatchCat.Core.Models;

public class Data
{

    [JsonProperty("eqmt")]
    public dynamic Equipment { set
        {
            ProcessEquipment(value);
        }
    }

    private void ProcessEquipment(dynamic value)
    {
        foreach (dynamic part in value)
        {
            foreach (dynamic data in part.Value.parts)
            {
                if (data.Value.ducats == null) continue;

                Parts.Add(ConvertToPart(data));

            }
        }
    }

    private Part ConvertToPart(dynamic data)
    {
        Part part = new Part();
        part.Name = data.Name;
        part.Ducats = data.Value.ducats;
        part.Vaulted = data.Value.vaulted;
        foreach (var relicType in RawRelicData)
        {
            foreach (var relic in relicType.Value)
            {
                foreach (var kvp in relic.Value)
                {
                    if(part.Name == kvp.Value)
                    {
                        part.Relic = $"{relicType.Key} {relic.Key}";
                    }
                }
            }
        }
        return part;
    }

    [JsonProperty("relics")]
    public Dictionary<string, Dictionary<string, Dictionary<string, string>>> RawRelicData { get; set; }

    public List<Part> Parts { get; set; } = new List<Part>();
}

public class Part
{
    public string Name { get; set; }
    public int Ducats { get; set; }
    public bool Vaulted { get; set; }
    public double Average { get; set; }
    public int Volume { get; set; }
    public string Relic { get; set; }
}