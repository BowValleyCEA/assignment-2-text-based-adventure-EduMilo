
namespace game1402_a2_starter
{
    [Serializable] //the [Serializable] attribute will be needed if you ever want to save this info
    public class Room
    {
        public string Name { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string North { get; set; }
        public string East {  get; set; }
        public string South { get; set; }
        public string West {  get; set; }
    }
}
