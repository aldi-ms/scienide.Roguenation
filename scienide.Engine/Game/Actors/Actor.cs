﻿namespace scienide.Engine.Game.Actors;

using SadRogue.Primitives;
using scienide.Common.Game;
using scienide.Common.Game.Interfaces;
using scienide.Common.Messaging;
using scienide.Common.Messaging.Events;
using System.Text.RegularExpressions;

public abstract partial class Actor : GameComposite, IActor
{
    private Ulid _id;
    private string _name;
    private ITimeEntity? _timeEntity;
    private IGameMap? _map;
    private Point _position;

    public Actor(Point pos, string name)
    {
        _id = Ulid.NewUlid();
        _name = name;
        _position = pos;
        Layer = Layer.Actor;
    }

    public Actor(Point pos) : this(pos, string.Empty)
    {
    }

    public Point Position
    {
        get
        {
            return _position;
        }
        set
        {
            if (_position == value)
            {
                GameMap.GameLogger.Warning("Trying to set {Name}'s position to the same value: {Position}.", Name, Position);
                return;
            }

            var oldCell = GameMap[Position];
            GameMap.DirtyCells.Add(oldCell);

            oldCell.RemoveComponent(this);

            _position = value;
            var newCell = GameMap[Position];
            newCell.AddComponent(this);

            GameMap.DirtyCells.Add(newCell);
        }
    }

    public string Name
    {
        get => _name;
        internal set => _name = value;
    }

    public Ulid TypeId
    {
        get => _id;
        protected set => _id = value;
    }

    public IActionCommand? Action { get; set; }

    public IGameMap GameMap
    {
        get
        {
            if (_map != null)
            {
                return _map;
            }

            if (Parent?.Parent is IGameMap map)
            {
                _map = map;
                return _map;
            }

            throw new ArgumentNullException(nameof(Parent));
        }
    }

    public ITimeEntity? TimeEntity
    {
        get { return _timeEntity; }
        set
        {
            _timeEntity = value;
            if (_timeEntity != null)
            {
                _timeEntity.Actor = this;
            }
        }
    }

    public int FoVRange { get; set; }

    public Glyph Glyph { get; set; }

    public Layer Layer { get; private set; }

    public Cell CurrentCell => GameMap[Position];

    public Dictionary<string, string> FetchComponentStatuses()
    {
        var componentMap = new Dictionary<string, string>();
        foreach (var c in Components)
        {
            if (!string.IsNullOrWhiteSpace(c.Status))
            {
                var key = c.GetType().Name;
                if (key.Length > 9)
                {
                    var split = SplitPascalCaseWords().Split(key);
                    key = split[^1];
                }
                componentMap.Add(key, c.Status);
            }
        }

        return componentMap;
    }

    public abstract IActionCommand TakeTurn();

    public abstract IActor Clone(bool deepClone);

    public virtual void SubscribeForMessages()
    {
        MessageBroker.Instance.Subscribe<GameMessageArgs>(Listener, this);
    }

    public virtual void UnsubscribeFromMessages()
    {
        MessageBroker.Instance.Unsubscribe<GameMessageArgs>(Listener, this);
    }

    private void Listener(GameMessageArgs args)
    {
        GameMap.GameLogger.Information("[{Name}] can hear message: {@args}.", Name, args);
    }

    [GeneratedRegex(@"(?<!^)(?=[A-Z])")]
    private static partial Regex SplitPascalCaseWords();
}
