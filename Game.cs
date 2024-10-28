
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
                    response = _gameData.Help;
                    break;
                case "clear":
                    Console.Clear();
                    response = _currRoom.GetDescription();
                    break;
                case "inspect":
                case "investigate":
                    //List out objects you can interact with, 
                    response = InspectCommand(input);
                    break;
                case "use":
                    //use item code!
                    response = UseCommand(input);
                    break;
                case "go":
                    response = GoCommand(input);
                    break;
                default:
                    response = _gameData.Invalid;
                    break;
            }
            return response;
        }

        private string GoCommand(string[] input)
        {
            string response = string.Empty;
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
            }

            //if reference was set to a valid value, AND was not set to "0" or 1, traverse to that room.

            bool refIsLocked = reference.Equals("1"); 
            bool refIsInvalid = reference.Equals("0");
            bool refExists = reference.Length > 0;

            if (refExists && !refIsInvalid && !refIsLocked)
            {
                _currRoom = _gameData.Rooms.Find(x => x.Reference == reference); 
                response = "You went " + input[1]+ ".\n" + _currRoom.GetDescription();
            } else if(refIsLocked)
            {
                response = "That way is locked!";
            } else
            {
                response = "You can't go " + input[1];
            }

            return response;
        }

        private string InspectCommand(string[] input)
        {
            if(input.Length > 1)
            {
                // return description of the specific object
            } else
            {
                // return list of all objects in the room you can interact with.
            }

            return "";
        }

        private string UseCommand(string[] input)
        {
            string inputName = input[1];
            var item = _currRoom.StaticItems.Find(x => x.Name == inputName);

            if (item != null)
            {
                return item.Use(_gameData);
            } else
            {
                return "There is no " + inputName + " in this room!";
            }

        }
    }




}
