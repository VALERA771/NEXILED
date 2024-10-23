// -----------------------------------------------------------------------
// <copyright file="PrismaticCloudHazard.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Hazards
{
    using System.Collections.Generic;

    using Exiled.API.Enums;
    using global::Hazards;
    using Mirror;
    using RelativePositioning;
    using UnityEngine;

    /// <summary>
    /// A wrapper for <see cref="PrismaticCloud"/>.
    /// </summary>
    public class PrismaticCloudHazard : TemporaryHazard
    {
        private static PrismaticCloud prismaticCloud;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrismaticCloudHazard"/> class.
        /// </summary>
        /// <param name="hazard">The <see cref="TantrumEnvironmentalHazard"/> instance.</param>
        public PrismaticCloudHazard(PrismaticCloud hazard)
            : base(hazard)
        {
            Base = hazard;
        }

        /// <summary>
        /// Gets the prismatic prefab.
        /// </summary>
        public static PrismaticCloud PrismaticCloudPrefab
        {
            get
            {
                if (prismaticCloud == null)
                    prismaticCloud = PrefabHelper.GetPrefab<PrismaticCloud>(PrefabType.TantrumObj);

                return prismaticCloud;
            }
        }

        /// <summary>
        /// Gets the <see cref="PrismaticCloud"/>.
        /// </summary>
        public new PrismaticCloud Base { get; }

        /// <inheritdoc />
        public override HazardType Type => HazardType.Tantrum;

        /// <summary>
        /// Gets .
        /// </summary>
        public float DecaySpeed => Base.DecaySpeed;

        /// <summary>
        /// Gets .
        /// </summary>
        public float ExplodeDistance => Base._explodeDistance;

        /// <summary>
        /// Gets .
        /// </summary>
        public List<ReferenceHub> IgnoredTargets => Base.IgnoredTargets;

        /// <summary>
        /// Gets or sets the synced position.
        /// </summary>
        public RelativePosition SynchronisedPosition
        {
            get => Base.SynchronizedPosition;
            set => Base.SynchronizedPosition = value;
        }

        /// <summary>
        /// Gets or sets the correct position of tantrum hazard.
        /// </summary>
        public Transform CorrectPosition
        {
            get => Base._correctPosition;
            set => Base._correctPosition = value;
        }

        /// <summary>
        /// Places a Prismatic (Halloween's ability) in the indicated position.
        /// </summary>
        /// <param name="position">The position where you want to spawn the Tantrum.</param>
        /// <param name="isActive">Whether or not the tantrum will apply the <see cref="EffectType.Prismatic"/> effect.</param>
        /// <remarks>If <paramref name="isActive"/> is <see langword="true"/>, the tantrum is moved slightly up from its original position. Otherwise, the collision will not be detected and the slowness will not work.</remarks>
        /// <returns>The <see cref="TantrumHazard"/> instance.</returns>
        public static PrismaticCloudHazard PlaceTantrum(Vector3 position, bool isActive = true)
        {
            PrismaticCloud prismatic = Object.Instantiate(PrismaticCloudPrefab);

            if (!isActive)
                prismatic.SynchronizedPosition = new(position);
            else
                prismatic.SynchronizedPosition = new(position + (Vector3.up * 0.25f));

            prismatic._destroyed = !isActive;

            NetworkServer.Spawn(prismatic.gameObject);

            return Get<PrismaticCloudHazard>(prismatic);
        }
    }
}