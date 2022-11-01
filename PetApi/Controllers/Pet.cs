namespace PetApi.Controllers
{
    public class Pet
    {
        private string name;
        private string type;
        private string color;
        private string price;

        public Pet(string name, string type, string color, string price)
        {
            this.name = name;
            this.type = type;
            this.color = color;
            this.price = price;
        }

        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public string Color { get => color; set => color = value; }
        public string Price { get => price; set => price = value; }

        public override bool Equals(object? obj)
        {
            var pet = obj as Pet;
            return this.name.Equals(pet.Name) && this.type.Equals(pet.Type) && this.color.Equals(pet.Color) && this.price.Equals(pet.Price);
        }
    }
}