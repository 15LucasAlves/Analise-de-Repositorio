using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

class GameObject : IUpdateable
{
    // Update fields
    private bool enabled;
    public bool Enabled
    {
        get => enabled;
        set
        {
            bool previousEnabled = enabled;
            enabled = value;
            EnabledChanged?.Invoke(this, new EnabledChangedEventArgs(enabled, previousEnabled));
        }
    }

    private int updateOrder;
    public int UpdateOrder
    {
        get => updateOrder;
        set
        {
            int previousUpdateOrder = updateOrder;
            updateOrder = value;
            UpdateOrderChanged?.Invoke(this, new UpdateOrderChangedEventArgs(updateOrder, previousUpdateOrder));
        }
    }

    public event EventHandler<EventArgs> EnabledChanged;
    public event EventHandler<EventArgs> UpdateOrderChanged;

    // Parenting Fields
    public GameObject Parent { get; set; }
    public List<GameObject> Children { get; private set; }

    public GameObject()
    {
        Children = new List<GameObject>();
    }
        
    public virtual void Update(GameTime gameTime) {
        if (!Enabled) return;

        foreach(GameObject child in Children)
        {
            child.Update(gameTime);
        }
    }
}