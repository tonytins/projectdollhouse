﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Files.IFF
{
    public class FCNSConstant
    {
        public string Name = "";
        public string Value = "";
    }

    public class FCNS : IFFChunk
    {
        public FCNS(IFFChunk BaseChunk) : base(BaseChunk)
        {
            FileReader Reader = new FileReader(new MemoryStream(m_Data), false);

            Reader.ReadInt32(); //4 bytes always set to 0.
            int Version = Reader.ReadInt32();
            Reader.ReadInt32(); //'SNCF'

            uint Count = Reader.ReadUInt32();

            for(int i = 0; i < Count; i++)
            {
                if(Version == 1)
                {
                    FCNSConstant Constant = new FCNSConstant();
                    Constant.Name = Reader.ReadPaddedCString();
                    Constant.Value = Reader.ReadPaddedCString();
                    Reader.ReadPaddedCString(); //Description
                }
                else
                {
                    FCNSConstant Constant = new FCNSConstant();
                    Constant.Name = Reader.ReadString();
                    Constant.Value = Reader.ReadString();
                    Reader.ReadString(); //Description
                }
            }

            Reader.Close();
            m_Data = null;
        }
    }
}
