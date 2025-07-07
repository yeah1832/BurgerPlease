using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrayController : MonoBehaviour
{
	[SerializeField]
	private Vector2 _shakeRange = new Vector2(0.8f, 0.4f);

	[SerializeField]
	private float _bendFactor = 0.1f;

	[SerializeField]
	private float _itemHeight = 0.5f;

	public int ItemCount => _items.Count; // ��� ���� ��� �ִ� ������ ����.
	public int ReservedCount => _reserved.Count; // ��� ���� �̵���.
	public int TotalItemCount => _reserved.Count + _items.Count; // ��� ���� �̵����� �������� ������ ��ü ����.

	private HashSet<Transform> _reserved = new HashSet<Transform>();
	private List<Transform> _items = new List<Transform>();

	private void Start()
	{
		StartCoroutine(CoTestBurger());
	}

	// TEST
	public GameObject prefab;

	// TEST
	IEnumerator CoTestBurger()
	{
		for (int i = 0; i < 20; i++)
		{
			yield return new WaitForSeconds(0.1f);

			GameObject go = GameObject.Instantiate(prefab);
			AddToTray(go.transform);
		}

		yield return null;
	}

	private void Update()
	{
		if (_items.Count == 0)
			return;

		Vector3 dir = GameManager.Instance.JoystickDir;
		Vector3 moveDir = new Vector3(dir.x, 0, dir.y);
		moveDir = (Quaternion.Euler(0, 45, 0) * moveDir).normalized;

		_items[0].position = transform.position;
		_items[0].rotation = transform.rotation;

		for (int i = 1; i < _items.Count; i++)
		{
			float rate = Mathf.Lerp(_shakeRange.x, _shakeRange.y, i / (float)_items.Count);

			_items[i].position =  Vector3.Lerp(_items[i].position, _items[i - 1].position + (_items[i - 1].up * _itemHeight), rate);
			_items[i].rotation = Quaternion.Lerp(_items[i].rotation, _items[i - 1].rotation, rate);

			if (moveDir != Vector3.zero)
				_items[i].rotation *= Quaternion.Euler(-i * _bendFactor * rate, 0, 0);
		}
	}

	public void AddToTray(Transform child)
	{
		_reserved.Add(child);

		Vector3 dest = transform.position + Vector3.up * TotalItemCount * _itemHeight;

		child.DOJump(dest, 5, 1, 0.3f)
			.OnComplete(() =>
			{
				_reserved.Remove(child);
				_items.Add(child);
			});
	}

	public Transform RemoveFromTray()
	{
		if (ItemCount == 0)
			return null;

		Transform item = _items.Last();

		_items.Remove(item);

		return null;
	}
}
