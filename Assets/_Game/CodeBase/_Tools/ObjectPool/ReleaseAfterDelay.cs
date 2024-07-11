using System.Collections;
using CodeBase._Tools.Helpers;
using UnityEngine;

namespace CodeBase._Tools.ObjectPool
{
	[RequireComponent(typeof(IPoolable))]
	public class ReleaseAfterDelay : MonoBehaviour
	{
		[SerializeField] private float _delay = 3f;

		private Coroutine _releaseCor;
		private IPoolable _poolItem;

		private void Start()
		{
			_poolItem = GetComponent<IPoolable>();
		}

		private void OnEnable()
		{
			RestartObject();
		}

		public void SetDelay(float delay = 0)
		{
			_delay = Mathf.Clamp(delay, 0, float.MaxValue);
		}

		private void RestartObject()
		{
			if (_delay < 0) return;
			_releaseCor.Stop(this);
			_releaseCor = StartCoroutine(ReleaseAfterTimeCor());
		}

		private IEnumerator ReleaseAfterTimeCor()
		{
			yield return new WaitForSeconds(_delay);

			_poolItem?.OnReturnToPool();
		}

		private void OnDisable()
		{
			_releaseCor.Stop(this);
		}
	}
}