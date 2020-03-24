﻿using UnityEngine;
using UnityEngine.UI;

public class Cake : MonoBehaviour
{
	public Sprite[] cakes;

	public Transform[] toppings;

	public GameObject[] toppingPrefab;

	public float factorPosition;
	
	public void OnEnable()
	{
		if (!Database.HasDatabase()) return;
		
		var db = Database.Get();

		GetComponent<Image>().overrideSprite = cakes[db.cake.id];

		// Remove all toppings and hide main
		foreach (var t in toppings)
		{
			foreach (Transform child in t)
			{
				Destroy(child.gameObject);
			}

			t.gameObject.SetActive(true);
		}

		// Create topping to child
		for (var i = 0; i < db.cake.toppings.Length; i++)
		{
			if (db.cake.toppings[i] == 0)
			{
				toppings[i].gameObject.SetActive(false);
			}
			else
			{
				var newPos = new Vector2();
				for (var j = 1; j < db.cake.toppings[i]; j++)
				{
					newPos.x += factorPosition;
					newPos.y += factorPosition;
					var newTopping = Instantiate(toppingPrefab[i], toppings[i], false);
					newTopping.GetComponent<RectTransform>().anchoredPosition =  newPos;
				}
			}
		}
	}

	public void AddToppingId(int id)
	{
		var db = Database.Get();

		db.cake.toppings[id]++;

		Database.Set(db);

		OnEnable();
		
		FindObjectOfType<Sc6>().scrollView.gameObject.SetActive(false);
	}
}