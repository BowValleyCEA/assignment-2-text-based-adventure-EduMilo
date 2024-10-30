
using System.Text;

namespace game1402_a2_starter
{
    public class GameData
    {
        public string GameName { get; set; } //This is an example of a property; for whatever reason your serializable data objects will need to be written this way
        public string Description { get; set; }
        public List<Room> Rooms { get; set; } //this is only an example.
        public string Help {  get; set; } 
        public string Invalid {  get; set; }
    }

    public class Game(GameData data)
    {
        private GameData _gameData = data;
        private Room _currRoom; //the room the player is standing in.
        private List<GrabbableItem> _inventory = new List<GrabbableItem>(); //the inventory of the player.

        //Gameplay Functions
        public void Init()
        {
            _currRoom = _gameData.Rooms[0]; //places you in the first room.
            Console.WriteLine(_currRoom.GetDescription()); //describes the room you are standing in for the first time.
        }
        public void ProcessString(string enteredString)
        {
            enteredString = enteredString.Trim().ToLower(); //trim any white space from the beginning or end of string and convert to lower case

            string[] commands = enteredString.Split(" "); //split based on spaces. The length of this array will tell you whether you have 1, 2, 3, 4, or more commands.
                                                          //modify these functions however you want, but this is where the beginning of calling functions to handle where you are
            string response = CommandParse(commands);
            
            Console.WriteLine(response); //what you tell the person after what they entered has been processed
        }
        private string CommandParse(string[] input)
        {
            string response = string.Empty;
            switch (input[0])
            {
                case "help":
                    response = HelpCommand(input);
                    break;
                case "clear":
                    response = ClearCommand(input);
                    break;
                case "inspect":
                    response = InspectCommand(input);
                    break;
                case "use":
                    response = UseCommand(input);
                    break;
                case "go":
                    response = GoCommand(input);
                    break;
                case "grab":
                    response = GrabCommand(input);
                    break;
                case "inventory":
                    response = InventoryCommand(input);
                    break;
                default:
                    response = _gameData.Invalid;
                    break;
            }
            return response;
        }
        
        //Command Functions
        private string HelpCommand(string[] input)
        {
            if(!IsValidLength(1, input))
            {
                return "help is only typed by itself! Example: 'help'";
            }
            return _gameData.Help;

        }
        private string ClearCommand(string[] input)
        {
            if (!IsValidLength(1, input))
            {
                return "clear is only typed by itself! Example: 'clear'";
            }

            Console.Clear();
            return _currRoom.GetDescription();
        }
        private string GoCommand(string[] input)
        {
            if (!IsValidLength(2, input) || input.Length == 1)
            {
                return "'go' is used with one argument. Examples: 'go north', 'go west', 'go n'";
            }
            string reference = string.Empty;
            switch (input[1])
            {
                case "north":
                case "n":
                        reference = _currRoom.North;
                    break;

                case "east":
                case "e":
                        reference = _currRoom.East;
                    break;

                case "south":
                case "s":
                        reference = _currRoom.South;
                    break;

                case "west":
                case "w":
                        reference = _currRoom.West;
                    break;
                default:
                    return "'go' must be followed by a cardinal direction (north, east, south, west).";
            }

            //check if ref is invalid. (No room linked there)
            bool refIsInvalid = reference.Equals("");
            if (refIsInvalid)
            {
                return "You can't go " + input[1];
            }
            
            //check if room actually exists
            var foundRoom = _gameData.Rooms.Find(x => x.Reference == reference);
            if(foundRoom == null)
            {
                return "DEBUG: attempted to traverse to nonexistent room. (incorrect reference name?)";
            } 

            //change room, clear console, then return room description
            _currRoom = foundRoom;
            Console.Clear();
            return "You went " + input[1]+ ".\n" + _currRoom.GetDescription();
        }
        private string InventoryCommand(string[] input)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Items in your inventory:\n");
            foreach (Item i in _inventory)
            {
                sb.Append(i.Name);
                sb.Append("\n");
            }

