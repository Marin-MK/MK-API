using System;
using System.Collections.Generic;
using System.Text;

namespace MKAPI
{
    public class GameVariableGroup
    {
        public int ID;
        public string Name;
        public List<GameVariable> Variables = new List<GameVariable>();
    }

    public class GameVariable
    {
        public int ID;
        public string Name;
    }
}
