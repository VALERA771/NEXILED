// -----------------------------------------------------------------------
// <copyright file="PrismaticCloudHazard.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Hazards
{
    using System.Collections.Generic;
    using System.Linq;

    using global::Hazards;

    /// <summary>
    /// Represents a Prismatic Cloud hazard.
    /// </summary>
    public class PrismaticCloudHazard : TemporaryHazard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrismaticCloudHazard"/> class.
        /// </summary>
        /// <param name="hazard">A <see cref="PrismaticCloud"/> instance.</param>
        public PrismaticCloudHazard(PrismaticCloud hazard)
            : base(hazard)
        {
            Base = hazard;
        }

        /// <summary>
        /// Gets the <see cref="PrismaticCloud"/>.
        /// </summary>
        public new PrismaticCloud Base { get; }

        /// <summary>
        /// Gets or sets a list of players that will be ignored by the hazard.
        /// </summary>
        public IEnumerable<Player> IgnoredPlayers
        {
            get => Base.IgnoredTargets.Select(Player.Get);
            set => Base.IgnoredTargets = value.Select(x => x.ReferenceHub).ToList();
        }

        /// <summary>
        /// Enables hazard's effects for target.
        /// </summary>
        /// <param name="target">Target to affect.</param>
        public void EnableEffect(Player target) => Base.ServerEnableEffect(target.ReferenceHub);
    }
}