            return sb.ToString();
        }
        private string InspectCommand(string[] input)
        {
            //check validity of input
            if(!IsValidLength(2, input))
            {
                return "'inspect' is used with at most one argument. Examples: 'inspect', 'inspect flashlight'";
            }
            //if input was just 'inspect', print out list of items in room + items in inventory
            if (input.Length == 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Items you can inspect in this room:\n");
                foreach (Item i in _currRoom.StaticItems)
                {
                    sb.Append(i.Name);
                    sb.Append("\n");
                }
                foreach(Item i in _currRoom.GrabbableItems)
                {
                    sb.Append(i.Name);
                    sb.Append("\n");
                }

                sb.Append("Items in your inventory:\n");
                foreach(Item i in _inventory)
                {
                    sb.Append(i.Name);
                    sb.Append("\n");
                }
                return sb.ToString();
            }
            //now that we know we have two words, search for the second word in items and return it's description.
            //check for duplicate items.
            if (HasDuplicates(input[1]))
            {
                return "DEBUG: " + input[1] + " has duplicates of different types!";
            }

            //now check if item exists. if it does, return it's description.
            var toBeInspected = FindItem(input[1]);
            if (toBeInspected == null)
            {
                return "There is no " + input[1] + " in this room!";
            }
                return toBeInspected.Describe();
        }
        private string UseCommand(string[] input)
        {
            //use has either two or four arguments. otherwise, its invalid
            bool invalidArgumentAmount = (input.Length != 2 && input.Length != 4);
            //if it has four arguments, it must be the format "use X on Y"
            bool invalidArgumentFormat = (input.Length == 4 && input[2] != "on");

            if (invalidArgumentAmount || invalidArgumentFormat)
            {
                return "'use' is used in the following ways: 'use key', 'use key on door'";
            }

            //check duplicates
            if (HasDuplicates(input[1]))
            {
                return "DEBUG: " + input[1] + " has duplicates of different types!";
            }

            //now check if item exists. if it does, return it's use message.
            var toBeUsed = FindItem(input[1]);
            if (toBeUsed == null)
            {
                return "There is no " + input[1] + " in this room!";
            }
            return toBeUsed.Use(_gameData, _currRoom, input);
        }
        private string GrabCommand(string[] input)
        {
            //check validity of use
            if (!IsValidLength(2, input))
            {
                return "'grab' is used with at most one argument. Examples: 'grab', 'grab flashlight'";
            }

            //check for duplicates
            if (HasDuplicates(input[1]))
            {
                return "DEBUG: " + input[1] + " has duplicates of different types!";
            }

            //find the object in the room, excluding inventory items.
            var toBeGrabbed = FindRoomItem(input[1]);

            //check for null
            if(toBeGrabbed == null)
            {
                return "There is no " + input[1] + " in this room!";
            }

            //if it's a static item, tell the player they cannot grab it.
            if (toBeGrabbed is StaticItem)
            {
                return "You cant grab the " + input[1] + "! It's too big!";
            }

            //now that we know it must be a GrabbableItem, cast it, remove it from the grabbableList, and add it to your inventory.
            _currRoom.GrabbableItems.Remove((GrabbableItem)toBeGrabbed);
            _inventory.Add((GrabbableItem)toBeGrabbed);
            return "Added " + input[1] + " to your inventory!";
        }

        //Functions used to help the commands:
        private bool IsValidLength(int maxLength, string[] input)
        {
            //if there are more arguments than the command needs, 
            return (input.Length <= maxLength);
        }
        private Item? FindItem(string itemRef)
        {
            //returns the item. if not found, returns null.
            return GetAllItems().Find(x => x.Reference == itemRef);
        }
        private Item? FindRoomItem(string itemRef)
        {
            //returns item as long as it is available and NOT in the inventory. If not found, return null.
            return GetRoomItems().Find(x => x.Reference == itemRef);
        }
        private bool HasDuplicates(string itemRef)
        {
            //search StaticItems, GrabbableItems, and Inventory for the object.
            var staticItem = _currRoom.StaticItems.Find(x => x.Reference == itemRef);
            var grabbableItem = _currRoom.GrabbableItems.Find(x => x.Reference == itemRef);
            var inventoryItem = _inventory.Find(x => x.Reference == itemRef);

            //this is a very unfortunate way of checking for duplicates. THANKS JSON!
            bool AreThereDuplicates = (staticItem != null && grabbableItem != null);
            AreThereDuplicates = AreThereDuplicates || (grabbableItem != null && inventoryItem != null);
            AreThereDuplicates = AreThereDuplicates || (inventoryItem != null && staticItem != null);

            return AreThereDuplicates;

        }
        private List<Item> GetAllItems()
        {
            var list = new List<Item>();
            list.AddRange(_currRoom.StaticItems);
            list.AddRange(_currRoom.GrabbableItems);
            list.AddRange(_inventory);
            return list;
        }
        private List<Item> GetRoomItems()
        {
            //Returns a list of all items OUTSIDE of the inventory.
            var list = new List<Item>();
            list.AddRange(_currRoom.StaticItems);
            list.AddRange(_currRoom.GrabbableItems);
            return list;
        }
    }
}
