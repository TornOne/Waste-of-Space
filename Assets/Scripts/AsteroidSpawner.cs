using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {
	public static AsteroidSpawner instance;

	public GameObject[] asteroidModels;
	public Asteroid asteroid;
	public int spawnBufferWidth = 10;
	public float minVelocity, maxVelocity, maxSpinVelocity;
	public float asteroidsPerSecondSquared = 0.02f;

	public readonly HashSet<Asteroid> allAsteroids = new HashSet<Asteroid>();

	void Awake() => instance = this;

	void Update() => SpawnAsteroids();

	void SpawnAsteroids() {
		float lastFrameTime = Time.time - Time.deltaTime;
		int asteroidsToSpawn = (int)(asteroidsPerSecondSquared * 0.5f * Time.time * Time.time + 0.5f) - (int)(asteroidsPerSecondSquared * 0.5f * lastFrameTime * lastFrameTime + 0.5f);
		for (int i = 0; i < asteroidsToSpawn; i++) {
			SpawnRandomAsteroid();
		}
	}

	Asteroid SpawnRandomAsteroid() {
		RectInt spawnPerimeter = GridManager.instance.bounds;
		spawnPerimeter = new RectInt(spawnPerimeter.xMin - spawnBufferWidth, spawnPerimeter.yMin + spawnBufferWidth, spawnPerimeter.width + 2 * spawnBufferWidth, spawnPerimeter.height);
		float spawnParameter = Random.Range(0f, spawnPerimeter.width + spawnPerimeter.height * 2);

		Vector3 position;
		bool leftSide;
		if (spawnParameter < spawnPerimeter.width) { //Top side
			position = new Vector3(spawnPerimeter.xMin + spawnParameter, 0, spawnPerimeter.yMax);
			leftSide = spawnParameter < spawnPerimeter.width * 0.5f;
		} else if (spawnParameter < spawnPerimeter.width + spawnPerimeter.height) { //Right side
			position = new Vector3(spawnPerimeter.xMax, 0, spawnPerimeter.yMin + spawnParameter - spawnPerimeter.width);
			leftSide = false;
		} else { //Left side
			position = new Vector3(spawnPerimeter.xMin, 0, spawnPerimeter.yMin + spawnParameter - spawnPerimeter.width - spawnPerimeter.height);
			leftSide = true;
		}

		Vector3 velocity = new Vector3(leftSide ? Random.Range(0.1f, 1f) : -Random.Range(0.1f, 1f), 0, -Random.Range(0.1f, 1f)).normalized * Random.Range(minVelocity, maxVelocity);

		Asteroid asteroid =  Instantiate(this.asteroid, position, Quaternion.identity);
		Instantiate(asteroidModels[Random.Range(0, asteroidModels.Length)], asteroid.transform);
		asteroid.velocity = velocity;
		asteroid.spinVelocity = Random.insideUnitSphere * maxSpinVelocity;

		allAsteroids.Add(asteroid);
		return asteroid;
	}

	public void RemoveAsteroid(Asteroid asteroid) => allAsteroids.Remove(asteroid);
}
