using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
	public List<GameObject> fishPrefabs = new List<GameObject>();
	public Vector3 tankSize = new Vector3(5, 5, 5);

	public uint minFish = 8;
	public uint maxFish = 16;
	private uint flockFish;
	public List<GameObject> allFish = new List<GameObject>();

	public float minGoalTime = 3;
	public float maxGoalTime = 8;
	public Vector3 goalPos = Vector3.zero;

	public Coroutine deactivating;
	private bool newGoal = false;

	// Use this for initialization
	void Start()
	{
		flockFish = (uint)Random.Range((int)minFish, (int)maxFish);

		// Make Fish
		StartCoroutine(CreateFish());
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
		// Make sure all fish exist
		if (allFish.Count < flockFish)
			StartCoroutine(CreateFish());

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

	void RandomFishPos(Transform fish)
	{
		Vector3 pos = new Vector3(Random.Range(-tankSize.x, tankSize.x),
									  Random.Range(-tankSize.y, tankSize.y),
									  Random.Range(-tankSize.z, tankSize.z));

		pos += transform.position;

		fish.position = pos;
	}

	void RandomPos()
	{
		for (int i = 0; i < allFish.Count; i++)
		{
			RandomFishPos(allFish[i].transform);
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

	IEnumerator CreateFish()
	{
		if (fishPrefabs.Count > 0)
		{
			GameObject fishPrefab = fishPrefabs[Random.Range(0, fishPrefabs.Count - 1)];
			while (allFish.Count < flockFish)
			{
				Vector3 rotVec = new Vector3(Random.Range(-1f, 1f),
									  Random.Range(-1f, 1f),
									  Random.Range(-1f, 1f));
				var fish = Instantiate(fishPrefab, Vector3.zero, Quaternion.Euler(rotVec));
				fish.GetComponent<Boid>().myFlock = this;
				RandomFishPos(fish.transform);
				allFish.Add(fish);

				yield return new WaitForEndOfFrame();
			}
		}
	}
}
