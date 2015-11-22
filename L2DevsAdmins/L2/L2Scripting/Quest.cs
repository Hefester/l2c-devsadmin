using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using L2.Net.Attributes;
using System.Timers;

namespace L2.L2Scripting
{

    public delegate void QuestLoadHandler();

    /// <summary>
    /// Quest Class, 
    /// </summary>
    [Author("Hefester", version = 1.0)]
    public class Quest
    {
        //Engine's
        private LuaEngine luaEngine;

        private static readonly string LOAD_QUEST_STATES = "Query__";
        private static readonly string LOAD_QUEST_VARIABLES = "Query__";
        private static readonly string DELETE_INVALID_QUEST = "Query__";

        [LuaVisible(Visible = true)]
        private static readonly string HTML_NONE_AVAILABLE = "<html><body>You are either not on a quest that involves this NPC, or you don't meet this NPC's minimum quest requirements.</body></html>";
        [LuaVisible(Visible = true)]
        private static readonly string HTML_ALREADY_COMPLETED = "<html><body>This quest has already been completed.</body></html>";
        [LuaVisible(Visible = true)]
        private static readonly string HTML_TOO_MUCH_QUESTS = "<html><body>You have already accepted the maximum number of quests. No more than 25 quests may be undertaken simultaneously.<br>For quest information, enter Alt+U.</body></html>";
	
        [LuaVisible(Visible =true)]
	    public static readonly byte STATE_CREATED = 0;
        [LuaVisible(Visible = true)]
        public static readonly byte STATE_STARTED = 1;
        [LuaVisible(Visible = true)]
        public static readonly byte STATE_COMPLETED = 2;

        //Events
        public event QuestLoadHandler QuestLoadHandler;

        private readonly IDictionary<int, BlockingCollection<QuestTimer>> _eventTimers = new ConcurrentDictionary<int, BlockingCollection<QuestTimer>>();

        private readonly int _id;
        private readonly string _name;
        private readonly string _descr;
        private bool _onEnterWorld;
        private int[] _itemsId;
        

        /// <summary>
        /// Initialize quest.
        /// </summary>
        /// <param name="questId"></param>
        /// <param name="name"></param>
        /// <param name="descr"></param>
        public Quest(int questId, string name, string descr)
        {
            _id = questId;
            _name = name;
            _descr = descr;

            //define handler
            QuestLoadHandler += OnQuestLoad;
            QuestLoadHandler();
        }
        /// <summary>
        /// Return Id quest
        /// </summary>
        /// <returns></returns>
        public int QuestId
        {
           get
           {
               return _id;
           }
        }
        /// <summary>
        /// Return type of the quest.
        /// </summary>
        /// <returns>True for (live) quest, False for script, AI, etc.</returns>
        public bool IsRealQuest
        {
            get
            {
                return _id > 0;
            }
        }


        /// <summary>
        /// Return's name of the quest
        /// </summary>
        /// <returns></returns>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Return descr of the quest
        /// </summary>
        /// <returns></returns>
        [LuaVisible(Visible =true)]
        public string Descr
        {
            get
            {
                return _descr;
            }
        }

        //Event's Quest.
        /// <summary>
        /// Load Quest Actions..
        /// </summary>
        public virtual void OnQuestLoad()
        {

        }


        /// <summary>
        /// Debug Mode quest.
        /// </summary>
        public bool PrintDebug
        {
            get;
            set;
        }

        [LuaVisible(Visible = true)]
        public bool OnEnterWorld
        {
            get
            {
                return _onEnterWorld;
            }
            set
            {
                _onEnterWorld = value;
            }
        }

        /// <summary>
        /// Return registered quest items.
        /// </summary>
        /// <returns></returns>
        [LuaVisible(Visible = true)]
        public int[] GetItemsIds()
        {
            return _itemsId;
        }

