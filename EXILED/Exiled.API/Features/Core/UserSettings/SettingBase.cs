// -----------------------------------------------------------------------
// <copyright file="SettingBase.cs" company="ExMod Team">
// Copyright (c) ExMod Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Core.UserSettings
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using global::UserSettings.ServerSpecific;

    /// <summary>
    /// A base class for all Server Specific Settings.
    /// </summary>
    public class SettingBase
    {
        /// <summary>
        /// A <see cref="Dictionary{TKey,TValue}"/> that contains <see cref="SettingBase"/> that were received by a players.
        /// </summary>
        private static Dictionary<Player, List<SettingBase>> receivedSettings = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingBase"/> class.
        /// </summary>
        /// <param name="settingBase">A <see cref="ServerSpecificSettingBase"/> instance.</param>
        internal SettingBase(ServerSpecificSettingBase settingBase)
        {
            Base = settingBase;
        }

        /// <summary>
        /// Gets the list of all settings.
        /// </summary>
        /// <remarks>This list contains only synced settings.</remarks>
        /// <seealso cref="Sync"/>
        public static IReadOnlyDictionary<Player, ReadOnlyCollection<SettingBase>> List
            => new ReadOnlyDictionary<Player, ReadOnlyCollection<SettingBase>>(receivedSettings.ToDictionary(x => x.Key, x => x.Value.AsReadOnly()));

        /// <summary>
        /// Gets or sets the predicate for syncing this setting when a player joins.
        /// </summary>
        public static Predicate<Player> SyncOnJoin { get; set; }

        /// <summary>
        /// Gets the base class for this setting.
        /// </summary>
        public ServerSpecificSettingBase Base { get; }

        /// <summary>
        /// Gets or sets the id of this setting.
        /// </summary>
        public int Id
        {
            get => Base.SettingId;
            set => Base.SetId(value, string.Empty);
        }

        /// <summary>
        /// Gets or sets the label of this setting.
        /// </summary>
        public string Label
        {
            get => Base.Label;
            set => Base.Label = value;
        }

        /// <summary>
        /// Gets or sets the description of this setting.
        /// </summary>
        public string HintDescription
        {
            get => Base.HintDescription;
            set => Base.HintDescription = value;
        }

        /// <summary>
        /// Gets the response mode of this setting.
        /// </summary>
        public ServerSpecificSettingBase.UserResponseMode ResponseMode => Base.ResponseMode;

        /// <summary>
        /// Tries ti get the setting with the specified id.
        /// </summary>
        /// <param name="player">Player who has received the setting.</param>
        /// <param name="id">Id of the setting.</param>
        /// <param name="setting">A <see cref="SettingBase"/> instance if found. Otherwise, <c>null</c>.</param>
        /// <typeparam name="T">Type of the setting.</typeparam>
        /// <returns><c>true</c> if the setting was found, <c>false</c> otherwise.</returns>
        public static bool TryGetSetting<T>(Player player, int id, out T setting)
            where T : SettingBase
        {
            setting = null;

            if (!receivedSettings.TryGetValue(player, out List<SettingBase> list))
                return false;

            setting = (T)list.FirstOrDefault(x => x.Id == id);
            return setting != null;
        }

        /// <summary>
        /// Tries ti get the setting with the specified id.
        /// </summary>
        /// <param name="player">Player who has received the setting.</param>
        /// <param name="id">Id of the setting.</param>
        /// <param name="setting">A <see cref="SettingBase"/> instance if found. Otherwise, <c>null</c>.</param>
        /// <returns><c>true</c> if the setting was found, <c>false</c> otherwise.</returns>
        public static bool TryGetSetting(Player player, int id, out SettingBase setting) => TryGetSetting<SettingBase>(player, id, out setting);

        /// <summary>
        /// Creates a new instance of this setting.
        /// </summary>
        /// <param name="settingBase">A <see cref="ServerSpecificSettingBase"/> instance.</param>
        /// <returns>A new instance of this setting.</returns>
        public static SettingBase Create(ServerSpecificSettingBase settingBase) => settingBase switch
        {
            SSTwoButtonsSetting twoButtonsSetting => new TwoButtonsSetting(twoButtonsSetting),
            _ => new SettingBase(settingBase)
        };

        /// <summary>
        /// Creates a new instance of this setting.
        /// </summary>
        /// <param name="settingBase">A<see cref="ServerSpecificSettingBase"/> instance.</param>
        /// <typeparam name="T">Type of the setting.</typeparam>
        /// <returns>A new instance of this setting.</returns>
        public static T Create<T>(ServerSpecificSettingBase settingBase)
            where T : SettingBase => (T)Create(settingBase);

        /// <summary>
        /// Syncs setting with all players.
        /// </summary>
        public static void SendToAll() => ServerSpecificSettingsSync.SendToAll();

        /// <summary>
        /// Syncs setting with all players according to the specified predicate.
        /// </summary>
        /// <param name="predicate">A requirement to meet.</param>
        public static void SendToAll(Func<Player, bool> predicate)
        {
            foreach (Player player in Player.List)
            {
                if (predicate(player))
                    SendToPlayer(player);
            }
        }

        /// <summary>
        /// Syncs setting with the specified target.
        /// </summary>
        /// <param name="player">Target player.</param>
        public static void SendToPlayer(Player player) => ServerSpecificSettingsSync.SendToPlayer(player.ReferenceHub);

        /// <summary>
        /// Syncs this setting with all players.
        /// </summary>
        public void Sync()
        {
            List<ServerSpecificSettingBase> newList =
                new((ServerSpecificSettingsSync.DefinedSettings ?? Array.Empty<ServerSpecificSettingBase>()).Where(x => x.SettingId != Id)) { Base };
            ServerSpecificSettingsSync.DefinedSettings = newList.ToArray();
            SendToAll();
        }

        /// <summary>
        /// Internal method that fires when a setting is updated.
        /// </summary>
        /// <param name="hub"><see cref="ReferenceHub"/> that has updates the setting.</param>
        /// <param name="settingBase">A new updated setting.</param>
        internal static void OnSettingUpdated(ReferenceHub hub, ServerSpecificSettingBase settingBase)
        {
            if (!Player.TryGet(hub, out Player player))
                return;

            SettingBase setting;

            if (!receivedSettings.TryGetValue(player, out List<SettingBase> list))
            {
                setting = Create(settingBase);
                receivedSettings.Add(player, new() { setting });
            }
            else if (!list.Exists(x => x.Id == settingBase.SettingId))
            {
                setting = Create(settingBase);
                list.Add(setting);
            }
            else
            {
                setting = list.Find(x => x.Id == settingBase.SettingId);
            }

            setting.OnUpdated(player);
        }

        /// <summary>
        /// Creates a clone of this setting.
        /// </summary>
        /// <returns>A new <see cref="SettingBase"/> instance with same data.</returns>
        internal virtual SettingBase Clone()
        {
            return null;
        }

        /// <summary>
        /// Fires when a setting is updated.
        /// </summary>
        /// <param name="player">Target who has updated the setting.</param>
        protected virtual void OnUpdated(Player player)
        {
        }
    }
}