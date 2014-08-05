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
using System;

/// <summary>
/// Controls player ship logic
/// </summary>
public class ShipController : MonoBehaviour
{
    /// <summary>
    /// Fired when player is eventually killed
    /// </summary>
    public event Action Destroyed;

    /// <summary>
    /// Ship speed
    /// </summary>
    [SerializeField]
    float _speed = 100.0f;
    /// <summary>
    /// Ship tilt
    /// </summary>
    [SerializeField]
    float _tilt = 10f;
    /// <summary>
    /// Ship tilt speed
    /// </summary>
    [SerializeField]
    float _tiltSpeed = 100.0f;
    /// <summary>
    /// Delay between continuous player shots
    /// </summary>
    [SerializeField]
    float _fireInterval = 0.5f;

    #region UI references
    
    [SerializeField]
    GameButton _leftButton = null;
    [SerializeField]
    GameButton _rightButton = null;
    [SerializeField]
    GameButton _shootButton = null;

    #endregion

    /// <summary>
    /// Used to spawn projectiles in correct position
    /// </summary>
    [SerializeField]
    Transform _gun = null;

    /// <summary>
    /// Gun projectile prefab
    /// </summary>
    [SerializeField]
    Transform _bulletPrefab = null;

    [SerializeField]
    GameObject _ship = null;

    [SerializeField]
    TextMesh _hpLabel = null;

    [SerializeField]
    TextMesh _repairCountLabel = null;

    /// <summary>
    /// Material to set when premium skin is purchased
    /// </summary>
    [SerializeField]
    Material _premiumSkin = null;

    float _lastTime = -100;

    /// <summary>
    /// Player hit points
    /// </summary>
    int _hp = 100;
    /// <summary>
    /// Number of repair kits
    /// </summary>
    int _repairKitCount = 2;

    bool _isPremiumSkin = false;

    public bool IsPremiumSkin 
    { 
        get { return _isPremiumSkin; } 
        set 
        { 
            _isPremiumSkin = value;
            _ship.GetComponent<MeshRenderer>().material = _premiumSkin;
        } 
    }

    /// <summary>
    /// Turns god mode on or off
    /// </summary>
    public bool IsGodMode { get; set; }

    void Start()
    {
        // Update UI when game starts
        UpdateHpLabel();
        UpdateRepairCountLabel();
    }

    /// <summary>
    /// Update routine
    /// </summary>
    void Update()
    {
        // Control ship with keyboard or screen buttons
        float movement = 0;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || _leftButton.IsDown())
            movement = -1;
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || _rightButton.IsDown())
            movement = 1;

        // Move ship left/right
        transform.position += new Vector3(movement * _speed * Time.deltaTime, 0.0f, 0.0f);
        var camera = Camera.main;
        var viewportPos = camera.WorldToViewportPoint(transform.position);

        // When ship reaches screen border, move it to the other side
        if (viewportPos.x < 0)
        {
            viewportPos.x = 1;
            transform.position = camera.ViewportToWorldPoint(viewportPos);
        }
        else if (viewportPos.x > 1)
        {
            viewportPos.x = 0;
            transform.position = camera.ViewportToWorldPoint(viewportPos);
        }

        // Tilt
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, -movement * _tilt)), _tiltSpeed * Time.deltaTime);

        // Shoot controls
        if (_shootButton.IsDown() || Input.GetKey(KeyCode.Space))
            Shoot();
    }

    /// <summary>
    /// Shoot standard projectile
    /// </summary>
    void Shoot()
    {
        if (Time.time - _lastTime > _fireInterval)
        {
            _lastTime = Time.time;
            Instantiate(_bulletPrefab, _gun.position, Quaternion.identity);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Collide with enemy ship
        if (other.gameObject.CompareTag(Tag.Enemy) && !IsGodMode)
        {
            _hp -= 25;
            if (_hp < 0) _hp = 0;
            UpdateHpLabel();

            // DEAD
            if (_hp == 0)
            {
                _ship.SetActive(false);
                enabled = false;

                if (Destroyed != null)
                    Destroyed();
            }            
        }
    }

    /// <summary>
    /// Add repair kit to the player inventory
    /// </summary>
    public void AddRepairKit()
    {
        ++_repairKitCount;
        UpdateRepairCountLabel();
    }

    /// <summary>
    /// Restore hit points
    /// </summary>
    public void UseRepairKit()
    {
        if (_repairKitCount > 0 && _hp < 100)
        {
            --_repairKitCount;
            _hp += 25;
            UpdateHpLabel();
            UpdateRepairCountLabel();
        }
    }

    /// <summary>
    /// Number of repair kits in the player inventory
    /// </summary>
    /// <returns>number of repair kits</returns>
    public int RepairKitCount()
    {
        return _repairKitCount;
    }

    /// <summary>
    /// Yep. Update it
    /// </summary>
    void UpdateHpLabel()
    {
        _hpLabel.text = string.Format("HP: {0}", _hp);
    }

    /// <summary>
    /// Update it without mercy
    /// </summary>
    void UpdateRepairCountLabel()
    {
        _repairCountLabel.text = string.Format("Repair Kits: {0}", _repairKitCount);
    }
}