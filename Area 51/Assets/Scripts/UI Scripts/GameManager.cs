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
        StartCoroutine(WaitShuffle(0.5f));
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
                if (mesh.vertexCount == 4)
                {
                    Vector2[] uv = new Vector2[4];
                    float gap = gapThickness / 2;

                    uv[0] = new Vector2((width * col) + gap, 1 - (width * (row + 1)) - gap);
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - (width * (row + 1)) - gap);
                    uv[2] = new Vector2((width * col) + gap, 1 - (width * row) + gap);
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - (width * row) + gap);

                    mesh.uv = uv;
                }

                if (!piece.GetComponent<Collider>())
                    piece.gameObject.AddComponent<BoxCollider>();

                pieces.Add(piece);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !shuffling)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    if (pieces[i] == hit.transform)
                    {
                        if (SwapIfValid(i, -size))
                            break;      
                        if (SwapIfValid(i, +size)) 
                            break;       
                        if (SwapIfValid(i, -1, true)) 
                            break;   
                        if (SwapIfValid(i, +1, true)) 
                            break;  
                    }
                }

                if (CheckCompletion())
                {
                    Debug.Log("Puzzle Completed!");
                }
            }
        }
    }

    private bool SwapIfValid(int i, int offset, bool checkColumn = false)
    {
        int target = i + offset;

        if (target < 0 || target >= pieces.Count)
            return false;

        
        if (checkColumn && (i / size != target / size))
            return false;

        if (target == emptyLocation)
        {
            (pieces[i], pieces[target]) = (pieces[target], pieces[i]);
            
            (pieces[i].localPosition, pieces[target].localPosition) =
                (pieces[target].localPosition, pieces[i].localPosition);

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
        shuffling = true;
        yield return new WaitForSeconds(duration);
        Shuffle();
        shuffling = false;
    }

    private void Shuffle()
    {
        for (int i = 0; i < size * size * size; i++)
        {
            List<int> neighbours = new List<int>();

            if (emptyLocation >= size) 
                neighbours.Add(emptyLocation - size);          
            if (emptyLocation < pieces.Count - size) 
                neighbours.Add(emptyLocation + size); 
            if (emptyLocation % size != 0) 
                neighbours.Add(emptyLocation - 1);        
            if (emptyLocation % size != size - 1) 
                neighbours.Add(emptyLocation + 1); 

            int rndIndex = Random.Range(0, neighbours.Count);
            int swapIndex = neighbours[rndIndex];
            SwapIfValid(swapIndex, emptyLocation - swapIndex);
        }
    }
}
