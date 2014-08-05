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
/// Spawns enemies
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    /// <summary>
    /// Prefab of the enemy ship
    /// </summary>
    [SerializeField]
    Transform _enemyPrefab = null;

    /// <summary>
    /// Spawn till the end of time
    /// </summary>
    /// <returns>coroutine</returns>
    IEnumerator Start()
    {
        while (true)
        {
            var pos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), 1.1f, 100));
            Instantiate(_enemyPrefab, pos, Quaternion.identity);
            
            // Spawn every second
            yield return new WaitForSeconds(1.0f);
        }
    }
}
