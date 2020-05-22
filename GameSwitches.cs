using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class GameSwitchGroup
    {
        public int ID;
        public string Name;
        public List<GameSwitch> Switches = new List<GameSwitch>();
    }

    public class GameSwitch
    {
        public int ID;
        public string Name;
    }
}
