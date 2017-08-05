using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyManager : MonoBehaviour {

	public static EnemyManager instance;

	public int numEnemies = 10;
	public Enemy[] enemyPrototypes;

	void createEnemy(Enemy prototype) {
		Instantiate(prototype, transform);
	}

	void Awake() {
		Assert.IsNull(instance);
		instance = this;

		for (int i = 0; i < numEnemies; i++) {
			Enemy prototype = enemyPrototypes[(int) (Random.value * enemyPrototypes.Length)];
			createEnemy(prototype); 
		}
	}
}
