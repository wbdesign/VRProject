using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
	public List<GameObject> fishPrefabs = new List<GameObject>();
	public Vector3 tankSize = new Vector3(5, 5, 5);

	public uint minFish = 8;
	public uint maxFish = 16;
	public List<GameObject> allFish = new List<GameObject>();

	public float minGoalTime = 3;
	public float maxGoalTime = 8;
	public Vector3 goalPos = Vector3.zero;

	public Coroutine deactivating;
	private bool newGoal = false;

	// Use this for initialization
	void Start()
	{
		//RenderSettings.fogColor = Color.blue;
		//RenderSettings.fogDensity = 0.03F;
		//RenderSettings.fog = true;
		int numFish = Random.Range((int)minFish, (int)maxFish);

		if (fishPrefabs.Count > 0)
		{
			GameObject fishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Count - 1)];
			for (int i = 0; i < numFish; i++)
			{

				allFish.Add(Instantiate(fishPrefab, transform.position, Quaternion.identity));
				allFish[i].GetComponent<Boid>().myFlock = this;
			}
		}

		RandomPos();
	}

	void ToggleFish()
	{
		foreach (var fish in allFish)
		{
			if (fish)
				fish.SetActive(!fish.activeSelf);
		}
	}

	private void OnDisable()
	{
		// Turn off fish
		ToggleFish();
	}

	private void OnEnable()
	{
		// Get new Goal
		NewGoalPos();
		if (newGoal)
			newGoal = false;

		// Turn on Fish
		RandomPos();
		ToggleFish();
	}

	// Update is called once per frame
	void Update()
	{
		if (!newGoal)
			StartCoroutine(NewGoalPosCoroutine(Random.Range(minGoalTime, maxGoalTime)));
	}

	void RandomPos()
	{
		for (int i = 0; i < allFish.Count; i++)
		{
			Vector3 pos = new Vector3(Random.Range(-tankSize.x, tankSize.x),
									  Random.Range(-tankSize.y, tankSize.y),
									  Random.Range(-tankSize.z, tankSize.z));

			pos += transform.position;

			allFish[i].transform.position = pos;
		}
	}

	public IEnumerator DisableAfter(float time)
	{
		yield return new WaitForSeconds(time);
		gameObject.SetActive(false);
	}

	public IEnumerator NewGoalPosCoroutine(float time)
	{
		newGoal = true;
		yield return new WaitForSeconds(time);
		NewGoalPos();
		newGoal = false;
	}
	
	void NewGoalPos()
	{
		goalPos = new Vector3(Random.Range(-tankSize.x, tankSize.x),
								  Random.Range(-tankSize.y, tankSize.y),
								  Random.Range(-tankSize.z, tankSize.z));

		goalPos += transform.position;
	}
}
