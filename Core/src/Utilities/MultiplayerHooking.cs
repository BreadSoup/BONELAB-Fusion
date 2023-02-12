﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BoneLib.BoneMenu.Elements;

using LabFusion.Network;
using LabFusion.Representation;
using LabFusion.Senders;

namespace LabFusion.Utilities {
    public delegate bool UserAccessEvent(ulong userId, out string reason);
    public delegate void ServerEvent();
    public delegate void UpdateEvent();
    public delegate void PlayerUpdate(PlayerId playerId);
    public delegate void PlayerAction(PlayerId playerId, PlayerActionType type, PlayerId otherPlayer = null);
    public delegate void CatchupAction(ulong longId);
    public delegate void LobbyMenuAction(MenuCategory category, INetworkLobby lobby);

    /// <summary>
    /// Hooks for getting events from the server, players, etc.
    /// <para> All hooks are events. You cannot invoke them yourself. </para>
    /// </summary>
    public static class MultiplayerHooking {
        // Confirmation hooks
        public static event UserAccessEvent OnShouldAllowConnection;

        // Server hooks
        public static event ServerEvent OnStartServer, OnJoinServer, OnDisconnect;
        public static event PlayerUpdate OnPlayerJoin, OnPlayerLeave;
        public static event PlayerAction OnPlayerAction;
        public static event CatchupAction OnPlayerCatchup;
        public static event LobbyMenuAction OnLobbyCategoryCreated;

        internal static bool Internal_OnShouldAllowConnection(ulong userId, out string reason) {
            reason = "";

            if (OnShouldAllowConnection == null)
                return true;

            foreach (var invocation in OnShouldAllowConnection.GetInvocationList()) {
                var accessEvent = (UserAccessEvent)invocation;

                if (!accessEvent.Invoke(userId, out reason))
                    return false;
            }

            return true;
        }

        internal static void Internal_OnStartServer() => OnStartServer.InvokeSafe("executing OnStartServer hook");

        internal static void Internal_OnJoinServer() => OnJoinServer.InvokeSafe("executing OnJoinServer hook");

        internal static void Internal_OnDisconnect() => OnDisconnect.InvokeSafe("executing OnDisconnect hook");

        internal static void Internal_OnPlayerJoin(PlayerId id) => OnPlayerJoin.InvokeSafe(id, "executing OnPlayerJoin hook");

        internal static void Internal_OnPlayerLeave(PlayerId id) => OnPlayerLeave.InvokeSafe(id, "executing OnPlayerLeave hook");

        internal static void Internal_OnPlayerAction(PlayerId id, PlayerActionType type, PlayerId otherPlayer = null) => OnPlayerAction.InvokeSafe(id, type, otherPlayer, "executing OnPlayerAction hook");

        internal static void Internal_OnPlayerCatchup(ulong longId) => OnPlayerCatchup.InvokeSafe(longId, "executing OnPlayerCatchup hook");

        internal static void Internal_OnLobbyCategoryCreated(MenuCategory category, INetworkLobby lobby) => OnLobbyCategoryCreated.InvokeSafe(category, lobby, "executing OnLobbyCategoryCreated");

        // Settings updates
        public static event ServerEvent OnServerSettingsChanged;

        internal static void Internal_OnServerSettingsChanged() => OnServerSettingsChanged.InvokeSafe("executing server settings changed hook");

        // Unity hooks
        public static event UpdateEvent OnUpdate, OnFixedUpdate, OnLateUpdate,
            OnMainSceneInitialized;

        internal static void Internal_OnUpdate() => OnUpdate.InvokeSafe("executing OnUpdate hook");
        internal static void Internal_OnFixedUpdate() => OnFixedUpdate.InvokeSafe("executing OnFixedUpdate hook");
        internal static void Internal_OnLateUpdate() => OnLateUpdate.InvokeSafe("executing OnLateUpdate hook");
        internal static void Internal_OnMainSceneInitialized() => OnMainSceneInitialized.InvokeSafe("executing OnMainSceneInitialized hook");
    }
}
