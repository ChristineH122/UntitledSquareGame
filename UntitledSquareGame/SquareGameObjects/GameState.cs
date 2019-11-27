﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquareGameObjects
{
    [Serializable()]
    public class GameState
    {
        public Square PlayerOne
        {
            get;
            set;
        }

        public Square PlayerTwo
        {
            get;
            set;
        }

        public List<Square> Searchers
        {
            get;
            set;
        }
    }
}
