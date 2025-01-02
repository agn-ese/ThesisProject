using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour
{
    [SerializeField] private GameObject currentHitObject;
    [SerializeField] private Transform _originTransform;
    private Vector3 _origin;
    private Vector3 _direction;
    public float maxRadius;
    public float MaxDist;
    public float coneAngle;
    public static bool _hit = false;
    private GameObject hitObject;
    private List<GameObject> hitObjects = new List<GameObject>();
    public RaycastHit[] coneHits;
    private Physics physics;
    public static bool first = true;
    private ChangeTarget changeTarget;
    private SpatialAwarenessManager spatialAwarenessManager;


    // Start is called before the first frame update
    void Start()
    {
        //_originTransform = GameObject.FindObjectOfType<Camera>().transform;
        changeTarget = GameObject.FindObjectOfType<ChangeTarget>();
        spatialAwarenessManager = GameObject.FindObjectOfType<SpatialAwarenessManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        _origin = _originTransform.position;
        _direction = _originTransform.forward;
        coneHits = physics.coneCast(_origin, maxRadius, _direction, MaxDist, coneAngle, LayerMask.GetMask("Useful"));

        
        if (coneHits.Length > 0)
        {
            foreach (RaycastHit hit in coneHits)
            {

                currentHitObject = hit.transform.gameObject;
                hitObjects.Add(currentHitObject);
                if (hitObject != currentHitObject)
                {
                    _hit = false;
                } 
                hitObject = currentHitObject;
            }
            Debug.Log(_hit + " " + first + " " + hitObject + " " + currentHitObject);
            _hit = true;
        }
        else
        {
            hitObjects.Clear();
            if ((Vector3.Distance(Camera.main.transform.position, changeTarget.GetTarget().position) < (1.7f*2)+1) && (Vector3.Angle(Camera.main.transform.forward, changeTarget.GetTarget().position - Camera.main.transform.position) < 80) && spatialAwarenessManager.NeverLooked == false)
                    _hit = true;
            else
                    _hit = false;

        }
    }

    public List<GameObject> getHits()
    {
        return hitObjects;
    }

    public GameObject getHitObject() { return currentHitObject; }

    /*void OnDrawGizmos()
{
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(_origin, maxRadius);
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(_origin + _direction * MaxDist, maxRadius);
}
    */
}