        /// <summary>
        /// Registers all items that have to be destroyed in case player abort the quest or finish it.
        /// </summary>
        /// <param name="items"></param>
        [LuaVisible(Visible =true)]
        public void RegisterItemsQuest(params int[] items)
        {
            if(_itemsId == null)
            {
                _itemsId = items;
            }
            else
            {
                List<int> _arrayTemp = new List<int>(_itemsId);
                _arrayTemp.AddRange(_itemsId);
                if(_arrayTemp.Count > _itemsId.Length)
                {
                    Array.Resize<int>(ref _itemsId, _arrayTemp.Count + 1);
                }
                _itemsId = _arrayTemp.ToArray();
            }
            
        }
        [LuaVisible(Visible = true)]
        public void RegisterItemQuest(int item)
        {
            if(_itemsId == null)
            {
                _itemsId = new int[5];
            }
            List<int> _arrayTemp = new List<int>(_itemsId);
            _arrayTemp.Add(item);
            if(_arrayTemp.Count > _itemsId.Length)
            {
                Array.Resize<int>(ref _itemsId, _arrayTemp.Count + 1);
            }
            _itemsId = _arrayTemp.ToArray();
        }

        /// <summary>
        /// Add a timer to the quest, if it doesn't exist already. If the timer is repeatable, it will auto-fire automatically, at a fixed rate, until explicitly canceled.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="time">time </param>
        /// <param name="npc"></param>
        /// <param name="player"></param>
        /// <param name="repeat"></param>
        [LuaVisible(Visible =true)]
        public void StartQuestTimer(string name, long time, object npc, object player, bool repeat)
        {
            BlockingCollection<QuestTimer> timers = null;
            _eventTimers.TryGetValue(name.GetHashCode(), out timers);
            if(timers == null)
            {
                timers = new BlockingCollection<QuestTimer>();
                timers.Add(new QuestTimer(this, name, npc, player, time, repeat));
                _eventTimers.Add(name.GetHashCode(), timers);
            }
            else
            {
                foreach(QuestTimer timer in timers)
                {
                    if (timer != null && timer.Equals(this, name, npc, player))
                        return;
                }
                timers.Add(new QuestTimer(this, name, npc, player, time));
            }
        }
        /// <summary>
        /// Get a timer of quest
        /// </summary>
        /// <param name="name"></param>
        /// <param name="npc"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        [LuaVisible(Visible =true)]
        public QuestTimer getQuestTimer(string name, object npc, object player)
        {
            // Get quest timers for this timer type.
            BlockingCollection<QuestTimer> timers = null;
            _eventTimers.TryGetValue(name.GetHashCode(), out timers);

            // Timer list does not exists or is empty, return.
            if (timers == null || timers.Count == 0)
                return null;

            // Check, if specific timer exists.
            foreach (QuestTimer timer in timers)
            {
                // If so, return him.
                if (timer != null && timer.Equals(this, name, npc, player))
                    return timer;
            }
            return null;
        }
        /// <summary>
        /// Cancel Timer
        /// </summary>
        /// <param name="name"></param>
        /// <param name="npc"></param>
        /// <param name="player"></param>
        [LuaVisible(Visible = true)]
        public void cancelQuestTimer(string name, object npc, object player)
        {
            // If specified timer exists, cancel him.
            QuestTimer timer = getQuestTimer(name, npc, player);
            if (timer != null)
                timer.Cancel();
        }
        [LuaVisible(Visible = true)]
        public void cancelQuestTimers(String name)
        {
            // Get quest timers for this timer type.
            BlockingCollection<QuestTimer> timers = null;
            _eventTimers.TryGetValue(name.GetHashCode(), out timers);

            // Timer list does not exists or is empty, return.
            if (timers == null || timers.Count == 0)
                return;

            // Cancel all quest timers.
            foreach (QuestTimer timer in timers)
            {
                if (timer != null)
                    timer.Cancel();
            }
        }
        [LuaVisible(Visible =true)]
        void removeQuestTimer(QuestTimer timer)
        {
            // Timer does not exist, return.
            if (timer == null)
                return;

            // Get quest timers for this timer type.
            BlockingCollection<QuestTimer> timers = null;
            _eventTimers.TryGetValue(timer.ToString().GetHashCode(), out timers);

            // Timer list does not exists or is empty, return.
            if (timers == null || timers.Count == 0)
                return;

            timers.TryTake(out timer);
        }

        //Engine Loader

        /// <summary>
        /// Lua Engine
        /// </summary>
        /// <param name="engine"></param>
        public void loadByScriptEngine(LuaEngine engine)
        {
            luaEngine = engine;
        }


        


    }

