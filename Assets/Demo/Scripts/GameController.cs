using UnityEngine;
using System.Collections;

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

/// <summary>
/// Straightforward game logic controller
/// </summary>
public class GameController : MonoBehaviour
{
    #region Game references

    [SerializeField]
    ShipController _shipController = null;

    [SerializeField]
    GameObject _gui = null;

    [SerializeField]
    GameObject _gameOverOverlay = null;

    [SerializeField]
    TextMesh _scoreLabel = null;

    [SerializeField]
    TextMesh _totalScore = null;

    [SerializeField]
    GameObject _enemySpawner = null;

    #endregion

    /// <summary>
    /// Stores final player score
    /// </summary>
    int _score = 0;

    void OnEnable()
    {
        // Subscribe to events
        _shipController.Destroyed += ShipController_Destroyed;
        Enemy.Killed += Enemy_Killed;
    }

    void OnDisable()
    {
        // Unsubscribe to avoid leaks
        _shipController.Destroyed -= ShipController_Destroyed;
        Enemy.Killed -= Enemy_Killed;
    }

    /// <summary>
    /// Enemy takedown handler
    /// </summary>
    /// <param name="enemy">enemy ship</param>
    void Enemy_Killed(Enemy enemy)
    {
        // Add some score
        _score += 10;
        // Update score on screen
        _scoreLabel.text = string.Format("SCORE: {0}", _score);
    }

    /// <summary>
    /// Player takedown handler
    /// </summary>
    void ShipController_Destroyed()
    {
        // Start game over movie
        StartCoroutine(GameOverCoroutine());
    }

    /// <summary>
    /// Game over beautification coroutine
    /// </summary>
    /// <returns></returns>
    IEnumerator GameOverCoroutine()
    {
        // Disable UI
        _gui.SetActive(false);
        // Disable spawning of enemies
        _enemySpawner.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        var camera = Camera.main.transform;
        // Rotate camera a bit
        while (camera.localRotation.eulerAngles.x > 65)
        {
            camera.Rotate(-30 * Time.deltaTime, 0, 0);
            yield return null;
        }
        // Show GAME OVER overlay
        _gameOverOverlay.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        _totalScore.text = string.Format("TOTAL SCORE: {0}", _score);
        _totalScore.gameObject.SetActive(true);
        while (true)
        {
            // Restart game
            if (Input.GetMouseButtonDown(0))
                Application.LoadLevel(Application.loadedLevel);

            yield return null;
        }
    }
}
