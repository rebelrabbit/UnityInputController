# UnityInputManager
Event-delegate based Input Manager for Unity (C#)

These scripts allow you to have a single InputManager class throughout your project. The main script, InputManager.cs, will handle every input and call all the methods that are suscribed to the input events. 
<b>All you have to do is create a method in some Monobehaviour script and register a new event to be listened by the InputManager, so your method will be called whenever this input event is detected.</b>

The main purpose of these scripts is to remove coupling. Your InputManager shouldn't depend on other scripts' references. All your InputManager should do is detect any user input and shout: HEY! THERE WAS A CLICK! SOMEONE RELEASED THE 'A' KEY! <b>Any methods suscribed to those events will be automatically called</b>. 

<h2>Note</h2>
The InputTracker.cs and Enums.cs scripts are just helpers to the InputManager.cs script and are not 100% neccesary, but they help readability. If you wish not to use them, remember to change inside InputManager.cs the types of the variables such as InputPhase, MouseButton and  PointerInputPlace.

<h2>Installation</h2>
Copy all the scripts within your project's Assets folder. As long as they are inside this folder, you can place them wherever you want. 

<h2>Usage</h2>

Make sure you have one and only one instance of the InputManager script attached to any GameObject in your scene.
Then you can register new input events from every monobehaviour script since InputManager's methods are static. 

<h4>Examples</h4>
<i>
InputManager.RegisterKeyEvent(InputPhase.onPressed, KeyCode.A, TestPressKeyA);
<br><br>
InputManager.RegisterKeyEvent(InputPhase.onHold, KeyCode.A, TestHoldKeyA);
<br><br>
InputManager.RegisterKeyEvent(InputPhase.onReleased, KeyCode.A, TestReleaseKeyA);
<br><br>
InputManager.RegisterMouseEvent(InputPhase.onPressed, MouseButton.Left, PointerInputPlace.onAnything, TestAnything);
<br><br>
InputManager.RegisterMouseEvent(InputPhase.onPressed, MouseButton.Left, PointerInputPlace.onEmptySpace, TestEmptySpace);
<br><br>
</i>
We need our method <i>TestPressKeyA</i> to be called whenever the user presses the key 'A', our method <i>TestHoldKeyA</i> when the user is holding the key 'A' and our method <i>TestReleaseKeyA</i> when the user releases the key.<br>
Also, we need that our method <i>TestAnything</i> is called whenever the user has left-clicked anywhere on the screen. BUT, if the user clicks over empty space (hasn't clicked on any collider), then our method <i>TestEmptySpace</i> should be called. 
<br>-------------------------------<br>
<br><i>
InputManager.RegisterMouseOverObjectEvent(InputPhase.onPressed, MouseButton.Left, PointerInputPlace.onObject, TestOnAutoClick, gameObject);
<br><br>
InputManager.RegisterMouseOverObjectEvent(InputPhase.onPressed, MouseButton.Left, PointerInputPlace.onObject, TestOnLayer, "layerName");
</i>
<br><br>
These events are not as simple as the previous ones. They will detect input over a GameObject. You can pass the GameObject as a reference, pass the name of the Layer you want to check or even both. 

<h4>Input Data</h4>

That's nice and all, but what if I want to know, for example, in which coordinates the user clicked. 

There is an extra attribute in the MouseEvent class. It is called inputData. Everytime an event is detected, data about the input is collected. Right now, only X and Y position of the input is collected, but this can be easily extended.

In order to use the input data, you <b>need</b> to store a reference of the event created. Then you can use this reference to access the inputData. For example:

<i>
void Start()<br>
{<br>
    event1 = InputManager.RegisterMouseOverObjectEvent(InputPhase.onPressed, MouseButton.Left, PointerInputPlace.onObject, TestOnGameObject, someGameObject);<br>
}<br>
<br>
void TestOnGameObject()<br>
{<br>
    Debug.Log("Coordinates: (" + event1.inputData.posX + ", " + event1.inputData.posY + ")");<br>
}<br>
</i>

<h2>What's next</h2>

I'm using this for my projects and I'm loving it. Sure, it lacks some functionality, but it's going to get updated constantly. Since I'm working on a PC project, there aren't TouchEvents or GamepadEvents yet. However, this code structure allows a quick implementation of those features. <br> <br>
