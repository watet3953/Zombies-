using System.Collections;
using UnityEngine;

public class Barricade : MonoBehaviour, IInteractable
{

    /// <summary> The pieces of the barricade, falling off in
    /// order from first added to last added </summary>
    [SerializeField] protected Rigidbody[] barricadePieces;

    /* The stored start locations of the barricade pieces,
    used for repairing */
    private Transform[] startLocations;

    /// <summary> The damage each piece of the barricade can take
    /// before it falls off. </summary>
    [SerializeField] private float healthPerPiece = 5f;

    /* The stored damage, when over healthPerPiece will destroy
    one piece of the barricade */
    private float deltaDamage = 0.0f;

    /* The current position in the array to repair, destroying
    removes the entry one below this (0 indicates all broken) */
    private volatile int piecesLeft;

    /// <summary> Is the barricade broken? If true is able to be
    /// passed through. </summary>
    public bool IsBroken { get; private set; } = false;

    public float pieceRepairTime = 1f;

    /* The distance to stop dragging the board towards the object
    and attach it */
    private readonly float reattachDistance = 1.0f;

    private void Start()
    {
        piecesLeft = barricadePieces.Length;

        startLocations = new Transform[piecesLeft];

        // initial setup of all the pieces
        for (int i = 0; i < barricadePieces.Length; i++)
        {
            // copy to preserve original
            startLocations[i] = barricadePieces[i].transform; 
            barricadePieces[i].isKinematic = true;
            barricadePieces[i].useGravity = false;
        }
    }

    /// <summary> Damages the barricade by the amount specified,
    /// breaking pieces and potentially disabling it </summary>
    /// <param name="damage"> The amount of damage dealt to the
    /// barricade</param>
    public void DamageBarricade(float damage)
    {
        deltaDamage += damage;
        while (deltaDamage > healthPerPiece)
        {
            KnockOffPiece();
            deltaDamage -= healthPerPiece;
        }
        while (deltaDamage < 0)
        {
            RepairPiece();
            deltaDamage += healthPerPiece;
        }
    }

    /// <summary> Break a piece of the barricade off, if last is
    /// broken then disable barricade </summary>
    private void KnockOffPiece()
    {
        piecesLeft--;
        if (piecesLeft <= 0)
        {
            piecesLeft = 0;

            barricadePieces[0].isKinematic = false;
            barricadePieces[0].useGravity = true;

            SetBarricadeState(true);

            return;
        }
        barricadePieces[piecesLeft].isKinematic = false;
        barricadePieces[piecesLeft].useGravity = true;

    }

    /// <summary> Toggles the barricade's collision. </summary>
    /// <param name="isBroken"> Is the barrier able to be walked
    /// through? if true collision is disabled. </param>
    private void SetBarricadeState(bool isBroken)
    {
        this.IsBroken = isBroken;
        GetComponent<BoxCollider>().enabled = !isBroken;
    }

    /// <summary> Repairs a piece of the barricade, reenables
    /// barricade if the first piece is repaired. </summary>
    private void RepairPiece()
    {
        piecesLeft++;
        if (piecesLeft <= 1)
        {
            SetBarricadeState(false);
            piecesLeft = 1;
        }

        if (piecesLeft >= barricadePieces.Length) {
            piecesLeft = barricadePieces.Length;
        }

        ReattachPiece(piecesLeft - 1);
    }

    /// <summary> Handles the physics of reattaching a piece
    /// by flinging it towards it's original position and then 
    /// clamping it when it gets close enough. </summary>
    /// <param name="piece"> The index of the piece. </param>
    private IEnumerable ReattachPiece(int piece)
    {
        while ((barricadePieces[piece].transform.position
                - startLocations[piece].position).magnitude
                < reattachDistance)
        {
            barricadePieces[piece].MovePosition(Vector3.Lerp(
                    barricadePieces[piece].transform.position,
                    startLocations[piece].position,
                    Time.fixedDeltaTime * 4));
            yield return null;
        }

        barricadePieces[piece].transform.SetPositionAndRotation(
                startLocations[piece].position,
                startLocations[piece].rotation);

        barricadePieces[piece].isKinematic = true;
        barricadePieces[piece].useGravity = false;
    }


    public string GetInteractionText()
    {
        if (piecesLeft >= barricadePieces.Length) return null;
        return "Repair Barricade";
    }

    public bool IsInteractable() 
    { 
        return piecesLeft < barricadePieces.Length; 
    }

    public IEnumerable StartInteraction()
    {
        // wait the number of seconds set to repair
        // heal by the amount needed to repair one board
        // if all fixed, end safely
        yield return null;
    }

    public void ForceEndInteraction()
    {
        
    }
}
