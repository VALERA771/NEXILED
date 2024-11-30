// -----------------------------------------------------------------------
// <copyright file="DropdownSetting.cs" company="ExMod Team">
// Copyright (c) ExMod Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Core.UserSettings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::UserSettings.ServerSpecific;

    /// <summary>
    /// A class.
    /// </summary>
    public class DropdownSetting : SettingBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DropdownSetting"/> class.
        /// </summary>
        /// <param name="settingBase">A <see cref="SSDropdownSetting"/> instance.</param>
        internal DropdownSetting(SSDropdownSetting settingBase)
            : base(settingBase)
        {
            Base = settingBase;
        }

        /// <summary>
        /// Gets a.
        /// </summary>
        public new SSDropdownSetting Base { get; }

        /// <summary>
        /// Gets or sets a collection of all options in dropdown.
        /// </summary>
        public IEnumerable<string> Options
        {
            get => Base.Options;
            set => Base.Options = value.ToArray();
        }

        /// <summary>
        /// Gets or sets an index of default option.
        /// </summary>
        public int DefaultOptionIndex
        {
            get => Base.DefaultOptionIndex;
            set => Base.DefaultOptionIndex = value;
        }

        /// <summary>
        /// Gets or sets a default option.
        /// </summary>
        public string DefaultOption
        {
            get => Base.Options[DefaultOptionIndex];
            set => DefaultOptionIndex = Array.IndexOf(Base.Options, value);
        }

        /// <summary>
        /// Gets or sets a type of dropdown.
        /// </summary>
        public SSDropdownSetting.DropdownEntryType DropdownType
        {
            get => Base.EntryType;
            set => Base.EntryType = value;
        }
    }
}