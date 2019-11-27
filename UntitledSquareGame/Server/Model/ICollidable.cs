using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    public interface ICollidable : IGameObject
    {
        bool CollidesWith(IGameObject gameObject);
    }
}