    /// <summary>
    /// Timer for Quest's
    /// </summary>
    public class QuestTimer
    {
        protected readonly Quest _quest;
        protected readonly string _name;
        protected readonly object _npc;
        protected readonly object _player;
        protected readonly bool _isRepeating;

        protected bool _isActive = true;

        private Timer _timer;
        /// <summary>
        /// Create a timer
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="name"></param>
        /// <param name="npc"></param>
        /// <param name="player"></param>
        /// <param name="time"></param>
        /// <param name="repeating"></param>
        public QuestTimer(Quest quest, string name, object npc, object player, long time, bool repeating)
        {
            _quest = quest;
            _name = name;
            _npc = npc;
            _player = player;
            _isRepeating = repeating;
            _timer = new Timer(time);
            _timer.Elapsed += new ElapsedEventHandler(TimerAction);
            _timer.AutoReset = _isRepeating;
            _timer.Start();
        }

        /// <summary>
        /// No repeating timer
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="name"></param>
        /// <param name="npc"></param>
        /// <param name="player"></param>
        /// <param name="time"></param>
        public QuestTimer(Quest quest, string name, object npc, object player, long time) : this(quest, name, npc, player, time, false)
        {

        }

        void TimerAction(object source, ElapsedEventArgs e)
        {
            if(!_isActive)
            {
                return;
            }
            if(!_isRepeating)
            {
                Cancel();
            }
            //_quest.NotifyEvent(_name, _npc, _player);
        }
        /// <summary>
        /// Get Name of timer
        /// </summary>
        /// <returns>name of timer</returns>
        public override string ToString()
        {
            return _name;
        }

        /// <summary>
        /// Cancel timer & remove
        /// </summary>
        public void Cancel()
        {
            _isActive = false;

            _timer.Stop();

            //_quest.removeQuestTimer(this);
        }

        /// <summary>
        /// public method to compare if this timer matches with the key attributes passed.
        /// </summary>
        /// <param name="quest"></param>
        /// <param name="name"></param>
        /// <param name="npc"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool Equals(Quest quest, String name, object npc, object player)
        {
            if (quest == null || quest != _quest)
                return false;

            if (name == null || !name.Equals(_name))
                return false;

            return ((npc == _npc) && (player == _player));
        }

    }

    /// <summary>
    /// Event Types
    /// </summary>
    [Author("Hefester", version =1.0)]
    public enum QuestEventType : byte
    {
        /// <summary>
        /// control the first dialog shown by NPCs when they are clicked (some quests must override the default npc action)
        /// </summary>
        ON_FIRST_TALK = 0,
        /// <summary>
        /// onTalk action from start npcs
        /// </summary>
        QUEST_START = 1,
        /// <summary>
        /// onTalk action from npcs participating in a quest
        /// </summary>
        ON_TALK = 1,
        /// <summary>
        /// onAttack action triggered when a mob gets attacked by someone
        /// </summary>
        ON_ATTACK = 1,
        /// <summary>
        /// onAttackAct event is triggered when a mob attacks someone
        /// </summary>
        ON_ATTACK_ACT = 1,
        /// <summary>
        /// onKill action triggered when a mob gets killed.
        /// </summary>
        ON_KILL = 1,
        /// <summary>
        /// onSpawn action triggered when an NPC is spawned or respawned.
        /// </summary>
        ON_SPAWN = 1,
        /// <summary>
        /// NPC or Mob saw a person casting a skill (regardless what the target is).
        /// </summary>
        ON_SKILL_SEE = 1,
        /// <summary>
        /// NPC or Mob saw a person casting a skill (regardless what the target is).
        /// </summary>
        ON_FACTION_CALL = 1,
        /// <summary>
        /// a person came within the Npc/Mob's range
        /// </summary>
        ON_AGGRO_RANGE_ENTER = 1,
        /// <summary>
        /// on spell finished action when npc finish casting skill
        /// </summary>
        ON_SPELL_FINISHED = 1,
        /// <summary>
        /// on zone enter
        /// </summary>
        ON_ENTER_ZONE = 1,
        /// <summary>
        /// on zone exit
        /// </summary>
        ON_EXIT_ZONE = 1
    }

}
