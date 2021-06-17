using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BQ.CollisionInfo
{ 
    public class HitboxInfo
    {
        public float x, w, h, y;
        public int frame;
        BoxCollider boxcollider = null;

        public HitboxInfo(float x, float y, float w, float h, int frame)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
            this.frame = frame;
        }

        public bool CheckAgainstBoxCollider(BoxCollider i)
        {
            return (i.size.x == w && i.size.y == h && ((i.center.x == x && i.center.y == y) || (i.center.x == -x && i.center.y == -y)));
        }

        public static HitboxInfo Flipped(HitboxInfo hb)
        {
            return new HitboxInfo(-hb.x, -hb.y, hb.w, hb.h, hb.frame);
        }
    }
}
