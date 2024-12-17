// -----------------------------------------------------------------------
// <copyright file="Objective.cs" company="ExMod Team">
// Copyright (c) ExMod Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Respawning;

namespace Exiled.API.Features.Objectives
{
    using Exiled.API.Enums;
    using Exiled.API.Features.Core;
    using Exiled.API.Interfaces;
    using PlayerRoles;
    using Respawning.Objectives;

    using BaseScpPickupObjective = Respawning.Objectives.ScpItemPickupObjective;
    using BaseHumanDamageObjective = Respawning.Objectives.HumanDamageObjective;
    using BaseHumanKillObjective = Respawning.Objectives.HumanKillObjective;

    /// <summary>
    /// A wrapper for Faction objective.
    /// </summary>
    public class Objective : TypeCastObject<FactionObjectiveBase>, IWrapper<FactionObjectiveBase>
    {
        internal static Dictionary<ObjectiveType, Objective> Objectibes = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Objective"/> class.
        /// </summary>
        /// <param name="objectiveFootprintBase"><inheritdoc cref="Base"/></param>
        internal Objective(FactionObjectiveBase objectiveFootprintBase)
        {
            Base = objectiveFootprintBase;
        }

        /// <inheritdoc/>
        public FactionObjectiveBase Base { get; }

        /// <summary>
        /// Gets the type of objective.
        /// </summary>
        public virtual ObjectiveType Type { get; } = ObjectiveType.None;

        public static Objective Get(ObjectiveType type) => Objectibes.TryGetValue(type, out Objective objective)
            ? objective
            : type switch
            {
                ObjectiveType.ScpItemPickup => new ScpItemPickupObjective(FactionInfluenceManager.Objectives.OfType<BaseScpPickupObjective>().First()),
                ObjectiveType.GeneratorActivation => new GeneratorActivationObjective(FactionInfluenceManager.Objectives.OfType<GeneratorActivatedObjective>().First()),
                ObjectiveType.HumanDamage => new HumanDamageObjective(FactionInfluenceManager.Objectives.OfType<BaseHumanDamageObjective>().First()),
                ObjectiveType.HumanKill => new HumanKillObjective(FactionInfluenceManager.Objectives.OfType<BaseHumanKillObjective>().First()),
                _ => null
            };

        /// <summary>
        /// Reduces timer for faction.
        /// </summary>
        /// <param name="faction">Faction to affect.</param>
        /// <param name="seconds">Time to reduce in seconds.</param>
        public void ReduceTimer(Faction faction, float seconds) => Base.ReduceTimer(faction, seconds);

        /// <summary>
        /// Grants influence to faction.
        /// </summary>
        /// <param name="faction">Faction to affect.</param>
        /// <param name="amount">Amount of influence to grant.</param>
        public void GrantInfluence(Faction faction, float amount) => Base.GrantInfluence(faction, amount);

        /// <summary>
        /// Achieves objective.
        /// </summary>
        public void Achieve() => Base.ServerSendUpdate();

        /// <summary>
        /// Checks if faction has this objective.
        /// </summary>
        /// <param name="faction">Faction to check.</param>
        /// <returns><c>true</c> if faction has this objective, <c>false</c> otherwise.</returns>
        public bool IsValidFaction(Faction faction) => Base.IsValidFaction(faction);

        /// <summary>
        /// Checks if player has this objective.
        /// </summary>
        /// <param name="player">Player to check.</param>
        /// <returns><c>true</c> if player has this objective, <c>false</c> otherwise.</returns>
        public bool IsValidFaction(Player player) => Base.IsValidFaction(player.ReferenceHub);
    }
}