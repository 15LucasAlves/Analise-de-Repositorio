using System;

class UpdateOrderChangedEventArgs : EventArgs
{
    public int UpdateOrder { get; private set; }
    public int PreviousUpdateOrder { get; private set; }

    public UpdateOrderChangedEventArgs(int updateOrder, int previousUpdateOrder)
    {
        UpdateOrder = updateOrder;
        PreviousUpdateOrder = previousUpdateOrder;
    }
}