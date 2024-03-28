using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class AnotherShin : MonoBehaviour
{
    public const string MessageClearLastStage = "Clear Last Stage";

    [SerializeField]
    private Event _event;

    public enum Clans
    {
        Hikari,
        Genbu,
        Suzaku,
        Seiryu,
        Byakko,
    }

    public Clans currentClan;

    [SerializeField]
    private EnemyStateMachine _enemyStateMachine;
    [SerializeField]
    private GameObject _chokutoSlashFx;
    [SerializeField]
    private WeaponGroup[] _weaponGroups;

    private void Awake()
    {
        MessagingCenter.Subscribe<EnemyCombatState, EnemyStateMachine>(this, EnemyCombatState.MessageOnExitCombatState, (sender, state) =>
        {
            if (state != _enemyStateMachine) return;
            Invoke(nameof(RandomClan), 1f);
        });

        MessagingCenter.Subscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent, (sender, @event) =>
        {
            if (@event != _event) return;
            MessagingCenter.Send(this, MessageClearLastStage);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<EnemyCombatState, EnemyStateMachine>(this, EnemyCombatState.MessageOnExitCombatState);
        MessagingCenter.Unsubscribe<EventManager, Event>(this, EventManager.MessageOnArchievedEvent);
    }

    private void Start()
    {
        if (_event != null)
            EventManager.Instance.ActivateEvent(_event);
    }

    public void RandomClan()
    {
        currentClan = (Clans)Random.Range(0, 5);
        _enemyStateMachine.Anim.SetInteger("Clan", (int)currentClan);
        _enemyStateMachine.Anim.runtimeAnimatorController = _weaponGroups[(int)currentClan].overrideController;

        foreach (WeaponGroup weaponGroup in _weaponGroups)
        {
            foreach (GameObject weapon in weaponGroup.weapons)
            {
                weapon.SetActive(false);
            }
        }
        foreach (GameObject weapon in _weaponGroups[(int)currentClan].weapons)
        {
            weapon.SetActive(true);
        }

        _enemyStateMachine.SetWeapon(_weaponGroups[(int)currentClan].weapon);
    }

    public void ChokutoSlash(int combo)
    {
        GameObject slashFx = Instantiate(_chokutoSlashFx, new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        switch (combo)
        {
            case 1:
                eulerAngle = new Vector3(0, transform.eulerAngles.y, 45);
                break;
            case 2:
                eulerAngle = new Vector3(0, transform.eulerAngles.y, -90);
                break;
            case 3:
                eulerAngle = new Vector3(0, transform.eulerAngles.y, 0);
                break;
            default:
                break;
        }

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        Destroy(slashFx, 0.5f);
    }

    [System.Serializable]
    public class WeaponGroup
    {
        public GameObject[] weapons;
        public Weapon weapon;
        public AnimatorOverrideController overrideController;
    }
}
