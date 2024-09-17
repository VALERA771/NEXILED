// -----------------------------------------------------------------------
// <copyright file="CustomKeycard.cs" company="Exiled Team">
// Copyright (c) Exiled Team. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Exiled.CustomItems.API.Features
{
    using Exiled.API.Enums;
    using Exiled.API.Extensions;
    using Exiled.API.Features;
    using Exiled.API.Features.Doors;
    using Exiled.Events.EventArgs.Item;
    using Exiled.Events.EventArgs.Player;
    using MapGeneration.Distributors;

    /// <summary>
    /// The Custom keycard base class.
    /// </summary>
    public abstract class CustomKeycard : CustomItem
    {
        /// <summary>
        /// Gets or sets the permissions for custom keycard.
        /// </summary>
        public virtual KeycardPermissions Permissions { get; set; }

        /// <summary>
        /// Called when custom keycard interacts with a door.
        /// </summary>
        /// <param name="player">Owner of Custom keycard.</param>
        /// <param name="door">Door with which interacting.</param>
        /// <param name="isAllowed">Indicates whether door can be opened.</param>
        protected virtual void OnInteractingDoor(Player player, Door door, ref bool isAllowed)
        {
        }

        /// <summary>
        /// Called when custom keycard interacts with a locker.
        /// </summary>
        /// <param name="player">Owner of Custom keycard.</param>
        /// <param name="chamber">Chamber with which interacting.</param>
        /// <param name="isAllowed">Indicates whether chamber can be opened.</param>
        protected virtual void OnInteractingLocker(Player player, LockerChamber chamber, ref bool isAllowed)
        {
        }

        private void OnInternalKeycardInteracting(KeycardInteractingEventArgs ev)
        {
            if (!Check(ev.Pickup))
                return;

            if (!ev.Door.IsKeycardDoor || ev.Door.KeycardPermissions == KeycardPermissions.None || ev.Door.IsLocked || ev.Player.IsBypassModeEnabled)
                return;

            if (ev.Door.KeycardPermissions.HasFlagFast(KeycardPermissions.ScpOverride))
            {
                ev.IsAllowed = Permissions.HasFlagFast(KeycardPermissions.Checkpoints);
                return;
            }

            bool isAllowed = Permissions.HasFlagFast(ev.Door.KeycardPermissions);

            OnInteractingDoor(ev.Player, ev.Door, ref isAllowed);

            ev.IsAllowed = isAllowed;
        }

        private void OnInternalInteractingDoor(InteractingDoorEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;

            if (ev.Door.KeycardPermissions == KeycardPermissions.None || ev.Door.IsLocked || ev.Player.IsBypassModeEnabled)
                return;

            if (ev.Door.KeycardPermissions.HasFlagFast(KeycardPermissions.ScpOverride))
            {
                ev.IsAllowed = Permissions.HasFlagFast(KeycardPermissions.Checkpoints);
                return;
            }

            bool isAllowed = Permissions.HasFlagFast(ev.Door.KeycardPermissions);

            OnInteractingDoor(ev.Player, ev.Door, ref isAllowed);

            ev.IsAllowed = isAllowed;
        }

        private void OnInternalInteractingLocker(InteractingLockerEventArgs ev)
        {
            if (!Check(ev.Player.CurrentItem))
                return;

            if ((KeycardPermissions)ev.Chamber.RequiredPermissions == KeycardPermissions.None || ev.Player.IsBypassModeEnabled)
                return;

            bool isAllowed = Permissions.HasFlagFast((KeycardPermissions)ev.Chamber.RequiredPermissions);

            OnInteractingLocker(ev.Player, ev.Chamber, ref isAllowed);

            ev.IsAllowed = isAllowed;
        }
    }
}