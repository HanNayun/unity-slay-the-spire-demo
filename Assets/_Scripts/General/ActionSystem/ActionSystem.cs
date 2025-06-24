using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ActionSystem : Singleton<ActionSystem>
{
    private static Dictionary<Type, List<Action<GameAction>>> typeToPrevSubs = new();
    private static Dictionary<Type, List<Action<GameAction>>> typeToPostSubs = new();
    private static Dictionary<Type, Func<GameAction, IEnumerator>> typeToPerformers = new();

    private List<GameAction> reactions;
    public bool IsPerforming { get; private set; }

    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);

        if (typeToPerformers.ContainsKey(type))
            typeToPerformers[type] = wrappedPerformer;
        else
            typeToPerformers.Add(type, wrappedPerformer);
        return;

        IEnumerator wrappedPerformer(GameAction action)
        {
            return performer(action as T);
        }
    }

    public static void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);
        typeToPerformers.Remove(type);
    }

    public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where  T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>>
            subs = timing == ReactionTiming.Pre ? typeToPrevSubs : typeToPostSubs;
        Type type = typeof(T);
        subs.TryAdd(type, new List<Action<GameAction>>());
        subs[type].Add(WrappedReaction);

        return;

        void WrappedReaction(GameAction action)
        {
            reaction(action as T);
        }
    }

    public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>>
            subs = timing == ReactionTiming.Pre ? typeToPrevSubs : typeToPostSubs;
        Type type = typeof(T);
        if (subs.ContainsKey(type)) subs[type].Remove(WrappedReaction);

        return;

        void WrappedReaction(GameAction action)
        {
            reaction(action as T);
        }
    }

    public void Perform(GameAction action, Action onPerformFinished = null)
    {
        if (IsPerforming) return;

        IsPerforming = true;
        StartCoroutine(Flow(action, () =>
        {
            IsPerforming = false;
            onPerformFinished?.Invoke();
        }));
    }

    public void AddReaction(GameAction gameAction)
    {
        reactions?.Add(gameAction);
    }

    private IEnumerator Flow(GameAction action, Action onFlowFinished = null)
    {
        reactions = action.PrevActions;
        PerformSubscribers(action, typeToPrevSubs);
        yield return PerformReactions();

        reactions = action.PerformActions;
        yield return PerformPerformer(action);
        yield return PerformReactions();

        reactions = action.PostActions;
        PerformSubscribers(action, typeToPostSubs);
        yield return PerformReactions();

        onFlowFinished?.Invoke();
    }

    private static void PerformSubscribers(GameAction action,
                                           Dictionary<Type, List<Action<GameAction>>> typeToActionSubs)
    {
        Type type = action.GetType();
        if (!typeToActionSubs.TryGetValue(type, out List<Action<GameAction>> actionSubs)) return;

        foreach (Action<GameAction> sub in actionSubs) sub(action);
    }

    private static IEnumerator PerformPerformer(GameAction action)
    {
        Type type = action.GetType();
        if (typeToPerformers.TryGetValue(type, out Func<GameAction, IEnumerator> performer))
            yield return performer(action);
    }

    /// <summary>
    ///     执行 <see cref="reactions" /> 里的 <see cref="GameAction" />
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformReactions()
    {
        foreach (GameAction reaction in reactions.ToList()) yield return Flow(reaction);
    }
}