﻿using System;

// ReSharper disable CheckNamespace
namespace BetterModules.Core.Events
// ReSharper restore CheckNamespace
{   
    public class SingleItemEventArgs<TItem> : EventArgs
    {
        public TItem Item { get; set; }

        public SingleItemEventArgs(TItem item)
        {
            Item = item;
        }
    }
}
