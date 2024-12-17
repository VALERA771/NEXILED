// -----------------------------------------------------------------------
// <copyright file="HumanKillObjective.cs" company="ExMod Team">
// Copyright (c) ExMod Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Objectives
{
    using Exiled.API.Interfaces;
    using PlayerRoles;
    using Respawning.Objectives;

    using BaseObjective = Respawning.Objectives.HumanKillObjective;

    /// <summary>
    /// A wrapper for Human kill objective.
    /// </summary>
    public class HumanKillObjective : HumanObjective<KillObjectiveFootprint>, IWrapper<BaseObjective>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HumanKillObjective"/> class.
        /// </summary>
        /// <param name="objectiveFootprintBase"><inheritdoc cref="Base"/></param>
        internal HumanKillObjective(BaseObjective objectiveFootprintBase)
            : base(objectiveFootprintBase)
        {
            Base = objectiveFootprintBase;
        }

        /// <inheritdoc/>
        public new BaseObjective Base { get; }

        /// <summary>
        /// Checks if the role is an enemy role.
        /// </summary>
        /// <param name="target">Target role.</param>
        /// <param name="player">Attacker.</param>
        /// <returns><c>true</c> if role is an enemy role, <c>false</c> otherwise.</returns>
        public bool IsValidEnemy(RoleTypeId target, Player player) => Base.IsValidEnemy(target, player.ReferenceHub);

        /// <summary>
        /// Checks if the player is an enemy.
        /// </summary>
        /// <param name="target">Target player.</param>
        /// <param name="player">Attacker.</param>
        /// <returns><c>true</c> if player is an enemy, <c>false</c> otherwise.</returns>
        public bool IsValidEnemy(Player target, Player player) => IsValidEnemy(target.Role, player);
    }
}