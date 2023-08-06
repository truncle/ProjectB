using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	static private GameObject playerInstance;
	static public GameObject PlayerInstance { get { return playerInstance; } }

	public PlayerPhysicalController physicalController;
	private PlayerActionController actionController;

	private Rigidbody2D playerRb;
	private Collider2D playerCollider;
	private Collider2D groundSensor;
	private Collider2D wallSensor;

	private ContactFilter2D groundFilter;

	//可调配置
	[SerializeField]
	private GameObject bullet;
	private float bulletLifetime = 3f;
	private float dashCoolDown = 1f;
	private float chargeStateTime1 = 1f;
	private float chargeStateTime2 = 1f;

	//状态管理字段
	public float lastGrounded = 0f;
	public float lastDashTime = 0f;
	public float chargeTime = 0f;
	public bool onWall = false;

	private LayerMask groundLayerMask;

	private void Awake()
	{
		if (playerInstance == null)
		{
			playerInstance = gameObject;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(this);
		}
		groundLayerMask = LayerMask.GetMask("Ground");
	}

	void Start()
	{
		playerRb = GetComponent<Rigidbody2D>();
		playerCollider = GetComponent<Collider2D>();
		actionController = GetComponent<PlayerActionController>();
		physicalController = GetComponent<PlayerPhysicalController>();
		groundSensor = transform.Find("Ground Sensor").GetComponent<Collider2D>();
		//wallSensor = transform.Find("Wall Sensor").GetComponent<Collider2D>();
		groundFilter.SetLayerMask(groundLayerMask);
	}

	void Update()
	{
		float x = playerCollider.bounds.center.x;
		float y = playerCollider.bounds.center.y;
		float dx = playerCollider.bounds.extents.x;
		float dy = playerCollider.bounds.extents.y;
		Util.PrintBox(x - dx, y - dy, x + dx, y + dy);
		HandleInput();

		//临时用，显示背包
		if (Input.GetKeyDown(KeyCode.H))
		{
			Debug.Log("inventory info");
			foreach (var item in PlayerInventory.itemList)
			{
				Debug.Log(string.Format("itemId:{0}, num:{1}", item.Key, item.Value));
			}
		}
	}

	private void FixedUpdate()
	{
		GroundCheck();
		CheckStateAction();
	}

	private void LateUpdate()
	{

	}

	//解析玩家的按键输入，转化成角色的动作输入
	public void HandleInput()
	{
		List<ActionId> inputActions = new();
		List<InputType> inputList = new();
		//List<InputType> keyDownList = new();
		//List<InputType> keyUpList = new();

		if (CheckJump()) { inputList.Add(InputType.Jump); }
		if (CheckDash()) { inputList.Add(InputType.Dash); }
		ActionId? chargeAction = CheckCharge();
		if (chargeAction.HasValue)
		{
			inputActions.Add(chargeAction.Value);
		}

		List<InputType> keyDownInput = new() {
			InputType.Attack, InputType.Skill,
			InputType.Interact, InputType.UseItem,
		};
		List<InputType> stateInput = new()
		{
			InputType.Up,InputType.Down, InputType.Left, InputType.Right,
		};
		foreach (var inputType in keyDownInput)
		{
			if (InputManager.GetKeyDown(inputType))
			{
				inputList.Add(inputType);
			}
		}
		foreach (var inputType in stateInput)
		{
			if (InputManager.GetInput(inputType))
			{
				inputList.Add(inputType);
			}
		}

		PlayerActionTable.actions.ForEach(action =>
		{
			if (Util.ContainsAll(inputList, action.inputList))
			{
				inputActions.Add(action.id);
			}
		});

		actionController.AddInputAction(inputActions);

		// todo 临时用的方法，需要改进
		if (InputManager.GetKeyDown(InputType.Skill))
		{
			Shoot();
		}
	}

	private void Shoot()
	{
		if (bullet != null)
		{
			GameObject bulletInstance = Instantiate(bullet);
			bulletInstance.SetActive(true);
			bulletInstance.transform.position = transform.position;
			Destroy(bulletInstance, bulletLifetime);
		}
	}

	public bool CheckJump()
	{
		if (InputManager.GetKeyDown(InputType.Jump))
		{
			if (lastGrounded == 0)
			{
				physicalController.Jump();
				return true;
			}
		}
		if (InputManager.GetKeyUp(InputType.Jump) && playerRb.velocity.y > 0)
		{
			physicalController.BreakJump();
		}
		return false;
	}

	public bool CheckDash()
	{
		lastDashTime += Time.deltaTime;
		if (InputManager.GetKeyDown(InputType.Dash))
		{
			if (lastDashTime >= dashCoolDown)
			{
				StartCoroutine(physicalController.Dash());
				lastDashTime = 0f;
				return true;
			}
		}
		return false;
	}

	public ActionId? CheckCharge()
	{
		if (InputManager.GetInput(InputType.Attack))
		{
			chargeTime += Time.deltaTime;
			if (chargeTime >= chargeStateTime2)
				return ActionId.Charge2;
			else if (chargeTime >= chargeStateTime1)
				return ActionId.Charge1;
		}
		else if (InputManager.GetKeyUp(InputType.Attack))
		{
			float endChargeTime = chargeTime;
			chargeTime = 0f;
			if (endChargeTime >= chargeStateTime2)
				return ActionId.ChargeAttack2;
			else if (endChargeTime >= chargeStateTime1)
				return ActionId.ChargeAttack1;
		}
		return null;
	}

	//===================== Fixed Update =====================

	private void GroundCheck()
	{
		lastGrounded += Time.fixedDeltaTime;

		List<Collider2D> overlappedColliders = new();

		groundSensor.OverlapCollider(groundFilter, overlappedColliders);
		if (overlappedColliders.Count > 0)
		{
			lastGrounded = 0f;
		}
	}

	private void CheckStateAction()
	{
		ActionId actionId;
		actionId = lastGrounded == 0f ? ActionId.Move : ActionId.Jump;
		actionController.StateActionId = actionId;
	}

	private void WallCheck()
	{
		List<Collider2D> overlappedColliders = new();
		wallSensor.OverlapCollider(groundFilter, overlappedColliders);

		if (overlappedColliders.Count > 0)
		{
			onWall = true;
		}
		else
		{
			onWall = false;
		}
	}
}
