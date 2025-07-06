using UnityEngine;
using static Define;

[RequireComponent(typeof(Animation))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
   [SerializeField, Range(1, 5)]
	private float _moveSpeed = 3;

	[SerializeField]
	private float _rotateSpeed = 360;

	private Animator _animator;
	private CharacterController _controller;
	private AudioSource _audioSource;
	private EState _state = EState.None;
	public EState State
	{
		get { return _state; }
		private set
		{
			if (_state == value)
				return;

			_state = value;
			UpdateAnimation();
		}
	}

	public bool IsServing { get; set; } = false;	
    private void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController>();
		_audioSource = GetComponent<AudioSource>();
	}

    private void Update()
	{
		Vector3 dir = GameManager.Instance.JoystickDir;
		Vector3 moveDir = new Vector3(dir.x, 0, dir.y);
		moveDir = (Quaternion.Euler(0, 45, 0) * moveDir).normalized;

		if (moveDir != Vector3.zero)
		{
			// 이동.
			_controller.Move(moveDir * Time.deltaTime * _moveSpeed);

			// 고개 돌리기.
			Quaternion lookRotation = Quaternion.LookRotation(moveDir);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * _rotateSpeed);

			_animator.CrossFade("Move", 0.05f);
		}
		else
		{
			_animator.CrossFade("Idle", 0.1f);
		}
		// 중력 작용
		transform.position = new Vector3(transform.position.x, 0, transform.position.z);
	}

	private void UpdateAnimation()
	{
		switch (State)
		{
			case EState.Idle:
				_animator.CrossFade(IsServing ? Define.SERVING_IDLE : Define.IDLE, 0.1f);
				break;
			case EState.Move:
				_animator.CrossFade(IsServing ? Define.SERVING_MOVE : Define.MOVE, 0.05f);
				break;
		}
	}
}
