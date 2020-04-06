using UnityEngine;

public class PlayerController : MonoBehaviour {

	private StateMachine stm;

	[SerializeField] private State[] states;

	private void Start() {
		stm = new StateMachine(this, states);
	}

	private void Update() {
		stm.Run();
	}

	public Vector3 GetInput() {
		return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
	}

}