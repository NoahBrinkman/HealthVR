﻿using UnityEngine;

[RequireComponent(typeof(Transform))]
public class DropZone : MonoBehaviour
{
	public bool IsOccupied { get; private set; }
	[SerializeField] private Pickupable desiredPickupable;
	private Transform cachedTransform = null;
	private Pickupable occupyingPickupable = null;

	private void OnEnable()
	{
		cachedTransform = GetComponent<Transform>();
	}

	public void Occupy(Pickupable occupyingPickupable)
	{
		if (IsOccupied)
		{			
			return;
		}

		this.occupyingPickupable = occupyingPickupable;

		IsOccupied = true;

		occupyingPickupable.InDropZone = true;
		occupyingPickupable.transform.position = cachedTransform.position;
		occupyingPickupable.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

		if(!IsRightDroppedObject(occupyingPickupable))
		{
			return;
		}

		GameManager.Instance.onMinigameCompleted?.Invoke();
	}

	public void Unoccupy()
	{
		if (!IsOccupied)
		{
			return;
		}

		Debug.Log($"Unocupping {name}...");
		IsOccupied = false;

		occupyingPickupable.InDropZone = false;
		occupyingPickupable.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
	}

	private bool IsRightDroppedObject(Pickupable occupyingPickupable)
	{
		if(occupyingPickupable != desiredPickupable)
		{
			return false;
		}

		return true;
	}
}