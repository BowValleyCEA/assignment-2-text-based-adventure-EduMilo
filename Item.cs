﻿using static game1402_a2_starter.StaticItem;

namespace game1402_a2_starter
{
    public class Item
    {
        //for JSON serializing to work properly, all of these fields must be public.
        //The actual serializing occurs in the StaticItem and GrabbableItem classes.

        public string Name { get; set; }
        public ItemType Type { get; set; }
        public string UseText { get; set; }
        public string Reference { get; set; }
        public string[] Description { get; set; }
        public int State { get; set; } //state of the object. 0 == unused. 1 == used. 
        public string TargetReference { get; set; }
        public int TargetState { get; set; }
        public string Describe()
        {
            if(State >= Description.Length)
            {
                return "DEBUG: ITEM TRIED TO LOAD A DESCRIPTION OUT OF BOUNDS!";
            }
            return Description[State];
        }

        public virtual string Use(GameData gameData, Room currRoom, String[] input)
        {
            return "DEBUG: TRIED TO LOAD (use) FROM THE PARENT ITEM CLASS!";
        }

        public enum ItemType
        {
            Descriptor,
            RoomChanger,
            ItemChanger
        }
    }


}