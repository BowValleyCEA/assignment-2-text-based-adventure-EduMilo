namespace game1402_a2_starter
{
    [Serializable]
    public class StaticItem
    {
        //Out of headaches from trying to deserialize child classes from JSON, all item functions are now collapsed into this one big ol class.
        public string Name { get; set; }
        public ItemType Type { get; set; }

        public string UseText { get; set; }
        public string Reference {  get; set; }
        public string[] Description { get; set; }
        public int State { get; set; }
        public string TargetRoom {  get; set; } 
        public int TargetState {  get; set; }

        public string Describe()
        {
            return Description[State];
        }
        public string Use(GameData gameData)
        {
            

            switch (Type)
            {
                case ItemType.Descriptor:
                    return "You can't 'use' the " + Name;
                case ItemType.Toggler:
                    bool hasTarget = (TargetRoom != "" && TargetState != -1);
                    if (!hasTarget) return "DEBUG - Toggler does not have target room and target state!";
                    
                    //check if it's null before changing the state.
                    var tr = gameData.Rooms.Find(x => x.Reference == TargetRoom);
                    if (tr != null)
                    {
                        tr.State = TargetState;
                    }
                    else
                    {
                        //couldn't find targetRoom, do not use object!
                        return "DEBUG - Could not find target room!";
                    }
                    return UseText;
                default:
                    return "DEBUG - Item did not have valid type!";
            }
        }

        public enum ItemType
        {
            Descriptor,
            Toggler
            
        }
        


    }
}
