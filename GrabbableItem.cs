namespace game1402_a2_starter
{
    [Serializable]
    public class GrabbableItem : Item
    {
        public override string Use(GameData gameData, Room currRoom, string[] input)
        {
            //determine behavior based on type.
            switch(Type)
            {
                case ItemType.Descriptor:
                    return "You can't 'use' the " + Name + ". You CAN inspect it however.";
                case ItemType.RoomChanger:
                    //check if this item has a targetRoom
                    bool hasTarget = (TargetReference != "" && TargetState != -1);
                    if (!hasTarget) return "DEBUG - Item does not have target and target state!";

                    //check if targetRoom is null before changing the state.
                    var tr = gameData.Rooms.Find(x => x.Reference == TargetReference);
                    if (tr == null) return "DEBUG - RoomChanger could not find target room!";
                    tr.State = TargetState;

                    //set this item as used, then make it a descriptor.
                    //that way, it can both no-longer be used AND have a new description.
                    State = 1;
                    Type = ItemType.Descriptor;
                    return UseText;
                case ItemType.ItemChanger:
                    //if player simply typed in "use X", tell them it must be used on something.
                    if(input.Length != 4)
                    {
                        return "You must use the " + Name + " on something! Example: 'use " + Reference + " on chair'";
                    }
                    //otherwise, look for the specified item.
                    var ti = currRoom.StaticItems.Find(x => x.Reference == input[3]);

                    //if null, say the item could not be found.
                    if(ti == null)
                    {
                        return "There is no " + input[3] + " in this room!";
                    }

                    //if the targetItem exists, but does not match the one this item is meant to be used with, say the items cannot be used together.
                    if(ti.Reference != TargetReference)
                    {
                        return "You aren't supposed to use the " + Name + " on the " + input[3] + "!";
                    }
                    
                    //Now that we know we have the right targetItem, change it.
                    ti.State = TargetState;

                    //set this item as used, then make it a descriptor.
                    //that way, it can both no-longer be used AND have a new description.
                    State = 1;
                    Type = ItemType.Descriptor;
                    return UseText;
                default:
                    //type was invalid
                    return "DEBUG - Item " + Name + " did not have valid type!";


            }
        }
    }
}
