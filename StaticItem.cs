using System.Net.Security;

namespace game1402_a2_starter
{
    [Serializable]
    public class StaticItem : Item
    {
        //note, the input parameter is not used by StaticItems, as they never have to check for 'target objects'
        public override string Use(GameData gameData, Room currRoom, string[] input)
        {
            //if item has already been used, simply return it's second description and do no further actions.
            if(State == 1)
            {
                return Describe();
            }

            switch (Type)
            {
                case ItemType.Descriptor:
                    return "You can't 'use' the " + Name + ". You CAN inspect it however.";
                case ItemType.RoomChanger:
                case ItemType.ItemChanger:

                    //check if this item has a target
                    bool hasTarget = (TargetReference != "" && TargetState != -1);
                    if (!hasTarget) return "DEBUG - Item does not have target and target state!";

                    //then change the target's state.
                    if (Type == ItemType.RoomChanger)
                    {
                        //check if it's null before changing the state.
                        var tr = gameData.Rooms.Find(x => x.Reference == TargetReference);
                        if (tr == null) return "DEBUG - RoomChanger could not find target room!";
                        tr.State = TargetState;
                    }
                    else
                    {
                        var tr = currRoom.StaticItems.Find(x => x.Reference == TargetReference);
                        if (tr == null) return "DEBUG - ItemChanger could not find target item!";
                        tr.State = TargetState;
                    }

                    //set this item as used
                    State = 1;
                    return UseText;
                default:
                    return "DEBUG - Item " + Name + "did not have valid type!";
            }
        }


    }
}
