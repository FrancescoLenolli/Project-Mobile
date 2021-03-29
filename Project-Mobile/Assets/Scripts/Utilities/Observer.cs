using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Observer
{
    /// <summary>
    /// Add a given method to a given Event.
    /// </summary>
    /// <param name="subject"> Event.</param>
    /// <param name="observer">Method called when the subject Event is called.</param>
    public static void ObserveSubject(Action subject, Action observer)
    {
        subject += observer;
    }
    public static void ObserveSubject<T>(Action<T> subject, Action<T> observer)
    {
        subject += observer;
    }
    public static void ObserveSubject<T1, T2>(Action<T1, T2> subject, Action<T1, T2> observer)
    {
        subject += observer;
    }
    public static void ObserveSubject<T1, T2, T3>(Action<T1, T2, T3> subject, Action<T1, T2, T3> observer)
    {
        subject += observer;
    }

    /// <summary>
    /// Remove a given Method from a given Event.
    /// </summary>
    /// <param name="subject">Event.</param>
    /// <param name="observer">Method to remove.</param>
    public static void StopObservingSubject(Action subject, Action observer)
    {
        subject -= observer;
    }
    public static void StopObservingSubject<T>(Action<T> subject, Action<T> observer)
    {
        subject -= observer;
    }
    public static void StopObservingSubject<T1, T2>(Action<T1, T2> subject, Action<T1, T2> observer)
    {
        subject -= observer;
    }
    public static void StopObservingSubject<T1, T2, T3>(Action<T1, T2, T3> subject, Action<T1, T2, T3> observer)
    {
        subject -= observer;
    }

    /// <summary>
    /// Remove all methods subscribed to a given Event.
    /// </summary>
    /// <param name="subject"></param>
    public static Action RemoveAllObservers(Action subject)
    {
        var observers = subject.GetInvocationList();

        foreach(var observer in observers)
        {
            subject -= (Action)observer;
        }

        return subject;
    }
    public static Action<T> RemoveAllObservers<T>(Action<T> subject)
    {
        var observers = subject.GetInvocationList();

        foreach (var observer in observers)
        {
            subject -= (Action<T>)observer;
        }

        return subject;
    }
    public static Action<T1, T2> RemoveAllObservers<T1, T2>(Action<T1, T2> subject)
    {
        var observers = subject.GetInvocationList();

        foreach (var observer in observers)
        {
            subject -= (Action<T1, T2>)observer;
        }

        return subject;
    }
    public static Action<T1, T2, T3> RemoveAllObservers<T1, T2, T3>(Action<T1, T2, T3> subject)
    {
        var observers = subject.GetInvocationList();

        foreach (var observer in observers)
        {
            subject -= (Action<T1, T2, T3>)observer;
        }

        return subject;
    }

    /// <summary>
    /// Add a list of Methods to a given Event.
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="observers">list of methods to subscribe to the given Event.</param>
    public static Action AddObserversToSubject(Action subject, List<Action> observers)
    {
        foreach (var observer in observers)
        {
            subject += observer;
        }

        return subject;
    }
    public static Action<T> AddObserversToSubject<T>(Action<T> subject, List<Action<T>> observers)
    {
        foreach(var observer in observers)
        {
            subject += observer;
        }

        return subject;
    }
    public static Action<T1, T2> AddObserversToSubject<T1, T2>(Action<T1, T2> subject, List<Action<T1, T2>> observers)
    {
        foreach (var observer in observers)
        {
            subject += observer;
        }

        return subject;
    }
    public static Action<T1, T2, T3> AddObserversToSubject<T1, T2, T3>(Action<T1, T2, T3> subject, List<Action<T1, T2, T3>> observers)
    {
        foreach (var observer in observers)
        {
            subject += observer;
        }

        return subject;
    }
}
