// -----------------------------------------------------------------------
// <copyright file="SettingBase.cs" company="ExMod Team">
// Copyright (c) ExMod Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Exiled.API.Features.Core.UserSettings
{
    using global::UserSettings.ServerSpecific;

    public class SettingBase
    {
        internal static Dictionary<ServerSpecificSettingBase, SettingBase> Settings = new();

        public SettingBase(ServerSpecificSettingBase settingBase)
        {
            Base = settingBase;

            Settings.Add(settingBase, this);
        }

        public ServerSpecificSettingBase Base { get; }

        public int Id
        {
            get => Base.SettingId;
            set => Base.SetId(value, string.Empty);
        }

        public string Label
        {
            get => Base.Label;
            set => Base.Label = value;
        }

        public string HintDescription
        {
            get => Base.HintDescription;
            set => Base.HintDescription = value;
        }

        public ServerSpecificSettingBase.UserResponseMode ResponseMode => Base.ResponseMode;

        public static bool TryGetSetting<T>(Player player, int id, out T setting)
            where T : SettingBase
                => (setting = ServerSpecificSettingsSync.TryGetSettingOfUser(player.ReferenceHub, id, out ServerSpecificSettingBase result) && Settings.TryGetValue(result, out SettingBase settingBase)
                    ? (T)settingBase : null) != null;
    }
}