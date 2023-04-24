using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveTrack : MonoBehaviour
{

	[SerializeField]
	private List<int> initPatterns;

	public List<MoveInstruction> instructions = new();
	private float lifeTime = 0f;

	private void Start()
	{
		foreach (var patternId in initPatterns)
		{
			AddPattern(patternId);
		}
	}

	// Update is called once per frame
	void Update()
	{
		GetMoveResult(out float speed, out float rotateSpeed);
		transform.Translate(speed * Time.deltaTime * Vector3.right);
		transform.Rotate(rotateSpeed * Time.deltaTime * Vector3.forward);
		lifeTime += Time.deltaTime;
	}

	private void GetMoveResult(out float speed, out float rotateSpeed)
	{
		speed = 0f;
		rotateSpeed = 0f;
		foreach (MoveInstruction pattern in instructions.Where(p => lifeTime >= p.startTime && lifeTime <= p.endTime))
		{
			if (pattern.type == MoveType.Move)
			{
				speed += pattern.speed;
			}
			else if (pattern.type == MoveType.Rotate)
			{
				rotateSpeed += pattern.speed;
			}
		}
	}

	public void AddInstruction(MoveInstruction instruction)
	{
		instructions.Add(instruction);
	}

	public void AddPattern(int patternId)
	{
		MovePattern pattern = MovePatternTable.GetMovePattern(patternId);
		AddPattern(pattern);
	}
	public void AddPattern(MovePattern movePattern)
	{
		instructions.AddRange(movePattern.instructions);
	}

}
