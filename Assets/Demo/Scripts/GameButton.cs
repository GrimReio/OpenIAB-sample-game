/*******************************************************************************
* Copyright 2012-2014 One Platform Foundation
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
******************************************************************************/

using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Simple screen button
/// </summary>
public class GameButton : MonoBehaviour 
{
    /// <summary>
    /// Fired when pushed down
    /// </summary>
    public event Action<GameButton> Down;
    
    /// <summary>
    /// Fired when released
    /// </summary>
    public event Action<GameButton> Up;

    bool _isDown = false;

    void OnMouseDown()
    {
        _isDown = true;
        transform.localScale = Vector3.one * 0.75f;
        if (Down != null)
            Down(this);
    }

    void OnMouseUp()
    {
        transform.localScale = Vector3.one;
        if (Up != null)
            Up(this);
        _isDown = false;
    }

    public bool IsDown()
    {
        return _isDown;
    }
}
