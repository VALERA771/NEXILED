// -----------------------------------------------------------------------
// <copyright file="CustomUnit.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.CustomRoles.API.Features
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.API.Features.Spawn;
    using Exiled.CustomItems.API.Features;
    using MEC;
    using PlayerRoles;
    using Respawning;
    using YamlDotNet.Serialization;

    /// <summary>
    /// A generic class for all custom units.
    /// </summary>
    public abstract class CustomUnit
    {
        private static Dictionary<Type, CustomUnit?> typeLookupTable = new();

        private static Dictionary<string, CustomUnit?> stringLookupTable = new();

        private static Dictionary<uint, CustomUnit?> idLookupTable = new();

        /// <summary>
        /// Gets a list of all registered custom roles.
        /// </summary>
        public static HashSet<CustomUnit> Registered { get; } = new();

        /// <summary>
        /// Gets or sets the custom RoleID of the role.
        /// </summary>
        public abstract uint Id { get; set; }

        /// <summary>
        /// Gets or sets the name of this role.
        /// </summary>
        public abstract string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of this role.
        /// </summary>
        public abstract string Description { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="RoleTypeId"/> to spawn this role as.
        /// </summary>
        public abstract List<UnitRole> Roles { get; set; }

        /// <summary>
        /// Gets all of the players currently set to this role.
        /// </summary>
        [YamlIgnore]
        public HashSet<Player> TrackedPlayers { get; } = new();

        /// <summary>
        /// Gets or sets the starting inventory for the role.
        /// </summary>
        public virtual List<string> Inventory { get; set; } = new();

        /// <summary>
        /// Gets or sets the starting ammo for the role.
        /// </summary>
        public virtual Dictionary<AmmoType, ushort> Ammo { get; set; } = new();

        /// <summary>
        /// Gets or sets the possible spawn locations for this role.
        /// </summary>
        public virtual SpawnProperties? SpawnProperties { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether players keep their current position when gaining this role.
        /// </summary>
        public virtual bool KeepPositionOnSpawn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether players keep their current inventory when gaining this role.
        /// </summary>
        public virtual bool KeepInventoryOnSpawn { get; set; }

        /// <summary>
        /// Gets or sets an announcement that will be played when unit is spawned.
        /// </summary>
        public virtual string? CassieAnnouncement { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="Dictionary{TKey, TValue}"/> containing cached <see cref="string"/> and their  <see cref="Dictionary{TKey, TValue}"/> which is cached Role with FF multiplier.
        /// </summary>
        public virtual Dictionary<RoleTypeId, float> CustomUnitFFMultiplier { get; set; } = new();

        /// <summary>
        /// Gets or sets a minimum amount of players that will be spawned.
        /// </summary>
        public virtual int MinimumToSpawn { get; set; } = 1;

        /// <summary>
        /// Gets or sets a maximum amount of players that will be spawned.
        /// </summary>
        public virtual int MaximumToSpawn { get; set; } = 10;

        /// <summary>
        /// Gets a <see cref="CustomUnit"/> by ID.
        /// </summary>
        /// <param name="id">The ID of the role to get.</param>
        /// <returns>The role, or <see langword="null"/> if it doesn't exist.</returns>
        public static CustomUnit? Get(uint id)
        {
            if (!idLookupTable.ContainsKey(id))
                idLookupTable.Add(id, Registered?.FirstOrDefault(r => r.Id == id));
            return idLookupTable[id];
        }

        /// <summary>
        /// Gets a <see cref="CustomUnit"/> by type.
        /// </summary>
        /// <param name="t">The <see cref="Type"/> to get.</param>
        /// <returns>The role, or <see langword="null"/> if it doesn't exist.</returns>
        public static CustomUnit? Get(Type t)
        {
            if (!typeLookupTable.ContainsKey(t))
                typeLookupTable.Add(t, Registered?.FirstOrDefault(r => r.GetType() == t));
            return typeLookupTable[t];
        }

        /// <summary>
        /// Gets a <see cref="CustomUnit"/> by name.
        /// </summary>
        /// <param name="name">The name of the role to get.</param>
        /// <returns>The role, or <see langword="null"/> if it doesn't exist.</returns>
        public static CustomUnit? Get(string name)
        {
            if (!stringLookupTable.ContainsKey(name))
                stringLookupTable.Add(name, Registered?.FirstOrDefault(r => r.Name == name));
            return stringLookupTable[name];
        }

        /// <summary>
        /// Tries to get a <see cref="CustomUnit"/> by <inheritdoc cref="Id"/>.
        /// </summary>
        /// <param name="id">The ID of the role to get.</param>
        /// <param name="customUnit">The custom role.</param>
        /// <returns>True if the role exists.</returns>
        public static bool TryGet(uint id, out CustomUnit? customUnit)
        {
            customUnit = Get(id);

            return customUnit is not null;
        }

        /// <summary>
        /// Tries to get a <see cref="CustomUnit"/> by name.
        /// </summary>
        /// <param name="name">The name of the role to get.</param>
        /// <param name="customUnit">The custom role.</param>
        /// <returns>True if the role exists.</returns>
        /// <exception cref="ArgumentNullException">If the name is <see langword="null"/> or an empty string.</exception>
        public static bool TryGet(string name, out CustomUnit? customUnit)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            customUnit = uint.TryParse(name, out uint id) ? Get(id) : Get(name);

            return customUnit is not null;
        }

        /// <summary>
        /// Tries to get a <see cref="CustomUnit"/> by name.
        /// </summary>
        /// <param name="t">The <see cref="Type"/> of the role to get.</param>
        /// <param name="customUnit">The custom role.</param>
        /// <returns>True if the role exists.</returns>
        /// <exception cref="ArgumentNullException">If the name is <see langword="null"/> or an empty string.</exception>
        public static bool TryGet(Type t, out CustomUnit? customUnit)
        {
            customUnit = Get(t);

            return customUnit is not null;
        }

        /// <summary>
        /// Forces a <see cref="CustomUnit"/> to spawn.
        /// </summary>
        public void Spawn()
        {
            IList<Player> playerToSpawn = Player.List.Where(x => RespawnManager.Singleton.CheckSpawnable(x.ReferenceHub)).ToArray();

            if (playerToSpawn.Count < MinimumToSpawn)
                return;

            playerToSpawn.ShuffleList();

            IList<UnitRole> roles = new UnitRole[] { };

            foreach (UnitRole role in Roles)
            {
                for (int i = 0; i < role.MaximumAmount; i++)
                    roles.Add(role);
            }

            for (int i = 0; i < Math.Min(MaximumToSpawn, playerToSpawn.Count); i++)
            {
                UnitRole role = roles.Any(x => x.MustSpawn) ? roles.GetRandomValue(x => x.MustSpawn) : roles.GetRandomValue();

                GrantRole(playerToSpawn[i], role);
                roles.Remove(role);

                TrackedPlayers.Add(playerToSpawn[i]);
            }

            if (CassieAnnouncement != null)
                Cassie.Message(CassieAnnouncement);
        }

        /// <summary>
        /// Grants a role to a player.
        /// </summary>
        /// <param name="player">Target player.</param>
        /// <param name="role">Role to grant.</param>
        public void GrantRole(Player player, UnitRole role)
        {
            if (role.CustomRole != null)
                role.CustomRole.AddRole(player);
            else if (role.RoleTypeId != RoleTypeId.None)
                player.Role.Set(role.RoleTypeId, SpawnReason.Respawn, RoleSpawnFlags.All);
            else
                throw new ArgumentException("Role must have a custom role or a role type.", nameof(role));

            Timing.CallDelayed(0.5f, () =>
            {
                if (SpawnProperties != null && !KeepPositionOnSpawn)
                {
                    SpawnPoint spawnPoint = SpawnProperties.GetRandomSpawnPoint();

                    player.Position = spawnPoint.Position;
                }

                if (!KeepInventoryOnSpawn)
                {
                    player.ClearAmmo();
                    player.ClearItems();

                    player.SetAmmo(Ammo);

                    foreach (string str in Inventory)
                    {
                        if (CustomItem.TryGet(str, out CustomItem? customItem) && customItem != null)
                        {
                            customItem.Give(player);
                            continue;
                        }

                        if (Enum.TryParse(str, out ItemType itemType))
                        {
                            player.AddItem(itemType);
                            continue;
                        }

                        Log.Warn($"{Name}: {str} is not a valid ItemType or Custom Item name.");
                    }
                }

                player.FriendlyFireMultiplier = CustomUnitFFMultiplier;

                OnGrantedRole(player, role);
            });
        }

        /// <summary>
        /// Fired when <see cref="GrantRole"/> method is finished.
        /// </summary>
        /// <param name="player">Player that received the role.</param>
        /// <param name="role">Role that was granted.</param>
        protected virtual void OnGrantedRole(Player player, UnitRole role)
        {
        }
    }
}