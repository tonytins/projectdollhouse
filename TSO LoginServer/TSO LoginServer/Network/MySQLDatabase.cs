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
using System.Security.Cryptography;
using System.IO;
using MySql.Data.MySqlClient;
using TSO_LoginServer.Network.Encryption;

namespace TSO_LoginServer.Network
{
    /// <summary>
    /// A class containing static functions for interacting with a DB on a MySQL server.
    /// </summary>
    class MySQLDatabase
    {
        private static MySqlConnection m_Connection;

        public static void Connect()
        {
            try
            {
                m_Connection = new MySqlConnection("Data Source=AFR0-PC\\SQLEXPRESS;" +
                    "Initial Catalog=TSO;User Id=Afr0;Password=Prins123;Asynchronous Processing=true");
                m_Connection.Open();
            }
            catch (Exception)
            {
                throw new NoDBConnection("Couldn't connect to database server! Reverting to flat file DB.");
            }
        }

        /// <summary>
        /// Checks whether or not an account existed, and whether or not the password supplied was correct.
        /// </summary>
        /// <param name="AccountName">The name of the account.</param>
        /// <param name="Client">The client that supplied the account.</param>
        /// <param name="Hash">The hash of the password (with the username as a salt).</param>
        public static void CheckAccount(string AccountName, LoginClient Client, byte[] Hash)
        {
            if (m_Connection == null)
            {
                if (GlobalSettings.Default.CreateAccountsOnLogin == false)
                {
                    //TODO: Check if a flat file database exists, otherwise send an accountlogin failed packet.
                }
                else
                {
                    //TODO: Write account into flat file DB if it doesn't exist.
                }
            }

            //Gets the data from both rows (AccountName & Password) 
            MySqlCommand Command = new MySqlCommand("SELECT AccountName, Password FROM Accounts");
            Command.Connection = m_Connection;
            EndCheckAccount(Command.BeginExecuteReader(System.Data.CommandBehavior.Default));
        }

        /// <summary>
        /// Callback-function for CheckAccount().
        /// </summary>
        private static void EndCheckAccount(IAsyncResult AR)
        {
            DatabaseAsyncObject AsyncObject = AR.AsyncState as DatabaseAsyncObject;
            bool FoundAccountName = false;

            using (MySqlDataReader Reader = AsyncObject.MySQLCmd.EndExecuteReader(AR))
            {
                while (Reader.Read())
                {
                    if (((string)Reader[0]).ToUpper() == AsyncObject.AccountName.ToUpper())
                    {
                        FoundAccountName = true;

                        AsyncObject.Password = (string)Reader[1];
                    }
                }
            }

            if (FoundAccountName == true)
            {
                //0x01 = InitLoginNotify
                PacketStream P = new PacketStream(0x01, 2);

                SaltedHash SHash = new SaltedHash(new SHA512Managed(), AsyncObject.AccountName.Length);

                if (SHash.VerifyHash(Encoding.ASCII.GetBytes(AsyncObject.Password.ToUpper()), AsyncObject.Hash,
                    Encoding.ASCII.GetBytes(AsyncObject.AccountName)))
                {
                    AsyncObject.Client.Username = AsyncObject.AccountName.ToUpper();
                    AsyncObject.Client.Password = AsyncObject.Password.ToUpper();
                    P.WriteByte(0x01);
                    P.WriteByte(0x01);
                }
                else //The client's password was wrong.
                {
                    PacketStream RejectPacket = new PacketStream(0x02, 2);
                    RejectPacket.WriteByte(0x02);
                    RejectPacket.WriteByte(0x02);
                    AsyncObject.Client.Send(RejectPacket.ToArray());

                    Logger.LogInfo("Bad password - sent SLoginFailResponse!\r\n");

                    return;
                }

                AsyncObject.Client.Send(P.ToArray());

                Logger.LogInfo("Sent InitLoginNotify!\r\n");
            }
            else
            {
                PacketStream P = new PacketStream(0x02, 2);
                P.WriteByte(0x02);
                P.WriteByte(0x01);
                AsyncObject.Client.Send(P.ToArray());

                Logger.LogInfo("Bad accountname - sent SLoginFailResponse!\r\n");
                //AsyncObject.Client.Disconnect();
            }

            //If this setting is true, it means an account will be created
            //if it doesn't exist.
            if (GlobalSettings.Default.CreateAccountsOnLogin == true)
            {
                if (FoundAccountName == false)
                {
                    //No idea if this call is gonna succeed, given it's called from a callback function...
                    CreateAccount(AsyncObject.AccountName, AsyncObject.Password);
                }
            }
        }

