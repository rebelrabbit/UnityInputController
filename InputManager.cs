/*
 *  Written by Germán Cruz @gontzalve
 *  July 9th, 2016
 *
 *  I will update this script periodically. 
 *  If you like this script, please use it! 
 *  And please, if you have the time, report any issues you have and collaborate!
 */

using UnityEngine;
using System.Collections.Generic;
using Enums;

public struct MouseInputData
{
    public float posX;
    public float posY;
}

public delegate void InputDelegate();

public class InputEvent
{
    public InputPhase phase;
}

public class KeyEvent : InputEvent
{
    public KeyCode key;
    public event InputDelegate method;

    public void ExecuteMethod()
    {
        method();
    }
}

public class MouseEvent : InputEvent
{
    public MouseButton buttonType;
    public PointerInputPlace inputPlace;
    public MouseInputData inputData;
    public event InputDelegate method;

    public void ExecuteMethod()
    {
        method();
    }
}

public class MouseOverObjectEvent : MouseEvent
{
    public GameObject goInput;
    public string layerName;
}

public class InputManager : MonoBehaviour
{
    private static List<KeyEvent> keyEventList;
    private static List<MouseEvent> mouseEventList;
    private static List<MouseOverObjectEvent> mouseOverObjectEventList;

    // KEY EVENTS

    public static void RegisterKeyEvent(InputPhase phase, KeyCode key, InputDelegate method)
    {
        if (keyEventList == null)
        {
            keyEventList = new List<KeyEvent>();
        }

        KeyEvent existingEvent = GetKeyEventFromList(phase, key);

        if (existingEvent == null)
        {
            existingEvent = CreateKeyEvent(phase, key);
            keyEventList.Add(existingEvent);
        }

        existingEvent.method += method;
    }

    private static KeyEvent GetKeyEventFromList(InputPhase phase, KeyCode key)
    {
        if (keyEventList != null)
        {
            foreach (KeyEvent e in keyEventList)
            {
                if (e.phase == phase && e.key == key)
                {
                    return e;
                }
            }
        }

        return null;
    }

    private static KeyEvent CreateKeyEvent(InputPhase phase, KeyCode key)
    { 
        KeyEvent newEvent = new KeyEvent();

        newEvent.phase = phase;
        newEvent.key = key;

        return newEvent;
    }

    // MOUSE EVENTS

    public static void RegisterMouseEvent(InputPhase phase, MouseButton buttonType, PointerInputPlace inputPlace, InputDelegate method)
    {
        if (mouseEventList == null)
        {
            mouseEventList = new List<MouseEvent>();
        }

        MouseEvent existingEvent = GetMouseEventFromList(phase, buttonType, inputPlace);

        if (existingEvent == null)
        {
            existingEvent = CreateMouseEvent(phase, buttonType, inputPlace);
            mouseEventList.Add(existingEvent);
        }

        existingEvent.method += method;
    }

    private static MouseEvent GetMouseEventFromList(InputPhase phase, MouseButton buttonType, PointerInputPlace inputPlace)
    {
        if (mouseEventList != null)
        {
            foreach (MouseEvent e in mouseEventList)
            {
                if (e.phase == phase && e.buttonType == buttonType && e.inputPlace == inputPlace)
                {
                    return e;
                }
            }
        }

        return null;
    }

    private static MouseEvent CreateMouseEvent(InputPhase phase, MouseButton buttonType, PointerInputPlace inputPlace)
    {
        MouseEvent newEvent = new MouseEvent();

        newEvent.phase = phase;
        newEvent.buttonType = buttonType;
        newEvent.inputPlace = inputPlace;

        return newEvent;
    }

    public static MouseOverObjectEvent RegisterMouseOverObjectEvent(InputPhase phase, MouseButton buttonType, PointerInputPlace inputPlace, InputDelegate method, GameObject goInput, string layerName)
    { 
        if (mouseOverObjectEventList == null)
        {
            mouseOverObjectEventList = new List<MouseOverObjectEvent>();
        }

        MouseOverObjectEvent existingEvent = GetMouseOverObjectEventFromList(phase, buttonType, inputPlace, goInput, layerName);

        if (existingEvent == null)
        {
            existingEvent = CreateMouseOverObjectEvent(phase, buttonType, inputPlace, goInput, layerName);
            mouseOverObjectEventList.Add(existingEvent);
        }

        existingEvent.method += method;

        return existingEvent;
    }


    public static MouseOverObjectEvent RegisterMouseOverObjectEvent(InputPhase phase, MouseButton buttonType, PointerInputPlace inputPlace, InputDelegate method, GameObject goInput)
    {
        return RegisterMouseOverObjectEvent(phase, buttonType, inputPlace, method, goInput, "");
    }

