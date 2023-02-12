﻿using LabFusion.Exceptions;
using LabFusion.Network;

namespace LabFusion.Senders {
    public static class GamemodeSender {
        public static void SendGamemodeTriggerResponse(ushort gamemodeId, string value)
        {
            // Make sure this is the server
            if (NetworkInfo.IsServer)
            {
                using (var writer = FusionWriter.Create())
                {
                    using (var data = GamemodeTriggerResponseData.Create(gamemodeId, value))
                    {
                        writer.Write(data);

                        using (var message = FusionMessage.Create(NativeMessageTag.GamemodeTriggerResponse, writer))
                        {
                            MessageSender.BroadcastMessage(NetworkChannel.Reliable, message);
                        }
                    }
                }
            }
            else
                throw new ExpectedClientException();
        }

        public static void SendGamemodeMetadataResponse(ushort gamemodeId, string key, string value) {
            // Make sure this is the server
            if (NetworkInfo.IsServer) {
                using (var writer = FusionWriter.Create()) {
                    using (var data = GamemodeMetadataResponseData.Create(gamemodeId, key, value)) {
                        writer.Write(data);

                        using (var message = FusionMessage.Create(NativeMessageTag.GamemodeMetadataResponse, writer)) {
                            MessageSender.BroadcastMessage(NetworkChannel.Reliable, message);
                        }
                    }
                }
            }
            else
                throw new ExpectedClientException();
        }
    }
}
