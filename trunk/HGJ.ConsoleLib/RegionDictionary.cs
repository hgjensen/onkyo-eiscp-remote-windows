using System;
using System.Collections.Generic;

namespace HGJ.ConsoleLib {
  public class RegionDictionary<TKey, TValue> : Dictionary<TKey, TValue> {
    #region Delegates

    public delegate void AddEventHandler(AddEventArgs pAddEventArgs);

    public delegate void RemoveEventHandler(RemoveEventArgs pRemoveEventArgs);

    #endregion

    public event AddEventHandler AddEvent;
    public event RemoveEventHandler RemoveEvent;

    public void Add(TKey pKey, TValue pValue) {
      if (AddEvent != null)
        AddEvent(new AddEventArgs(pKey, pValue));
      base.Add(pKey, pValue);
    }

    public void Remove(TKey pKey) {
      if (RemoveEvent != null)
        RemoveEvent(new RemoveEventArgs(pKey));
      base.Remove(pKey);
    }

    #region Nested type: AddEventArgs

    public class AddEventArgs : EventArgs {
      private readonly TKey _key;
      private readonly TValue _value;

      public AddEventArgs(TKey key, TValue value) {
        _key = key;
        _value = value;
      }

      public TKey Key {
        get { return _key; }
      }

      public TValue Value {
        get { return _value; }
      }
    }

    #endregion

    #region Nested type: RemoveEventArgs

    public class RemoveEventArgs : EventArgs {
      private readonly TKey _key;

      public RemoveEventArgs(TKey key) {
        _key = key;
      }

      public TKey Key {
        get { return _key; }
      }
    }

    #endregion
  }
}