using System.Collections;
using UnityEngine;

public class Barricade : MonoBehaviour, IInteractable
{
    /// <summary> The barricade material when not currently in range. </summary>
    [SerializeField] protected Material unselectedMaterial;

    /// <summary> The barricade material when currently in range. </summary>
    [SerializeField] protected Material selectedMaterial;

    /// <summary> The pieces of the barricade, falling off in order from 
    /// first added to last added </summary>
    [SerializeField] protected Rigidbody[] barricadePieces;

    /// <summary> The stored start locations of the barricade pieces, 
    /// used for repairing. </summary>
    protected Vector3[] startPosition;

    /// <summary> The stored start rotations of the barricade pieces,
    /// used for repairing. </summary>
    protected Quaternion[] startRotation;

    /// <summary> The damage each piece of the barricade can take before it 
    /// falls off. </summary>
    [SerializeField] protected float healthPerPiece = 5f;

    /// <summary> The stored damage, when over healthPerPiece will destroy one 
    /// piece of the barricade </summary>
    protected float deltaDamage = 0.0f;

    /// <summary> The current position in the array to repair, destroying
    /// removes the entry one below this (0 indicates all broken) </summary>
    protected volatile int piecesLeft;

    /// <summary> Is the barricade broken? If true is able to be 
    /// passed through. </summary>
    public bool IsBroken 
    { get; private set; } = false;

    /// <summary> Is the barrier being repaired by the player? </summary>
    protected bool beingRepaired = false;

    /// <summary> Is the barricade eligible to be selected? </summary>
    protected bool isSelected = false;

    /// <summary> The time to repair one piece of the barricade. </summary>
    public float pieceRepairTime = 1f;

    /// <summary> The distance to stop dragging the board towards the object and
    /// attach it </summary>
    protected readonly float reattachDistance = 0.01f;

    protected void Start()
    {
        piecesLeft = barricadePieces.Length;

        startPosition = new Vector3[piecesLeft];
        startRotation = new Quaternion[piecesLeft];

        // initial setup of all the pieces
        for (int i = 0; i < barricadePieces.Length; i++)
        {
            // copy to preserve original
            startPosition[i] = barricadePieces[i].transform.position;
            startRotation[i] = barricadePieces[i].transform.rotation;
            barricadePieces[i].isKinematic = true;
            barricadePieces[i].useGravity = false;
        }
    }

    /// <summary> Damages the barricade by the amount specified, breaking pieces
    /// and potentially disabling it </summary>
    /// <param name="damage"> The damage dealt to the barricade. </param>
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

    /// <summary> Break a piece of the barricade off, if last is broken then 
    /// disable barricade </summary>
    protected void KnockOffPiece()
    {
        piecesLeft--;
        if (piecesLeft >= 0) 
        {
            barricadePieces[piecesLeft].isKinematic = false;
            barricadePieces[piecesLeft].useGravity = true;
            barricadePieces[piecesLeft].AddForce(Random.onUnitSphere * 100f);
        }

        if (piecesLeft <= 0)
        {
            piecesLeft = 0;
            SetBarricadeState(true);
        }
        
        // Recalculate if selected now that a piece has been removed.
        if (isSelected) {
            Selected(isSelected); 
        }

    }

    /// <summary> Toggles the barricade's collision. </summary>
    /// <param name="isBroken"> Is the barrier able to be walked through?
    /// if true collision is disabled. </param>
    private void SetBarricadeState(bool isBroken)
    {
        IsBroken = isBroken;

        Debug.Assert(
                GetComponent<Collider>() != null,
                "Could not find collider for Barricade."
            );

        GetComponent<Collider>().isTrigger = isBroken;
    }

    /// <summary> Repairs a piece of the barricade, reenables barricade if the
    /// first piece is repaired. </summary>
    protected void RepairPiece()
    {
        piecesLeft++;
        if (piecesLeft <= 1)
        {
            SetBarricadeState(false);
            piecesLeft = 1;
        }

        if (piecesLeft >= barricadePieces.Length)
        {
            piecesLeft = barricadePieces.Length;
            if (isSelected) 
            {
                Selected(isSelected);
            }
        }

        StartCoroutine(ReattachPiece(piecesLeft - 1));
    }

    /// <summary> Handles the physics of reattaching a piece by flinging it 
    /// towards it's original position and then clamping it when it gets 
    /// close enough. </summary>
    /// <param name="piece"> The index of the piece. </param>
    /// <returns> Unused. <\returns>
    protected IEnumerator ReattachPiece(int piece)
    {
        Debug.Log("Moving from "
                + barricadePieces[piece].transform.position
                + " To "
                + startPosition[piece]
            );

        barricadePieces[piece].isKinematic = true;
        barricadePieces[piece].useGravity = false;

        // FIXME: will fail if barricade piece is unable to get within distance
        // While further away than the re-attach distance
        while (
            (
                barricadePieces[piece].transform.position
                - startPosition[piece]
            ).magnitude > reattachDistance
        )
        {
            // Lerp the position and rotation towards the end goal.
            barricadePieces[piece].MovePosition(Vector3.Lerp(
                    barricadePieces[piece].transform.position,
                    startPosition[piece],
                    Time.fixedDeltaTime * 4
                ));

            barricadePieces[piece].MoveRotation(Quaternion.Lerp(
                    barricadePieces[piece].transform.rotation,
                    startRotation[piece],
                    Time.fixedDeltaTime * 4
                ));

            yield return null;
        }

        barricadePieces[piece].transform.SetPositionAndRotation(
                startPosition[piece],
                startRotation[piece]
            );
    }


    /* IInteractable Implementation */


    public MonoBehaviour GetSelf()
    {
        return this;
    }

    public string GetInteractionText()
    {
        if (!IsInteractable()) {
           return null; 
        } 
        return "Repair Barricade";
    }

    public bool IsInteractable()
    {
        return piecesLeft < barricadePieces.Length;
    }

    public void Selected(bool isSelected)
    {
        this.isSelected = isSelected;

        // modify the local version to confirm it is interactable
        isSelected &= IsInteractable();

        foreach (Rigidbody piece in barricadePieces)
        {
            Debug.Assert(
                    piece.GetComponent<Renderer>() != null,
                    "Could not find renderer for Barricade piece"
                );
            
            piece.GetComponent<Renderer>().material = 
                    isSelected 
                    ? selectedMaterial 
                    : unselectedMaterial;
        }
    }

    public IEnumerator StartInteraction()
    {
        Debug.Log("Starting Barricade Repair...");

        beingRepaired = true;

        while (piecesLeft < barricadePieces.Length)
        {
            yield return new WaitForSeconds(pieceRepairTime);
            DamageBarricade(-healthPerPiece); // negative damage repairs
            Debug.Log("Repaired a Piece of a Barricade.");
        }
        beingRepaired = false;
    }

    public bool IsBeingInteractedWith()
    {
        return beingRepaired;
    }

    public void ForceEndInteraction(Coroutine interaction)
    {
        beingRepaired = false;
        StopCoroutine(interaction);
    }
}
