using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IndiactorManager : Singleton<IndiactorManager>
{
    [SerializeField]
    private GameObject _pointPrefab;
    [SerializeField]
    private List<Indiactor> _activatedIndiactors = new List<Indiactor>();

    protected override void Awake()
    {
        base.Awake();

        MessagingCenter.Subscribe<Projectile, Vector3>(this, Projectile.MessageOnProjectileSpawned, (sender, pos) =>
        {
            ActivateIndiactor(Indiactor.Type.Point, pos, sender.gameObject);
        });

        MessagingCenter.Subscribe<Projectile>(this, Projectile.MessageOnProjectileWillDestroy, (sender) =>
        {
            DeactiveIndiactor(sender.gameObject);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<Projectile, Vector3>(this, Projectile.MessageOnProjectileSpawned);
        MessagingCenter.Unsubscribe<Projectile>(this, Projectile.MessageOnProjectileWillDestroy);
    }

    private void ActivateIndiactor(Indiactor.Type type, Vector3 pos, GameObject owner)
    {
        Indiactor indiactor = new Indiactor(type, owner, pos);
        if (!_activatedIndiactors.Contains(indiactor))
        {
            GameObject indiactorObj = null;
            pos.y += 0.1f;
            if (type == Indiactor.Type.Point)
            {
                indiactorObj = _pointPrefab;
            }

            GameObject GO = Instantiate(indiactorObj, pos, Quaternion.Euler(90, 0, 0));
            indiactor.indiactorObj = GO;
            _activatedIndiactors.Add(indiactor);
        }
            
    }

    private void DeactiveIndiactor(GameObject owner)
    {
        Indiactor indiactor = _activatedIndiactors.First(x => x.owner == owner);
        if (indiactor != null && _activatedIndiactors.Contains(indiactor))
        {
            Destroy(indiactor.indiactorObj);
            _activatedIndiactors.Remove(indiactor);
        }         
    }
}

[System.Serializable]
public class Indiactor
{
    public enum Type
    {
        Area,
        Point
    }

    public Type type;
    public GameObject owner;
    public Vector3 position;
    public GameObject indiactorObj;

    public Indiactor(Type type, GameObject owner, Vector3 position)
    {
        this.type = type;
        this.owner = owner;
        this.position = position;
    }
}
