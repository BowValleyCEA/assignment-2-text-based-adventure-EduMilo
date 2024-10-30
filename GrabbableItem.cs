/*PLEASE READ:
 * GrabbableItems function differently from StaticItems in the sense that they have more constraints in the ways they can be used.
 *  -GrabbableItem cannot be a RoomChanger. 
 *      Since items can be moved around from one room to another within the player's inventory.
 * 
 * 
 * 
 */
namespace game1402_a2_starter
{
    [Serializable]
    public class GrabbableItem : Item
    {
    
        public override string Use(GameData gameData, Room currRoom, string[] input)
        {
            //if already used, just describe.
            if(State == 1)
            {
                return Describe();
            }

            //determine behavior based on type.
            switch(Type)
            {
                case ItemType.Descriptor:
                    return "You can't 'use' the " + Name + ". You CAN inspect it however.";
                case ItemType.RoomChanger:
                    //Note: since Grabbable items are meant to be used by 'use X on Y', they cannot be RoomChangers.
                    return "DEBUG: grabbable item incorrectly set to RoomChanger!";
                case ItemType.ItemChanger:
                    //if player simply typed in "use X", tell them it must be used on something.
                    if(input.Length != 4)
                    {
                        return "You must use the " + Name + " on something! Example: 'use " + Name + " on chair";
                    }
                    //otherwise, look for the specified item.
                    var tr = currRoom.StaticItems.Find(x => x.Reference == input[3]);

                    //if null, say the item could not be found.
                    if(tr == null)
                    {
                        return "There is no " + input[3] + " in this room!";
                    }

                    //if the searched item exists, but does not match the one this item is meant to be used with, say the items cannot be used together.
                    if(tr.Reference != TargetReference)
                    {
                        return "You aren't supposed to use the " + Name + " on the " + input[3] + "!";
                    }
                    
                    //Now that we know we have the right targetItem, change it.
                    tr.State = TargetState;
                    //set this item as used
                    State = 1;

                    return UseText;
                default:
                    //type was invalid
                    return "DEBUG - Item " + Name + " did not have valid type!";


            }
        }
    }
}
