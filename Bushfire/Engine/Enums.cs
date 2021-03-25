using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BushFire.Engine
{
    public enum KeyClickState
    {
        UP,
        PRESS,
        DOWN,

    }

    public enum MouseClickState
    {
        CLICK,
        DRAG,
        UP
    }

    public enum MouseScrollState
    {
        NONE,
        SCROLLUP,
        SCROLLDOWN
    }

    public enum ControlClickState
    {
        NONE,
        LEFTPRESS,
        RIGHTPRESS
    }

    enum DockType
    {
        TOPLEFT,
        TOPLEFTFIXEDX,
        TOPLEFTFIXEDY,
        TOPLEFTFIXEDBOTH,
        TOPRIGHT,
        TOPRIGHTFIXEDX,
        TOPRIGHTFIXEDY,
        TOPRIGHTFIXEDBOTH,
        BOTTOMRIGHT,
        BOTTOMRIGHTFIXEDX,
        BOTTOMRIGHTFIXEDY,
        BOTTOMRIGHTFIXEDBOTH,
        BOTTOMLEFT,
        BOTTOMLEFTFIXEDX,
        BOTTOMLEFTFIXEDY,
        BOTTOMLEFTFIXEDBOTH,
        SCREENRESOLUTION,
        CENTERSCREENX,
        CENTERSCREENY,
        CENTERSCREENBOTH
    }


}
