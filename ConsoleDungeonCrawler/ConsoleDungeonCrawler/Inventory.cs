
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Inventory
{
    public List<ItemWrapper> content = new List<ItemWrapper>();
    public Inventory()
    {
    }

    public void Add(Item item, int count)
    {
        bool newItem = true;
        ItemWrapper wrapper = new ItemWrapper(item, count);

        /*if(item.type == "cons")
        {
            Console.WriteLine("item consumed");
            return;
        }
        if(item.type == "ammo")
        {
            Console.WriteLine("ammo added");
        }
         * */

        //if item exists within inventory, count up
        for (int i = 0; i < content.Count; i++)
        {
            if (content[i].item.name == wrapper.item.name)
            {
                content[i].count += 1;
                newItem = false;
            }
        }

        //if item not found, add it as new item with count 1
        if (newItem)
        {
            content.Add(wrapper);
        }
    }

    public void Remove(Item item)
    {
        // TODO implement here
    }

    public ItemWrapper Get(int index)
    {
        return content[index];
    }

}