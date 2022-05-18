using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hospital.Desktop.Model.Util;

/// <summary>
/// A container type which observes the changes to the value and can be chained together with other state instances
/// </summary>
/// <typeparam name="T">Type of the Value stored</typeparam>
public class State<T> :  INotifyPropertyChanged
{
    private T _value;

    public State(T value)
    {
        _value = value;
    }
    public T Value
    {
        get => _value;
        set
        {
            if (Equals(_value, value)) return;
            _value = value;
            ValueChanged?.Invoke(this, value);
            OnPropertyChanged();
        }
    }
    
    /// <summary>
    /// Creates a new State instance with the Value changing every time the original State's Value changes.
    /// </summary>
    /// <param name="transform">The logic on which the original State Value transforms into the new State's Value</param>
    /// <typeparam name="T2">The type of the Value the new State can store</typeparam>
    /// <returns>The new State having the original State as it's dependency</returns>
    public State<T2> Select<T2>(Func<T, T2> transform)
    {
        var state = new State<T2>(transform(Value));
        ValueChanged += (sender, e) => state.Value = transform(e);
        return state;
    }

    public State<T2> SelectTwoWay<T2>(Func<T, T2> transform, Func<T2, T> inverse)
    {
        if (!Equals(inverse(transform(Value)), Value))
            throw new Exception("The inverse parameter must be the inverse if the transform function");

        var state = Select(transform);
        state.ValueChanged += (_, v) => Value = inverse(v);
        return state;
    }

    /// <summary>
    /// Makes the other State's Value reflect this State's Value. If this State's Value changes,
    /// The other State's Value will also change to the same Value.
    /// </summary>
    /// <param name="other">The State instance which will be influenced by this State</param>
    public void Subscribe(State<T> other)
    {
        Subscribe(other, x => x);
    }

    /// <summary>
    /// Makes the other State's Value reflect this State's Value. If this State's Value changes,
    /// The other State's Value will also change.
    /// </summary>
    /// <param name="other">The State instance which will be influenced by this State</param>
    /// <param name="transform">The transformation which is executed on each of this State's Value</param>
    /// <typeparam name="T2">The type the other State can Store as Value</typeparam>
    public void Subscribe<T2>(State<T2> other, Func<T, T2> transform)
    {
        other.Value = transform(Value);
        ValueChanged += (_, _) => other.Value = transform(Value);
    }

    public State<T3> Combine<T2, T3>(State<T2> other, Func<T, T2, T3>  transform)
    {
        var state = new State<T3>(transform(Value, other.Value));
        ValueChanged += (_, v) => state.Value = transform(v, other.Value);
        other.ValueChanged += (_, v) => state.Value = transform(Value, v);
        return state;
    }
    
    public State<T2> SelectMany<T2>(Func<T, State<T2>> transform)
    {
        var state = transform(Value);
        ValueChanged += (_, v) =>
        {
            transform(v).Subscribe(state);
        };
        return state;
    }

    public ObservableCollection<T2> SelectMany<T2>(Func<T, ObservableCollection<T2>> transform)
    {
        var oc = new ObservableCollection<T2>(transform(Value));
        transform(Value).Subscribe(oc);
        ValueChanged += (_, v) =>
        {
            transform(v).Subscribe(oc);
        };
        return oc;

    }

    public void EmitValueChanged()
    {
        ValueChanged?.Invoke(this, Value);
    }

    public State<T> Do(Action<T> action)
    {
        ValueChanged += (_, v) => action(v);
        return this;
    }

    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public event EventHandler<T>? ValueChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
}