
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
            Console.WriteLine(_currRoom.Description); //describes the room you are standing in for the first time.
        }

        public void ProcessString(string enteredString)
        {

            enteredString = enteredString.Trim().ToLower(); //trim any white space from the beginning or end of string and convert to lower case

            string[] commands = enteredString.Split(" "); //split based on spaces. The length of this array will tell you whether you have 1, 2, 3, 4, or more commands.
                                                          //modify these functions however you want, but this is where the beginning of calling functions to handle where you are
            string response = string.Empty;


            switch (commands.Length)
            {
                case 1:
                    //single command
                    response = SingleCommandParse(commands);
                    break;
                case 2:
                    //double trouble
                    response = DoubleCommandParse(commands);
                    break;
                case 3:
                    //tripple dipple
                    response = TripleCommandParse(commands);
                    break;
                case 4:
                    //quadruple oople
                    break;
                default:
                    //unknown!
                    response = _gameData.Invalid;
                    break;
            }
            
            Console.WriteLine(response); //what you tell the person after what they entered has been processed
        }

        private string SingleCommandParse(string[] input)
        {
            string response = string.Empty;
            switch (input[0])
            {
                case "help":
                    response = _gameData.Help;
                    break;
                default:
                    response = _gameData.Invalid;
                    break;
            }
            return response;
        }

        private string DoubleCommandParse(string[] input)
        {
            string response = string.Empty;
            switch (input[0])
            {
                case "use":
                    //use item code!

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

        private string TripleCommandParse(string[] input)
        {
            string response = string.Empty;
            switch (input[0])
            {
                //? not sure what kind of triple commands may be used.
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
                        reference = _currRoom.North;
                    break;

                case "east":
                        reference = _currRoom.East;
                    break;

                case "south":
                        reference = _currRoom.South;
                    break;

                case "west":
                        reference = _currRoom.West;
                    break;
            }

            //if reference was set to a valid value, AND was not set to "0", traverse to that room.
            if (reference.Length > 0 && !reference.Equals("0"))
            {
                _currRoom = _gameData.Rooms.Find(x => x.Reference == reference); 
                response = "You went " + input[1]+ ".\n" + _currRoom.Description;
            } else
            {
                response = "You can't go " + input[1];
            }

            return response;
        }

    }




}
