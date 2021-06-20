using System;
using System.Collections.Generic;

/// <summary>
/// Helper class used to Subscribe/Unsubscribe from Actions.
/// </summary>
public static class Observer
{
    /// <summary>
    /// Add a given method to a given Event.
    /// </summary>
    /// <param name="subject"> Event.</param>
    /// <param name="observer">Method called when the subject Event is called.</param>
    public static void AddObserver(ref Action subject, Action observer)
    {
        subject += observer;
    }
    public static void AddObserver<T>(ref Action<T> subject, Action<T> observer)
    {
        subject += observer;
    }
    public static void AddObserver<T1, T2>(ref Action<T1, T2> subject, Action<T1, T2> observer)
    {
        subject += observer;
    }
    public static void AddObserver<T1, T2, T3>(ref Action<T1, T2, T3> subject, Action<T1, T2, T3> observer)
    {
        subject += observer;
    }


    /// <summary>
    /// Remove a given Method from a given Event.
    /// </summary>
    /// <param name="subject">Event.</param>
    /// <param name="observer">Method to remove.</param>
    public static void RemoveObserver(ref Action subject, Action observer)
    {
        subject -= observer;
    }
    public static void RemoveObserver<T>(ref Action<T> subject, Action<T> observer)
    {
        subject -= observer;
    }
    public static void RemoveObserver<T1, T2>(ref Action<T1, T2> subject, Action<T1, T2> observer)
    {
        subject -= observer;
    }
    public static void RemoveObserver<T1, T2, T3>(ref Action<T1, T2, T3> subject, Action<T1, T2, T3> observer)
    {
        subject -= observer;
    }


    /// <summary>
    /// Add a list of Methods to a given Event.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="observers">list of methods to subscribe to the given Event.</param>
    public static void AddObservers(ref Action subject, IEnumerable<Action> observers)
    {
        foreach (var observer in observers)
        {
            subject += observer;
        }
    }
    public static void AddObservers<T>(ref Action<T> subject, IEnumerable<Action<T>> observers)
    {
        foreach (var observer in observers)
        {
            subject += observer;
        }
    }
    public static void AddObservers<T1, T2>(ref Action<T1, T2> subject, IEnumerable<Action<T1, T2>> observers)
    {
        foreach (var observer in observers)
        {
            subject += observer;
        }
    }
    public static void AddObservers<T1, T2, T3>(ref Action<T1, T2, T3> subject, IEnumerable<Action<T1, T2, T3>> observers)
    {
        foreach (var observer in observers)
        {
            subject += observer;
        }
    }


    /// <summary>
    /// Remove all methods subscribed to a given Event.
    /// </summary>
    /// <param name="subject"></param>
    public static void RemoveAllObservers(ref Action subject)
    {
        var observers = subject.GetInvocationList();

        foreach (var observer in observers)
        {
            subject -= (Action)observer;
        }
    }
    public static void RemoveAllObservers<T>(ref Action<T> subject)
    {
        var observers = subject.GetInvocationList();

        foreach (var observer in observers)
        {
            subject -= (Action<T>)observer;
        }
    }
    public static void RemoveAllObservers<T1, T2>(ref Action<T1, T2> subject)
    {
        var observers = subject.GetInvocationList();

        foreach (var observer in observers)
        {
            subject -= (Action<T1, T2>)observer;
        }
    }

    public static void RemoveAllObservers<T1, T2, T3>(ref Action<T1, T2, T3> subject)
    {
        var observers = subject.GetInvocationList();

        foreach (var observer in observers)
        {
            subject -= (Action<T1, T2, T3>)observer;
        }
    }
}
