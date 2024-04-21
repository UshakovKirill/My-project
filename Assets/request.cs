using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Request
{
    public Human Requester { get; private set; } // Reference to the human who made the request

    public Request(Human requester)
    {
        Requester = requester;
    }
    public abstract string Description { get; }
   
}

public class FoodRequest : Request
{
    // Constructor that takes a Human object as the requester
    public FoodRequest(Human requester) : base(requester)
    {
        
    }

    public override string Description
    {
        get { return "Food Request by " + Requester.name; } // Use the Requester property
    }
}

public class HouseRequest : Request
{
    public HouseRequest(Human requester) : base(requester) { }

    public override string Description
    {
        get { return $"House Request by {Requester.name}"; }
    }
}

public class ItemRequest : Request
{
    public ItemSO RequiredItem { get; private set; }
    public int Quantity { get; private set; }

    public ItemRequest(Human requester, ItemSO requiredItem, int quantity) : base(requester)
    {
        RequiredItem = requiredItem;
        Quantity = quantity;
    }

    public override string Description => $"Item request by {Requester.name} for {Quantity}x {RequiredItem.itemName}";
}


public class WorkRequest : Request
{
    public WorkRequest(Human requester) : base(requester)
    {
    }

    public override string Description
    {
        get { return $"Work Request by {Requester.name}"; }
    }
}

// Дополнительный класс для запроса на место работы
public class WorkPlaceRequest : Request
{
    public Work Work { get; private set; }

    public WorkPlaceRequest(Human requester, Work work) : base(requester)
    {
        Work = work;
    }

    public override string Description
    {
        get { return $"WorkPlace Request by {Requester.name} for {Work.GetType().Name}"; }
    }
}



