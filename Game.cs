﻿
using System.Text;

namespace game1402_a2_starter
{
    public class GameData
    {
        public string GameName { get; set; } //This is an example of a property; for whatever reason your serializable data objects will need to be written this way
        public string Author { get; set; }
        public string Introduction { get; set; }
        public List<Room> Rooms { get; set; } //this is only an example.
        public string Help {  get; set; } 
        public string Invalid {  get; set; }

        //these two are used to check if the "win item" has been set to the "winning state", thus ending the game.
        public string WinItemRef { get; set; }
        public int WinItemState {  get; set; }
        public string WinMessage {  get; set; }
    }

    public class Game(GameData data)
    {
        private GameData _gameData = data;
        private Room _currRoom; //the room the player is standing in.
        private List<GrabbableItem> _inventory = new List<GrabbableItem>(); //the inventory of the player.
        //public bool gameStarted = false; //has the game officially started?
        public bool gameIsWon = false; // has the game been won!

        //Gameplay Functions
        public void Init()
        {
            //first, present introduction.
            Console.WriteLine(_gameData.GameName);
            Console.WriteLine("By " + _gameData.Author + "\n");
            Console.WriteLine(_gameData.Introduction + "\n");
            Console.WriteLine("PRESS ENTER TO BEGIN!");
            Console.ReadLine();
            Console.Clear();

            //gameStarted = true;
            _currRoom = _gameData.Rooms[0]; //places you in the first room in the JSON file.
            Console.WriteLine(_currRoom.GetDescription()); //describes the room you are standing in for the first time.
        }
        public void ProcessString(string enteredString)
        {
            enteredString = enteredString.Trim().ToLower(); //trim any white space from the beginning or end of string and convert to lower case

            string[] commands = enteredString.Split(" "); //split based on spaces. The length of this array will tell you whether you have 1, 2, 3, 4, or more commands.
                                                          //modify these functions however you want, but this is where the beginning of calling functions to handle where you are
            string response = CommandParse(commands);

            //string paddingLine = "  >" + enteredString + "\n";

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
                case "take":
                    response = TakeCommand(input);
                    break;
                case "inventory":
                    response = InventoryCommand(input);
                    break;
                case "turn":
                    response = TurnCommand(input);
                    break;
                default:
                    response = _gameData.Invalid;
                    break;
            }
            //check for win.
            CheckWin();
            if (gameIsWon)
            {
                Console.Clear();
                return _gameData.WinMessage; //ya won!
            }
            return response;
        }
        private void CheckWin()
        {
            //first, check for duplicates
            if (HasDuplicates(_gameData.WinItemRef))
            {
                Console.WriteLine("JSON MISTAKE: There are multiple win items!");
            }

            //search for Win item!
            foreach (Item i in GetAllItems())
            {
                if(i.Reference == _gameData.WinItemRef && i.State == _gameData.WinItemState)
                {
                    gameIsWon = true;
                }
            }
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
                        reference = _currRoom.North[_currRoom.State];
                    break;

                case "east":
                case "e":
                        reference = _currRoom.East[_currRoom.State];
                    break;

                case "south":
                case "s":
                        reference = _currRoom.South[_currRoom.State];
                    break;

                case "west":
                case "w":
                        reference = _currRoom.West[_currRoom.State];
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
                return "JSON MISTAKE: attempted to traverse to nonexistent room. (incorrect reference name?)";
            } 

            //change room, clear console, then return room description
            _currRoom = foundRoom;
            Console.Clear();
            return "You went " + input[1]+ ".\n" + _currRoom.GetDescription();
        }
        private string InventoryCommand(string[] input)
        {
            if(input.Length != 1)
            {
                return "'inventory' is only typed by itself. Example: 'inventory'";
            }

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
                sb.Append("Items you can inspect in this area:\n");
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

                sb.Append("Items you can inspect in your inventory:\n");
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
                return "JSON MISTAKE: " + input[1] + " has duplicates!";
            }

            //now check if item exists. if it does, return it's description.
            var toBeInspected = FindItem(input[1]);
            if (toBeInspected == null)
            {
                return "There is no " + input[1] + " item in this area!";
            }
                return toBeInspected.Describe();
        }
        private string TurnCommand(string[] input)
        {
            //turn only works with three arguments! "Turn X On
            if (input.Length != 3 || (input[2] != "on" && input[2] != "off"))
            {
                return "Turn is only used in the format 'turn X on' or 'turn X off";
            }

            //check for duplicates
            if (HasDuplicates(input[1]))
            {
                return "JSON MISTAKE: " + input[1] + " has duplicates!";
            }

            //find the object
            var toBeTurned = FindItem(input[1]);

            //check for null
            if (toBeTurned == null)
            {
                return "There is no " + input[1] + " in this area!";
            }

            //if item exists, check if its toggleable
            if (!toBeTurned.Toggleable)
            {
                return "You cant 'turn' " + toBeTurned.Name + " " + input[2] + ".";
            }

            switch (input[2])
            {
                case "on":
                    toBeTurned.State = 1;
                    return toBeTurned.Name + " was turned on!";
                case "off":
                    toBeTurned.State = 0;
                    return toBeTurned.Name + " was turned off!";
                default:
                    //since we already checked for this, this should never return.
                    return "ERROR: TRIED TO TURN TO INVALID TYPE!";
            }
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
                return "JSON MISTAKE: " + input[1] + " has duplicates!";
            }

            //now check if item exists. if it does, return it's use message.
            var toBeUsed = FindItem(input[1]);
            if (toBeUsed == null)
            {
                return "There is no " + input[1] + " in this area!";
            }

            //if the object is toggleable, tell the player!
            if (toBeUsed.Toggleable)
            {
                return "To use this object, use the 'turn' command! Example: 'turn " + toBeUsed.Reference + " on.'";
            }
            return toBeUsed.Use(_gameData, _currRoom, input);
        }
        private string TakeCommand(string[] input)
        {
            //check validity of use
            if (!IsValidLength(2, input) || input.Length == 1)
            {
                return "'take' is used with one argument. Examples: 'take apple', 'take flashlight'";
            }

            //check for duplicates
            if (HasDuplicates(input[1]))
            {
                return "JSON MISTAKE: " + input[1] + " has duplicates!";
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
                return "You cant take the " + input[1] + "! It's too big!";
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
            int count = 0;
            foreach (var item in GetAllItems())
            {
                if(item.Reference == itemRef)
                {
                    count++;
                }
                if(count > 1)
                {
                    return true;
                }
            }
            return false;

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
