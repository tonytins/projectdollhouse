﻿/*The contents of this file are subject to the Mozilla Public License Version 1.1
(the "License"); you may not use this file except in compliance with the
License. You may obtain a copy of the License at http://www.mozilla.org/MPL/

Software distributed under the License is distributed on an "AS IS" basis,
WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
the specific language governing rights and limitations under the License.

The Original Code is the SimsLib.

The Initial Developer of the Original Code is
Mats 'Afr0' Vederhus. All Rights Reserved.

Contributor(s): Nicholas Roth.
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace SimsLib.FAR3
{
    /// <summary>
    /// Represents an exception thrown by a FAR3Archive instance.
    /// </summary>
    public class FAR3Exception : Exception
    {
        public FAR3Exception(string Message)
            : base(Message)
        {
        }
    }
}
