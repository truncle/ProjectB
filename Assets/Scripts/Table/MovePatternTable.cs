using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public struct MoveInstruction
{
	public float speed;
	public MoveType type;
	public float startTime;
	public float endTime;
}

public struct MovePattern
{
	public int id;
	public List<MoveInstruction> instructions;
}

public enum MoveType
{
	Move, Rotate
}

public static class MovePatternTable
{
	static public List<MovePattern> patternList = new(){
		new()
		{
			id = 0,
			instructions = new(){
				new MoveInstruction {speed = 5, type = MoveType.Move, startTime = 0, endTime = 1f},
				new MoveInstruction {speed = 5, type = MoveType.Move, startTime = 1f, endTime = 2f},
				new MoveInstruction {speed = 90, type = MoveType.Rotate, startTime = 1f, endTime = 2f},
			}
		}
	};

	static public MovePattern GetMovePattern(int patternId)
	{
		return patternList[patternId];
	}
}
