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

public class GameController : MonoBehaviour
{
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

    int _score = 0;

    void OnEnable()
    {
        _shipController.Destroyed += ShipController_Destroyed;
        Enemy.Killed += Enemy_Killed;
    }

    void OnDisable()
    {
        _shipController.Destroyed -= ShipController_Destroyed;
        Enemy.Killed -= Enemy_Killed;
    }

    void Enemy_Killed(Enemy obj)
    {
        _score += 10;
        _scoreLabel.text = string.Format("SCORE: {0}", _score);
    }

    void ShipController_Destroyed()
    {
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        _gui.SetActive(false);
        _enemySpawner.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        var camera = Camera.main.transform;
        while (camera.localRotation.eulerAngles.x > 65)
        {
            camera.Rotate(-30 * Time.deltaTime, 0, 0);
            yield return null;
        }
        _gameOverOverlay.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        _totalScore.text = string.Format("TOTAL SCORE: {0}", _score);
        _totalScore.gameObject.SetActive(true);
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
                Application.LoadLevel(Application.loadedLevel);

            yield return null;
        }
    }
}
