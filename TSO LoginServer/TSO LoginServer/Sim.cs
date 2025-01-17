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

namespace TSO_LoginServer
{
    /// <summary>
    /// Represents a Sim/Character in the game.
    /// </summary>
    public class Sim
    {
        private string m_GUID;
        private int m_CharacterID;
        private string m_Timestamp;
        private string m_Name;
        private string m_Sex;

        private bool m_CreatedThisSession = false;

        /// <summary>
        /// A Sim's GUID, created by the client and stored in the DB.
        /// </summary>
        public string GUID
        {
            get { return m_GUID; }
        }

        /// <summary>
        /// The character's ID, as it exists in the DB.
        /// </summary>
        public int CharacterID
        {
            get { return m_CharacterID; }
            set { m_CharacterID = value; }
        }

        /// <summary>
        /// When was this character last cached by the client?
        /// </summary>
        public string Timestamp
        {
            get { return m_Timestamp; }
            set { m_Timestamp = value; }
        }

        /// <summary>
        /// The character's name, as it exists in the DB.
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public string Sex
        {
            get { return m_Sex; }
            set { m_Sex = value; }
        }

        /// <summary>
        /// Set to true when a CharacterCreate packet was
        /// received. If this is false, the character in
        /// the DB will NOT be updated with the city that
        /// the character resides in when receiving a 
        /// KeyRequest packet from a CityServer, saving 
        /// an expensive DB call.
        /// </summary>
        public bool CreatedThisSession
        {
            get { return m_CreatedThisSession; }
            set { m_CreatedThisSession = value; }
        }

        public Sim(string GUID)
        {
            m_GUID = GUID;
        }
    }
}
