using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager3D : MonoBehaviour
{
    [Header("Puzzle Settings")]
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;
    [SerializeField] private int size = 3;
    [SerializeField] private float gapThickness = 0.01f;

    private List<Transform> pieces = new List<Transform>();
    private int emptyLocation;
    private bool shuffling = false;

    void Start()
    {
        CreateGamePieces();
    }

    private void CreateGamePieces()
    {
        float width = 1f / size;

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;

                piece.localPosition = new Vector3(
                    -1 + (2 * width * col) + width,
                    0,
                    1 - (2 * width * row) - width
                );

                piece.name = $"{(row * size) + col}";

                if ((row == size - 1) && (col == size - 1))
                {
                    emptyLocation = (size * size) - 1;
                    piece.gameObject.SetActive(false);
                }

                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                Vector2[] uv = new Vector2[4];
                float gap = gapThickness / 2;

                uv[0] = new Vector2((width * col) + gap, 1 - (width * (row + 1)) - gap);
                uv[1] = new Vector2((width * (col + 1)) - gap, 1 - (width * (row + 1)) - gap);
                uv[2] = new Vector2((width * col) + gap, 1 - (width * row) + gap);
                uv[3] = new Vector2((width * (col + 1)) - gap, 1 - (width * row) + gap);

                mesh.uv = uv;

                if (!piece.GetComponent<Collider>())
                    piece.gameObject.AddComponent<BoxCollider>();

                pieces.Add(piece);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    if (pieces[i] == hit.transform)
                    {
                        if (SwapIfValid(i, -size, size)) break;
                        if (SwapIfValid(i, +size, size)) break;
                        if (SwapIfValid(i, -1, 0)) break;
                        if (SwapIfValid(i, +1, size - 1)) break;
                    }
                }
            }
        }

        if (!shuffling && CheckCompletion())
        {
            shuffling = true;
            StartCoroutine(WaitShuffle(0.5f));
        }
    }

    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % size) != colCheck) && ((i + offset) == emptyLocation))
        {
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            (pieces[i].localPosition, pieces[i + offset].localPosition) =
                (pieces[i + offset].localPosition, pieces[i].localPosition);

            emptyLocation = i;
            return true;
        }
        return false;
    }

    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != $"{i}") return false;
        }
        return true;
    }

    private IEnumerator WaitShuffle(float duration)
    {
        yield return new WaitForSeconds(duration);
        Shuffle();
        shuffling = false;
    }

    private void Shuffle()
    {
        int count = 0;
        int last = 0;

        while (count < size * size * size)
        {
            int rnd = Random.Range(0, size * size);
            if (rnd == last) continue;
            last = emptyLocation;

            if (SwapIfValid(rnd, -size, size)) count++;
            else if (SwapIfValid(rnd, +size, size)) count++;
            else if (SwapIfValid(rnd, -1, 0)) count++;
            else if (SwapIfValid(rnd, +1, size - 1)) count++;
        }
    }
}
