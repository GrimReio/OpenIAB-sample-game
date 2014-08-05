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
using System.Collections;

/// <summary>
/// Anchor side enumeration
/// </summary>
public enum AnchorSide
{
    /// <summary>
    /// Align to the bottom left of the screen
    /// </summary>
    BottomLeft,

    /// <summary>
    /// Align to the bottom right of the screen
    /// </summary>
    BottomRight,

    /// <summary>
    /// Align to the top left of the screen
    /// </summary>
    TopLeft,

    /// <summary>
    /// Align to the top right of the screen
    /// </summary>
    TopRight
}

/// <summary>
/// Aligns gameObject on the screen
/// </summary>
public class Anchor : MonoBehaviour
{
    [SerializeField]
    AnchorSide _anchorSide = AnchorSide.BottomLeft;

    /// <summary>
    /// Offset in pixels
    /// </summary>
    [SerializeField]
    Vector2 _offset = Vector2.zero;

    /// <summary>
    /// Camera used for calculations
    /// </summary>
    [SerializeField]
    Camera _camera = null;

    void Awake()
    {
        // Align once on the start 
        switch (_anchorSide)
        {
            case AnchorSide.BottomLeft:
                transform.position = (Vector2) _camera.ViewportToWorldPoint(new Vector2(0, 0)) + _offset;
                break;
            case AnchorSide.BottomRight:
                transform.position = (Vector2) _camera.ViewportToWorldPoint(new Vector2(1, 0)) + _offset;
                break;
            case AnchorSide.TopLeft:
                transform.position = (Vector2) _camera.ViewportToWorldPoint(new Vector2(0, 1)) + _offset;
                break;
            case AnchorSide.TopRight:
                transform.position = (Vector2) _camera.ViewportToWorldPoint(new Vector2(1, 1)) + _offset;
                break;
        }        
    }
}
