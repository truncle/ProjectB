using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicalController : MonoBehaviour
{
	private Rigidbody2D objectRigidbody;
	private Collider2D objectCollider;

	public bool FreeMove { get; set; } = true;
	public bool FreeTowards { get; set; } = true;

	private void Start()
	{
		objectRigidbody = GetComponent<Rigidbody2D>();
		objectCollider = GetComponent<Collider2D>();
	}


	//更新物理状态
	void FixedUpdate()
	{
		if (FreeMove)
		{
			Move();
			if (FreeTowards)
				Towards();
		}
	}

	public void Move()
	{
	}

	//控制朝向
	public void Towards()
	{
	}

	public void DoAction(string actionName, int frame)
	{

	}

	public void Stop()
	{
		objectRigidbody.velocity = Vector2.zero;
	}
}
