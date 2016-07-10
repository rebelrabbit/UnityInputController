/*
 *  Written by Germán Cruz @gontzalve
 *  July 9th, 2016
 *
 *  I will update this script periodically. 
 *  If you like this script, please use it! 
 *  And please, if you have the time, report any issues you have and collaborate!
 */

using UnityEngine;
using Enums;

public static class InputTracker 
{
	#region Mouse detection
	
	public static bool HasClicked(MouseButton button)
	{
		return Input.GetMouseButtonDown((int)button);
	}

	public static bool IsClicking(MouseButton button)
	{
		return Input.GetMouseButton((int)button);
	}

	public static bool HasReleasedClick(MouseButton button)
	{
		return Input.GetMouseButtonUp((int)button);
	}

	public static Vector3 GetMousePositionInWorldSpace()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    #endregion

    #region Object detection

    public static GameObject GetObjectUnderMouse(string layerName = "")
	{
		Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 posicion = new Vector2(wp.x, wp.y);
		Collider2D col = null;

		col = (layerName == "") ? (Physics2D.OverlapPoint(posicion)) : (Physics2D.OverlapPoint(posicion, 1<<LayerMask.NameToLayer(layerName)));

		if(col != null)
		{
			return col.gameObject;
		}

		return null;
	}

	#endregion

	#region Key detection

	public static bool HasPressedKey(KeyCode key)
	{
		return Input.GetKeyDown(key);
	}

	public static bool IsPressingKey(KeyCode key)
	{
		return Input.GetKey(key);
	}

	public static bool HasReleasedKey(KeyCode key)
	{
		return Input.GetKeyUp(key);
	}

	#endregion
}
