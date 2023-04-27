using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	static public GameObject playerInstance;

	public PhysicalController physicalController;
	private ActionManager playerActionManager;

	private Rigidbody2D playerRb;
	private Collider2D playerCollider;
	private Collider2D groundSensor;
	private Collider2D wallSensor;

	[SerializeField]
	private GameObject bullet;
	private float bulletLifetime = 3f;

	private ContactFilter2D groundFilter;

	private Animator animator;

	public float timeScale = 1f;
	public float lastGrounded = 0f;
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
		playerActionManager = GetComponent<ActionManager>();
		physicalController = new PhysicalController(playerRb, playerCollider);
		groundSensor = transform.Find("Ground Sensor").GetComponent<Collider2D>();
		//wallSensor = transform.Find("Wall Sensor").GetComponent<Collider2D>();
		groundFilter.SetLayerMask(groundLayerMask);
		animator = GetComponent<Animator>();
	}

	void Update()
	{
		float x = playerCollider.bounds.center.x;
		float y = playerCollider.bounds.center.y;
		float dx = playerCollider.bounds.extents.x;
		float dy = playerCollider.bounds.extents.y;
		Util.PrintBox(x - dx, y - dy, x + dx, y + dy);
		HandleInput();
		Jump();

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
		physicalController.FixedUpdate();
		GroundCheck();
		FaceTo();
	}

	private void LateUpdate()
	{

	}

	public void HandleInput()
	{
		List<ActionId> inputActions = new();

		List<InputType> inputList = new();
		//List<InputType> keyDownList = new();
		//List<InputType> keyUpList = new();

		List<InputType> keyDownInput = new() {
			InputType.Attack, InputType.Jump, InputType.Dash, InputType.Skill,
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

		ActionTable.actions.ForEach(action =>
		{
			if (Util.ContainsAll(inputList, action.inputList))
			{
				inputActions.Add(action.id);
			}
		});

		playerActionManager.AddInputAction(inputActions);

		// todo 临时用的方法，需要改进
		if (InputManager.GetKeyDown(InputType.Skill))
		{
			Shoot();
		}
	}

	public void Jump()
	{
		if (InputManager.GetKeyDown(InputType.Jump))
		{
			if (lastGrounded == 0)
				physicalController.Jump();
		}
		if (InputManager.GetKeyUp(InputType.Jump) && playerRb.velocity.y > 0)
		{
			physicalController.BreakJump();
		}
	}

	//控制朝向
	public void FaceTo()
	{
		float xInput = Input.GetAxisRaw("Horizontal");
		if (xInput != 0)
		{
			transform.localScale = new Vector3(Mathf.Sign(xInput) * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
		}
	}

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

	private void Shoot()
	{
		if (bullet != null)
		{
			GameObject bulletInstance = Instantiate(bullet);
			bulletInstance.SetActive(true);
			float rotation = transform.localScale.x > 0 ? 0 : 180;
			bulletInstance.transform.position = transform.position;
			bulletInstance.transform.rotation = Quaternion.Euler(0, 0, rotation);
			MoveTrack moveTrack = bulletInstance.AddComponent<MoveTrack>();
			moveTrack.AddPattern(0);
			//moveTrack.AddInstruction(new()
			//{
			//	speed = 20f,
			//	type = MoveType.Move,
			//	startTime = 0f,
			//	endTime = 1f,
			//});
			Destroy(bulletInstance, bulletLifetime);
		}
	}
}
