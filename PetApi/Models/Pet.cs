namespace PetApi.Models
{
    public class Pet
    {
        public string Name { get; set; }
        public PetType Type { get; set; }
        public string Color { get; set; }
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