        /// <summary>
        /// Checks when a client's characters were last cached, against a timestamp received from the client.
        /// If the client's timestamp doesn't match the one in the DB (meaning it was older or newer), information
        /// about all the characters is sent to the client.
        /// </summary>
        /// <param name="Timestamp">The timestamp received from the client.</param>
        public static void CheckCharacterTimestamp(string AccountName, LoginClient Client, DateTime Timestamp)
        {
            MySqlCommand Command = new MySqlCommand("SELECT AccountName, NumCharacters, Character1, Character2, Character3 " +
            "FROM Accounts");
            Command.Connection = m_Connection;
            EndCheckCharacterID(Command.BeginExecuteReader(System.Data.CommandBehavior.Default));
        }

        /// <summary>
        /// Callback mehod for CheckCharacterTimestamp.
        /// This queries for the existence of a particular account
        /// in the DB and retrieves the character IDs associated with it.
        /// </summary>
        private static void EndCheckCharacterID(IAsyncResult AR)
        {
            DatabaseAsyncObject AsyncObject = AR.AsyncState as DatabaseAsyncObject;
            bool FoundAccountName = false;

            int NumCharacters = 0;
            int CharacterID1 = 0;
            int CharacterID2 = 0;
            int CharacterID3 = 0;

            using (MySqlDataReader Reader = AsyncObject.MySQLCmd.EndExecuteReader(AR))
            {
                while (Reader.Read())
                {
                    if (((string)Reader[0]).ToUpper() == AsyncObject.AccountName.ToUpper())
                    {
                        FoundAccountName = true;

                        NumCharacters = (int)Reader[1];

                        if (NumCharacters == 0)
                            break;
                        else if (NumCharacters == 1)
                            CharacterID1 = (int)Reader[2];
                        else if (NumCharacters == 2)
                        {
                            CharacterID1 = (int)Reader[2];
                            CharacterID2 = (int)Reader[3];
                        }
                        else if (NumCharacters == 3)
                        {
                            CharacterID1 = (int)Reader[2];
                            CharacterID2 = (int)Reader[3];
                            CharacterID3 = (int)Reader[4];
                        }

                        if (FoundAccountName == true)
                            break;
                    }
                }
            }

            if (FoundAccountName)
            {
                if (NumCharacters > 0)
                {
                    MySqlCommand Command = new MySqlCommand("SELECT CharacterID, LastCached, Name, Sex FROM Character");

                    AsyncObject.NumCharacters = NumCharacters;
                    AsyncObject.CharacterID1 = CharacterID1;
                    AsyncObject.CharacterID2 = CharacterID2;
                    AsyncObject.CharacterID3 = CharacterID3;

                    Command.Connection = m_Connection;
                    EndCheckCharacterTimestamp(Command.BeginExecuteReader(System.Data.CommandBehavior.Default));
                }
                else
                {
                    PacketStream Packet = new PacketStream(0x05, 0);
                    Packet.WriteByte(0x00); //0 characters.

                    AsyncObject.Client.SendEncrypted(0x05, Packet.ToArray());
                }
            }
        }

