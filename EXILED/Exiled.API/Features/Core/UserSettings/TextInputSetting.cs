// -----------------------------------------------------------------------
// <copyright file="TextInputSetting.cs" company="ExMod Team">
// Copyright (c) ExMod Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.API.Features.Core.UserSettings
{
    using System;

    using Exiled.API.Interfaces;
    using global::UserSettings.ServerSpecific;
    using TMPro;

    /// <summary>
    /// Represents a text input setting.
    /// </summary>
    public class TextInputSetting : SettingBase, IWrapper<SSPlaintextSetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputSetting"/> class.
        /// </summary>
        /// <param name="id"><inheritdoc cref="SettingBase.Id"/></param>
        /// <param name="label"><inheritdoc cref="SettingBase.Label"/></param>
        /// <param name="placeholder"><inheritdoc cref="Placeholder"/></param>
        /// <param name="characterLimit"><inheritdoc cref="CharacterLimit"/></param>
        /// <param name="contentType"><inheritdoc cref="ContentType"/></param>
        /// <param name="hint"><inheritdoc cref="SettingBase.HintDescription"/></param>
        /// <param name="header"><inheritdoc cref="SettingBase.Header"/></param>
        /// <param name="onChanged"><inheritdoc cref="SettingBase.OnChanged"/></param>
        public TextInputSetting(
            int id,
            string label,
            string placeholder = "...",
            int characterLimit = 64,
            TMP_InputField.ContentType contentType = TMP_InputField.ContentType.Standard,
            string hint = null,
            HeaderSetting header = null,
            Action<Player, SettingBase> onChanged = null)
            : base(new SSPlaintextSetting(id, label, placeholder, characterLimit, contentType, hint), header, onChanged)
        {
            Base = (SSPlaintextSetting)base.Base;

            if (OriginalDefinition != null && OriginalDefinition.Is(out TextInputSetting textInputSetting))
            {
                Placeholder = textInputSetting.Placeholder;
                ContentType = textInputSetting.ContentType;
                CharacterLimit = textInputSetting.CharacterLimit;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextInputSetting"/> class.
        /// </summary>
        /// <param name="settingBase">A <see cref=""/></param>
        internal TextInputSetting(SSPlaintextSetting settingBase)
            : base(settingBase)
        {
            Base = settingBase;
        }

        /// <inheritdoc />
        public new SSPlaintextSetting Base { get; }

        /// <summary>
        /// Gets or sets the input text.
        /// </summary>
        public string InputText
        {
            get => Base.SyncInputText;
            set => Base.SyncInputText = value;
        }

        /// <summary>
        /// Gets or sets the placeholder.
        /// </summary>
        public string Placeholder
        {
            get => Base.Placeholder;
            set => Base.Placeholder = value;
        }

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public TMP_InputField.ContentType ContentType
        {
            get => Base.ContentType;
            set => Base.ContentType = value;
        }

        /// <summary>
        /// Gets or sets the character limit.
        /// </summary>
        public int CharacterLimit
        {
            get => Base.CharacterLimit;
            set => Base.CharacterLimit = value;
        }

        /// <summary>
        /// Gets a string representation of the setting.
        /// </summary>
        /// <returns>A string in human readable format.</returns>
        public override string ToString()
        {
            return base.ToString() + $"/{InputText}/ *{Placeholder}* '{CharacterLimit}'";
        }
    }
}