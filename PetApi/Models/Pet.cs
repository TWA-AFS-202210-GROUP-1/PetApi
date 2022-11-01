using Newtonsoft.Json;

namespace PetApi.Models
{
    public class Pet
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public PetType Type { get; set; }
        
        [JsonProperty("color")]
        public string Color { get; set; }
        
        [JsonProperty("price")]
        public double Price { get; set; }

        public Pet(string name, PetType type, string color, double price)
        {
            Name = name;
            Type = type;
            Color = color;
            Price = price;
        }
    }
}
