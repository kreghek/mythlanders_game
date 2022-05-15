using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg.Client.Core
{
    public interface IStoryPoint: IJobExecutable
    {
        public IReadOnlyCollection<IStoryPointAftermath>? Aftermaths { get; }

        public bool IsComplete { get; }

        public string Title { get; }
    }
    
    internal sealed class StoryPoint: IStoryPoint
    {
        private readonly IStoryPointAftermathContext _aftermathContext;

        public StoryPoint(IStoryPointAftermathContext aftermathContext)
        {
            _aftermathContext = aftermathContext;
        }

        public IReadOnlyCollection<IJob>? CurrentJobs { get; init; }
        public void HandleCompletion()
        {
            if (Aftermaths is null)
            {
                return;
            }

            foreach (var storyPointAftermath in Aftermaths)
            {
                storyPointAftermath.Apply(_aftermathContext);
            }

            IsComplete = true;
        }

        public IReadOnlyCollection<IStoryPointAftermath>? Aftermaths { get; init; }
        public bool IsComplete { get; private set; }

        public string Title { get; init; }
    }

    internal sealed class StoryPointCatalog
    {
        public IReadOnlyCollection<IStoryPoint> Create(Globe globe)
        {
            var list = new List<IStoryPoint>();

            var story2 = new StoryPoint(new StoryPointAftermathContext())
            {
                CurrentJobs = new[]
                {
                    new Job
                    {
                        Scheme = new JobScheme
                        {
                            Scope = JobScope.Global,
                            Type = JobType.Combats,
                            Value = 3
                        }
                    }
                },
                Aftermaths = new IStoryPointAftermath[]
                {
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.Swamp)),
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.GreatWall)),
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.ScreamValey)),
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.Oasis)),
                }
            };

            var story1 = new StoryPoint(new StoryPointAftermathContext())
            {
                CurrentJobs = new[]
                {
                    new Job
                    {
                        Scheme = new JobScheme
                        {
                            Scope = JobScope.Global,
                            Type = JobType.Combats,
                            Value = 3
                        }
                    }
                },
                Aftermaths = new IStoryPointAftermath[]
                {
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.Battleground)),
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.GaintBamboo)),
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.Obelisk)),
                    new UnlockLocation(globe.Biomes.SelectMany(x=>x.Nodes).Single(x=>x.Sid == GlobeNodeSid.Vines)),
                    new ActivateStoryPoint(story2, globe)
                }
            };
            
            list.Add(story1);
            
            return list;
        }
    }

    public interface IStoryPointAftermath
    {
        void Apply(IStoryPointAftermathContext context);
    }
    
    internal sealed class UnlockLocation: IStoryPointAftermath
    {
        private readonly GlobeNode _location;

        public UnlockLocation(GlobeNode location)
        {
            _location = location;
        }

        public void Apply(IStoryPointAftermathContext context)
        {
            _location.IsAvailable = true;
        }
    }

    internal sealed class ActivateStoryPoint: IStoryPointAftermath
    {
        private readonly IStoryPoint _newStoryPoint;
        private readonly Globe _globe;

        public ActivateStoryPoint(IStoryPoint newStoryPoint, Globe globe)
        {
            _newStoryPoint = newStoryPoint;
            _globe = globe;
        }

        public void Apply(IStoryPointAftermathContext context)
        {
            _globe.ActiveStoryPoints = _globe.ActiveStoryPoints.Concat(new[] { _newStoryPoint }).ToArray();
        }
    }

    public interface IStoryPointAftermathContext
    {
        
    }

    public sealed class StoryPointAftermathContext : IStoryPointAftermathContext
    {
        
    }

    /// <summary>
    /// Интерфейс для сущностей, выполнение которых зависит от работ (перки, квесты).
    /// </summary>
    public interface IJobExecutable
    {
        IReadOnlyCollection<IJob>? CurrentJobs { get; }

        void HandleCompletion();
    }

    /// <summary>
    /// Интерфейс произвольной работы и её текущего состояния.
    /// </summary>
    public interface IJob
    {
        /// <summary>
        /// Признак того, что работа завершена.
        /// </summary>
        bool IsComplete { get; set; }

        /// <summary>
        /// Текущий прогресс по работе.
        /// </summary>
        int Progress { get; set; }

        /// <summary>
        /// Схема текущей работы.
        /// </summary>
        IJobSubScheme Scheme { get; }
    }
    
    internal sealed class Job: IJob
    {
        public bool IsComplete { get; set; }
        public int Progress { get; set; }
        public IJobSubScheme Scheme { get; set; }
    }

    public interface IJobProgressResolver
    {
        /// <summary>
        /// Применяет прогресс к текущим работам.
        /// </summary>
        /// <param name="progress"> Объект прогресса. </param>
        /// <param name="evolutionData"> Данные о развитии персонажа. </param>
        /// <returns> Возвращает true, если все работы выполнены. </returns>
        void ApplyProgress(IJobProgress progress, IJobExecutable target);
    }
    
    public sealed class JobProgressResolver: IJobProgressResolver
    {
        public void ApplyProgress(IJobProgress progress, IJobExecutable target)
        {
            if (target.CurrentJobs is null)
            {
                // Перки у которых нет работ, не могут развиваться.

                // Некоторые перки (например врождённые таланты), не прокачиваются.
                // Сразу игнорируем их.
                return;
            }

            var affectedJobs = progress.ApplyToJobs(target.CurrentJobs);

            foreach (var job in affectedJobs)
            {
                // Опеределяем, какие из прогрессировавших работ завершены.
                // И фиксируем их состояние завершения.
                if (job.Progress >= job.Scheme.Value)
                {
                    job.IsComplete = true;
                }
            }
            
            // Опеределяем, все ли работы выполнены.
            var allJobsAreComplete = target.CurrentJobs.All(x => x.IsComplete);

            if (allJobsAreComplete)
            {
                target.HandleCompletion();
            }
        }
    }

    /// <summary>
    /// Интерфейс для прогресса по работе.
    /// </summary>
    /// <remarks>
    /// Используется, чтобы зафиксировать выполнение работы или её части.
    /// </remarks>
    public interface IJobProgress
    {
        /// <summary>
        /// Применяет прогресс к указанным работам.
        /// </summary>
        /// <param name="currentJobs"> Текущий набор работ, к которым необходимо применить прогресс. </param>
        /// <returns> Возвращает набор работ, которые были изменены. </returns>
        IEnumerable<IJob> ApplyToJobs(IEnumerable<IJob> currentJobs);
    }
    
    internal sealed class CombatCompleteJobProgress : IJobProgress
    {
        public IEnumerable<IJob> ApplyToJobs(IEnumerable<IJob> currentJobs)
        {
            if (currentJobs is null)
            {
                throw new ArgumentNullException(nameof(currentJobs));
            }

            var modifiedJobs = new List<IJob>();
            foreach (var job in currentJobs)
            {
                if (job.Scheme.Type != JobType.Combats)
                {
                    continue;
                }

                ProcessJob(job, modifiedJobs);
            }

            return modifiedJobs.ToArray();
        }

        private static void ProcessJob(IJob job, ICollection<IJob> modifiedJobs)
        {
            job.Progress++;
            modifiedJobs.Add(job);
        }
    }

    public interface IJobSubScheme
    {
        string[]? Data { get; }
        JobScope Scope { get; }
        JobType Type { get; }
        int Value { get; }
    }

    internal sealed class JobScheme: IJobSubScheme
    {
        public string[]? Data { get; set; }
        public JobScope Scope { get; set; }
        public JobType Type { get; set; }
        public int Value { get; set; }
    }

    /// <summary>
    /// Область действия работы.
    /// </summary>
    public enum JobScope
    {
        /// <summary>
        /// Общая область действия.
        /// Прогресс не будет сбрасываться после окончания боя.
        /// Не влияет на одноходовые задачи (одним ударом убить 2 противника)
        /// </summary>
        Global,

        /// <summary>
        /// Область действия на высадку.
        /// Если работы в рамках одной высадки не выполнены, то прогресс будет сброшен.
        /// </summary>
        Scenario,

        /// <summary>
        /// Область действия на этап высадки.
        /// Этап высадки это от отдыха до отдыха (ключевые точки).
        /// </summary>
        KeyPoint
    }
    
    public enum JobType
    {
        Undefined = 0,
        Defeats = 1, // Просто повергнуть кого-нибудь. Все победы - это нанесение крита, дающего OutOfControl
        DefeatGaarns, // повергнуть представителя народа Гаарн
        DefeatAleberts, // повергнуть алеберта
        DefeatLegions, // повергнуть легионера
        DefeatDeamons, // повергнуть демона-наемника
        DefeatCults, // повергнуть технокультиста
        DefeatTechbots, // повергнуть робота-самурая
        Blocks, // Выдержать удар
        Hits, // Попасть
        Crits, // Получить крит
        Combats, // Поучавствовать в боях
        Victories, // Победить
        ReceiveHits,
        ReceiveDamage,
        ReceiveDamagePercent,
        CraftAssaultRifle,
        MeleeHits, // Попасть в рукопашном бою
        OneUseHits, // задамажить одним использованием скилла
        OneUseDefeats, // уничтожить одним использованием скилла
        DefeatClasses, // Повергнуть персонажей указанных поколений,
        Craft,

        /// <summary>
        /// Поглотить провиант.
        /// </summary>
        ConsumeProviant,

        /// <summary>
        /// Атаковать актёра.
        /// </summary>
        /// <remarks>
        /// Атаковать - значит использовать действие с типом Attack, чтобы целью был другой актёр.
        /// Эти работы могут содержать данные, в которых указаны подробности, кого и как нужно атаковать.
        /// Например, атаковать монстра с тегом beast. Или атаковать с использованием оружия с тегом sword.
        /// Если данные не указаны, то засчитывается любая атака.
        /// </remarks>
        AttacksActor
    }
}