using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPhysicalController : MonoBehaviour
{
	private Rigidbody2D playerRb;
	private Collider2D playerCollider;

	public bool FreeMove { get; set; } = true;
	public bool FreeTowards { get; set; } = true;

	private float moveSpeed = 15f;
	private float moveAcc = 80f;
	private float slowAcc = 60f;
	private float jumpAcc = 20f;
	private float reactRate = 0.5f;

	private float dashTime = 0.2f;
	private float dashSpeed = 35f;

	public GameObject DashFX;


	private void Start()
	{
		playerRb = GetComponent<Rigidbody2D>();
		playerCollider = GetComponent<Collider2D>();
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

	public void Jump()
	{
		if (FreeMove)
		{
			playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
			playerRb.AddForce(jumpAcc * playerRb.mass * Vector2.up, ForceMode2D.Impulse);
		}
	}
	public void BreakJump()
	{
		if (FreeMove)
		{
			playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y / 3);
		}
	}

	public IEnumerator Dash()
	{
		if (!FreeMove)
			yield break;
		DashFX.SetActive(true);
		float gravityScale = playerRb.gravityScale;
		FreeMove = false;
		FreeTowards = false;
		playerRb.gravityScale = 0;
		//playerRb.AddForce(dashAcc * playerRb.mass * Vector2.right * speedSign);
		playerRb.velocity = new Vector2(Mathf.Sign(transform.localScale.x) * dashSpeed, 0);
		yield return new WaitForSeconds(dashTime);
		playerRb.gravityScale = gravityScale;
		FreeTowards = true;
		FreeMove = true;
		DashFX.SetActive(false);
	}

	public void Move()
	{
		float xInput = InputManager.GetAxisHorizontal();
		float speedX = playerRb.velocity.x;
		float speedSign = Mathf.Sign(speedX);
		float deltaSpeedX = 0f;

		//如果当前速度与目标速度相反则计算冲突比例, 施加额外的减速力
		if (Mathf.Abs(xInput) > 0.01)
		{
			float oppositeRate = 0f;
			if (speedSign != Mathf.Sign(xInput))
			{
				oppositeRate = Mathf.Clamp(speedX / moveSpeed, -1, 1) * speedSign;
			}
			deltaSpeedX = xInput * moveAcc * (1 + reactRate * oppositeRate) * Time.fixedDeltaTime;
		}
		//无输入且有速度则减速生效
		else if (Mathf.Abs(speedX) > 0.001f)
		{
			deltaSpeedX -= slowAcc * speedSign * Time.fixedDeltaTime;
			//速度反向则置为0
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

	//控制朝向
	public void Towards()
	{
		float xInput = InputManager.GetAxisHorizontal();
		if (xInput != 0)
		{
			transform.localScale = new Vector3(Mathf.Sign(xInput) * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
		}
	}

	public void Stop()
	{
		playerRb.velocity = Vector2.zero;
	}
}
