﻿/*The contents of this file are subject to the Mozilla Public License Version 1.1
(the "License"); you may not use this file except in compliance with the
License. You may obtain a copy of the License at http://www.mozilla.org/MPL/

Software distributed under the License is distributed on an "AS IS" basis,
WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
the specific language governing rights and limitations under the License.

The Original Code is the TSO LoginServer.

The Initial Developer of the Original Code is
Mats 'Afr0' Vederhus. All Rights Reserved.

Contributor(s): ______________________________________.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace TSO_LoginServer.Network
{
    /// <summary>
    /// Contains static methods for handling incoming packets from CityServers.
    /// </summary>
    class CityServerPacketHandlers
    {
        /// <summary>
        /// A cityserver logged in!
        /// </summary>
        public static void HandleCityServerLogin(PacketStream P, ref CityServerClient Client)
        {
            byte PacketLength = (byte)P.ReadByte();

            string Name = P.ReadString();
            string Description = P.ReadString();
            ulong Thumbnail = P.ReadUInt64();
            string IP = P.ReadString();
            int Port = P.ReadInt32();

            CityServerInfo Info = new CityServerInfo(Name, Description, Thumbnail, IP, Port);
            Client.ServerInfo = Info;
        }

        /// <summary>
        /// A cityserver requested a decryptionkey for a client!
        /// </summary>
        public static void HandleKeyFetch(ref LoginListener Listener, PacketStream P, CityServerClient Client)
        {
            string AccountName = P.ReadString();

            byte[] EncKey = new byte[1];

            foreach (LoginClient Cl in Listener.Clients)
            {
                if (Cl.Username == AccountName)
                {
                    EncKey = Cl.EncKey;

                    if (Cl.CurrentlyActiveSim.CreatedThisSession)
                    {
                        //TODO: Update the DB to reflect the city that
                        //      this sim resides in.
                        Database.UpdateCityForCharacter(Cl.CurrentlyActiveSim.Name, Client.ServerInfo.Name);
                    }
                }
            }

            PacketStream OutPacket = new PacketStream(0x01, 0x00);
            OutPacket.WriteByte((byte)0x01);
            OutPacket.WriteByte((byte)(EncKey.Length + 2));
            OutPacket.WriteByte((byte)EncKey.Length);
            OutPacket.Write(EncKey, 0, EncKey.Length);
            Client.Send(OutPacket.ToArray());

            //For now, assume client has already disconnected and doesn't need to be disconnected manually.
            Listener.TransferringClients.Remove(Client);
        }
    }
}
