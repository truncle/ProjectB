using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalController
{
	public Rigidbody2D playerRb;
	public Collider2D playerCollider;

	private float moveSpeed = 10f;
	private float moveAcc = 80f;
	private float slowAcc = 80f;
	private float jumpAcc = 20f;
	private float reactRate = 0.5f;

	public PhysicalController(Rigidbody2D playerRb, Collider2D playerCollider)
	{
		this.playerRb = playerRb;
		this.playerCollider = playerCollider;
	}

	//��������״̬
	public void FixedUpdate()
	{
		Move();
	}

	public void Jump()
	{
		playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
		playerRb.AddForce(jumpAcc * playerRb.mass * Vector2.up, ForceMode2D.Impulse);
	}
	public void BreakJump()
	{
		playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y / 3);
	}

	private void Move()
	{
		float xInput = Input.GetAxisRaw("Horizontal");
		float speedX = playerRb.velocity.x;
		float speedSign = Mathf.Sign(speedX);
		float deltaSpeedX = 0f;

		//�����ǰ�ٶ���Ŀ���ٶ��෴������ͻ����
		if (Mathf.Abs(xInput) > 0.01)
		{
			float oppositeRate = 0f;
			if (speedSign != Mathf.Sign(xInput))
			{
				oppositeRate = Mathf.Clamp(speedX / moveSpeed, -1, 1) * speedSign;
			}
			deltaSpeedX = xInput * moveAcc * (1 + reactRate * oppositeRate) * Time.fixedDeltaTime;
		}
		//�����������ٶ��������Ч
		else if (Mathf.Abs(speedX) > 0.001f)
		{
			deltaSpeedX -= slowAcc * speedSign * Time.fixedDeltaTime;
			//�ٶȷ�������Ϊ0
			if (Mathf.Sign(speedX + deltaSpeedX) != speedSign)
				speedX *= 0;
		}

		speedX = Mathf.Clamp(speedX + deltaSpeedX, -moveSpeed, moveSpeed);

		if (Mathf.Abs(speedX) <= 0.01f)
		{
			speedX = 0;
		}

		playerRb.velocity = new Vector2(speedX, playerRb.velocity.y);
	}
}
