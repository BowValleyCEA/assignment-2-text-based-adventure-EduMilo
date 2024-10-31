
using System.ComponentModel;

namespace game1402_a2_starter
{
    [Serializable] //the [Serializable] attribute will be needed if you ever want to save this info
    public class Room
    {
        public string Reference { get; set; }
        public string[] Description { get; set; } // list of descriptions assigned to the state.
        public int State { get; set; } //state in the room, used for rooms that change over time.

        public string[] North { get; set; }
        public string[] East {  get; set; }
        public string[] South { get; set; }
        public string[] West {  get; set; }

        public List<StaticItem> StaticItems { get; set; }
        public List<GrabbableItem> GrabbableItems { get; set; }
        
        public string GetDescription()
        {
            return Description[State];
        }

    }
}