        /// <summary>
        /// Callback method for EndCheckCharacterID.
        /// This retrieves information about the characters 
        /// corresponding to the IDs retrieved earlier.
        /// </summary>
        private static void EndCheckCharacterTimestamp(IAsyncResult AR)
        {
            DatabaseAsyncObject AsyncObject = AR.AsyncState as DatabaseAsyncObject;

            List<Sim> Sims = new List<Sim>();

            using (MySqlDataReader Reader = AsyncObject.MySQLCmd.EndExecuteReader(AR))
            {
                while (Reader.Read())
                {
                    if ((int)Reader[0] == AsyncObject.CharacterID1)
                    {
                        int CharacterID = AsyncObject.CharacterID1;

                        Sim Character = new Sim((string)Reader[1]);
                        Character.CharacterID = CharacterID;
                        Character.Timestamp = (string)Reader[2];
                        Character.Name = (string)Reader[3];
                        Character.Sex = (string)Reader[4];

                        Sims.Add(Character);
                    }

                    if (AsyncObject.NumCharacters == 1)
                        break;

                    if (AsyncObject.NumCharacters > 1)
                    {
                        if ((int)Reader[1] == AsyncObject.CharacterID2)
                        {
                            int CharacterID = AsyncObject.CharacterID2;

                            Sim Character = new Sim((string)Reader[1]);
                            Character.CharacterID = CharacterID;
                            Character.Timestamp = (string)Reader[2];
                            Character.Name = (string)Reader[3];
                            Character.Sex = (string)Reader[4];

                            Sims.Add(Character);
                        }
                    }

                    if (AsyncObject.NumCharacters == 2)
                        break;

                    if (AsyncObject.NumCharacters > 2)
                    {
                        if ((int)Reader[2] == AsyncObject.CharacterID3)
                        {
                            int CharacterID = AsyncObject.CharacterID3;

                            Sim Character = new Sim((string)Reader[1]);
                            Character.CharacterID = CharacterID;
                            Character.Timestamp = (string)Reader[2];
                            Character.Name = (string)Reader[3];
                            Character.Sex = (string)Reader[4];

                            Sims.Add(Character);

                            //For now, assume that finding the third character means
                            //all characters have been found.
                            break;
                        }
                    }
                }
            }

            PacketStream Packet = new PacketStream(0x05, 0);

            MemoryStream PacketData = new MemoryStream();
            BinaryWriter PacketWriter = new BinaryWriter(PacketData);

            //The timestamp for all characters should be equal, so just check the first character.
            if (AsyncObject.CharacterTimestamp < DateTime.Parse(Sims[0].Timestamp) ||
                AsyncObject.CharacterTimestamp > DateTime.Parse(Sims[0].Timestamp))
            {
                //Write the characterdata into a temporary buffer.
                if (AsyncObject.NumCharacters == 1)
                {
                    PacketWriter.Write(Sims[0].CharacterID);
                    PacketWriter.Write(Sims[0].GUID);
                    PacketWriter.Write(Sims[0].Timestamp);
                    PacketWriter.Write(Sims[0].Name);
                    PacketWriter.Write(Sims[0].Sex);

                    PacketWriter.Flush();
                }
                else if (AsyncObject.NumCharacters == 2)
                {
                    PacketWriter.Write(Sims[0].CharacterID);
                    PacketWriter.Write(Sims[0].GUID);
                    PacketWriter.Write(Sims[0].Timestamp);
                    PacketWriter.Write(Sims[0].Name);
                    PacketWriter.Write(Sims[0].Sex);

                    PacketWriter.Write(Sims[1].CharacterID);
                    PacketWriter.Write(Sims[1].GUID);
                    PacketWriter.Write(Sims[1].Timestamp);
                    PacketWriter.Write(Sims[1].Name);
                    PacketWriter.Write(Sims[1].Sex);

                    PacketWriter.Flush();
                }
                else if (AsyncObject.NumCharacters == 3)
                {
                    PacketWriter.Write(Sims[0].CharacterID);
                    PacketWriter.Write(Sims[0].GUID);
                    PacketWriter.Write(Sims[0].Timestamp);
                    PacketWriter.Write(Sims[0].Name);
                    PacketWriter.Write(Sims[0].Sex);

                    PacketWriter.Write(Sims[1].CharacterID);
                    PacketWriter.Write(Sims[1].GUID);
                    PacketWriter.Write(Sims[1].Timestamp);
                    PacketWriter.Write(Sims[1].Name);
                    PacketWriter.Write(Sims[1].Sex);

                    PacketWriter.Write(Sims[2].CharacterID);
                    PacketWriter.Write(Sims[2].GUID);
                    PacketWriter.Write(Sims[2].Timestamp);
                    PacketWriter.Write(Sims[2].Name);
                    PacketWriter.Write(Sims[2].Sex);

                    PacketWriter.Flush();
                }

                Packet.WriteByte((byte)AsyncObject.NumCharacters);      //Total number of characters.
                Packet.Write(PacketData.ToArray(), 0, (int)PacketData.Length);

                AsyncObject.Client.SendEncrypted(0x05, Packet.ToArray());
            }
            else if (AsyncObject.CharacterTimestamp == DateTime.Parse(Sims[0].Timestamp))
            {
                PacketWriter.Write((byte)0x00); //0 characters.

                AsyncObject.Client.SendEncrypted(0x05, Packet.ToArray());
            }

            PacketWriter.Close();
        }

        /// <summary>
        /// Creates an account in the DB.
        /// </summary>
        /// <param name="AccountName">The accountname.</param>
        /// <param name="Password">The password.</param>
        public static void CreateAccount(string AccountName, string Password)
        {
            MySqlCommand Command = new MySqlCommand("INSERT INTO Accounts(AccountName, Password) VALUES('" +
                AccountName + "', '" + Password + "')");
            Command.BeginExecuteNonQuery(new AsyncCallback(EndCreateAccount), Command);
        }

        private static void EndCreateAccount(IAsyncResult AR)
        {
            MySqlCommand Cmd = AR.AsyncState as MySqlCommand;

            Cmd.EndExecuteNonQuery(AR);
        }
    }
}