    public static MouseOverObjectEvent RegisterMouseOverObjectEvent(InputPhase phase, MouseButton buttonType, PointerInputPlace inputPlace, InputDelegate method, string layerName)
    { 
        return RegisterMouseOverObjectEvent(phase, buttonType, inputPlace, method, null, layerName);
    }

    private static MouseOverObjectEvent GetMouseOverObjectEventFromList(InputPhase phase, MouseButton buttonType, PointerInputPlace inputPlace, GameObject goInput, string layerName)
    {
        if (mouseOverObjectEventList != null)
        {
            foreach (MouseOverObjectEvent e in mouseOverObjectEventList)
            {
                if (e.phase == phase && e.buttonType == buttonType && e.inputPlace == inputPlace && e.goInput == goInput && e.layerName == layerName)
                {
                    return e;
                }
            }
        }

        return null;
    }

    private static MouseOverObjectEvent CreateMouseOverObjectEvent(InputPhase phase, MouseButton buttonType, PointerInputPlace inputPlace, GameObject goInput, string layerName)
    {
        MouseOverObjectEvent newEvent = new MouseOverObjectEvent();

        newEvent.phase = phase;
        newEvent.buttonType = buttonType;
        newEvent.inputPlace = inputPlace;
        newEvent.goInput = goInput;
        newEvent.layerName = layerName;

        return newEvent;
    }

    // MONOBEHAVIOUR METHODS

    private void Update()
    {
        CheckMouseInput();
        CheckKeyboardInput();
    }

    private void CheckMouseInput()
    {
        CheckSimpleMouseInput();
        CheckMouseOverObjectInput();
    }

    private void CheckSimpleMouseInput()
    {
        bool shouldCheckClick = false;

        foreach (MouseEvent e in mouseEventList)
        {
            switch (e.phase)
            {
                case InputPhase.onPressed: shouldCheckClick = (InputTracker.HasClicked(e.buttonType)); break;
                case InputPhase.onHold: shouldCheckClick = (InputTracker.IsClicking(e.buttonType)); break;
                case InputPhase.onReleased: shouldCheckClick = (InputTracker.HasReleasedClick(e.buttonType)); break;
            }

            if (shouldCheckClick)
            {
                if (e.inputPlace == PointerInputPlace.onAnything)
                {
                    e.ExecuteMethod();
                }
                else
                {
                    GameObject goInput = InputTracker.GetObjectUnderMouse();

                    if (e.inputPlace == PointerInputPlace.onEmptySpace && goInput == null)
                    {
                        e.ExecuteMethod();
                    }
                }
            }
        }
    }

    private void CheckMouseOverObjectInput()
    {
        bool shouldCheckClick = false;
        
        foreach (MouseOverObjectEvent e in mouseOverObjectEventList)
        {
            switch (e.phase)
            {
                case InputPhase.onPressed:  shouldCheckClick = (InputTracker.HasClicked(e.buttonType)); break;
                case InputPhase.onHold:     shouldCheckClick = (InputTracker.IsClicking(e.buttonType)); break;
                case InputPhase.onReleased: shouldCheckClick = (InputTracker.HasReleasedClick(e.buttonType));break;
            }

            if (shouldCheckClick)
            {
                if (e.inputPlace == PointerInputPlace.onObject)
                {
                    GameObject goInput = InputTracker.GetObjectUnderMouse(e.layerName);

                    e.inputData = new MouseInputData();

                    e.inputData.posX = InputTracker.GetMousePositionInWorldSpace().x;
                    e.inputData.posY = InputTracker.GetMousePositionInWorldSpace().y;

                    if (e.goInput == null && goInput != null)
                    {
                        e.ExecuteMethod();
                    }
                    else if (e.goInput != null && e.goInput == goInput)
                    {
                        e.ExecuteMethod();
                    }
                }
            }
        }
    }

    private void CheckKeyboardInput()
    {
        foreach (KeyEvent e in keyEventList)
        {
            bool shouldCallMethod = false;

            switch (e.phase)
            {
                case InputPhase.onPressed:  shouldCallMethod = (InputTracker.HasPressedKey(e.key)); break;
                case InputPhase.onHold:     shouldCallMethod = (InputTracker.IsPressingKey(e.key));break;
                case InputPhase.onReleased: shouldCallMethod = (InputTracker.HasReleasedKey(e.key));break;
            }

            if (shouldCallMethod)
            {
                e.ExecuteMethod();
            }
        }
    }
}