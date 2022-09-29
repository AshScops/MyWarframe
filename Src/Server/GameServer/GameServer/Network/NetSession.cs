using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameServer;
using GameServer.Entities;
using SkillBridge.Message;

namespace Network
{
    class NetSession
    {
        //private TUser user = new TUser();

        //public TUser User { get => user; set => user = value; }

        public TUser User { get; set; }
        public Character Character { get; set; }
        public NEntity Entity { get; set; }
    }
}
