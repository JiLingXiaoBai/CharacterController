using UnityEngine;

public class GetPlayerPos : MonoBehaviour
{
    private Material _mat;
    private GameObject _player;
    private static readonly int PlayerPos = Shader.PropertyToID("_PlayerPos");

    private void Start()
    {
        _mat = GetComponent<MeshRenderer>().materials[0];
        _player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        _mat.SetVector(PlayerPos, _player.transform.position);
    }
}
