using System;
using System.Collections.ObjectModel;

namespace Hospital.Desktop.Model.Util;

public static class ObservableCollectionExtensions
{
    public static State<T> AsState<T, T2>(this ObservableCollection<T2> oc, Func<ObservableCollection<T2>, T> transform)
    {
        var state = new State<T>(transform(oc));
        oc.CollectionChanged += (_, e) => state.Value = transform(oc);
        return state;
    }

    public static ObservableCollection<T> Do<T>(this ObservableCollection<T> oc, Action<T> action)
    {
        foreach (var item in oc)
        {
            action(item);
        }

        oc.CollectionChanged += (_, e) =>
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var item2 = (T) item;
                    action(item2);
                }
            }
        };
        return oc;
    }

    public static void Subscribe<T>(this ObservableCollection<T> c1, ObservableCollection<T> c2)
    {
        c2.Clear();
        foreach (var item in c1)
        {
            c2.Add(item);
        }

        c1.CollectionChanged += (_, e) =>
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    c2.Add((T)item);
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    c2.Remove((T) item);
                }
            }
        };
    }
}