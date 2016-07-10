/*
 *  Written by Germán Cruz @gontzalve
 *  July 9th, 2016
 *
 *  I will update this script periodically. 
 *  If you like this script, please use it! 
 *  And please, if you have the time, report any issues you have and collaborate!
 */

namespace Enums
{
    public enum InputPhase
    { 
        onPressed,
        onHold,
        onReleased
    }

    public enum MouseButton
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

    public enum PointerInputPlace
    {
        onObject,
        onEmptySpace,
        onAnything
    }
}