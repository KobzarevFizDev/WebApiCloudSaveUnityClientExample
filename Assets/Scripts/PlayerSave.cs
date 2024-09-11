using System;
using Newtonsoft.Json;

[Serializable]
public class PlayerSave
{
    public string Login { get; set; }
    [JsonProperty(PropertyName = "nickname")]
    public string Nickname { get; set; }
    [JsonProperty(PropertyName = "money")]
    public int Money { get; set; }
    [JsonProperty(PropertyName = "level")]
    public int Level { get; set; }
}
