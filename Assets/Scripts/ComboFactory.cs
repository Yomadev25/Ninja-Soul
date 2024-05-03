using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class ComboFactory : MonoBehaviour
{
    public const string MessageSendComboData = "Send Combo Data";

    [SerializeField]
    private ComboGroup[] _comboGroups;
    public ComboGroup[] ComboGroups => _comboGroups;

    [Header("Effects")]
    [SerializeField]
    private GameObject _chokutoSlashFx;
    [SerializeField]
    private ParticleSystem _knucklePunchFxR;
    [SerializeField]
    private ParticleSystem _knucklePunchFxL;
    [SerializeField]
    private GameObject _knuckleStompFx;
    [SerializeField]
    private GameObject _sickleSlashFx;
    [SerializeField]
    private GameObject _swordSlashFx;
    [SerializeField]
    private GameObject _lastBladeWaveFx;
    [SerializeField]
    private GameObject _biteFx;
    [SerializeField]
    private GameObject _javelinSlashFx;
    [SerializeField]
    private ParticleSystem _javelinDashFx;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        foreach (ComboGroup comboGroup in _comboGroups)
        {
            comboGroup.isUnlocked = false;
        }
        _comboGroups[0].isUnlocked = true;

        MessagingCenter.Subscribe<GameManager>(this, GameManager.MessageWantToSelectWeapon, (sender) =>
        {
            MessagingCenter.Send(this, MessageSendComboData, _comboGroups);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<GameManager>(this, GameManager.MessageWantToSelectWeapon);
    }

    private void Start()
    {
        Player player = PlayerData.Instance.GetPlayerData();
        if (player.knuckles)
        {
            UnlockWeapon("Knuckles");
        }
        if (player.sword)
        {
            UnlockWeapon("Sword");
        }
        if (player.jevalin)
        {
            UnlockWeapon("Javelin");
        }
        if (player.sickles)
        {
            UnlockWeapon("Sickles");
        }

        _rigidBody = GetComponent<Rigidbody>();
        MessagingCenter.Send(this, MessageSendComboData, _comboGroups);
    }

    private void UnlockWeapon(string name)
    {
        _comboGroups.First(x => x.name == name).isUnlocked = true;
        MessagingCenter.Send(this, MessageSendComboData, _comboGroups);
    }

    #region CHOKUTO
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
    #endregion

    #region KNUCKLES
    public void KnucklePunch(int side)
    {
        if (side == 1)
        {
            _knucklePunchFxR.Play();
        }
        else if (side == 2)
        {
            _knucklePunchFxL.Play();
        }
        else
        {
            _knucklePunchFxR.Play();
            _knucklePunchFxL.Play();
        }

        AudioManager.Instance.PlaySFX("Punch");
    }

    public void KnuckleStomp()
    {
        Instantiate(_knuckleStompFx, transform.position, Quaternion.Euler(90, 0, 0));
        StartCoroutine(StompDamage());
    }

    IEnumerator StompDamage()
    {
        Vector3 pos = transform.position;

        for (int i = 0; i < 3; i++)
        {
            Collider[] colliders = Physics.OverlapSphere(pos, 5f);
            if (colliders.Any(x => x.CompareTag("Enemy")))
            {
                if (i == 2)
                    TimeStop.Instance.StopTime(0.05f, 10, 0.1f);
            }

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    EnemyManager enemy = collider.GetComponent<EnemyManager>();
                    enemy.TakeDamage(30);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    #endregion

    #region SICKLES
    public void SickleSlash(int combo)
    {
        GameObject slashFx = Instantiate(_sickleSlashFx, new Vector3(transform.localPosition.x, transform.position.y + 1.1f, transform.localPosition.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        switch (combo)
        {
            case 1:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -30f);
                break;
            case 2:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -145f);
                break;
            case 3:
                eulerAngle = new Vector3(0, transform.eulerAngles.y, -30f);
                break;
            case 4:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -220f);
                break;
            default:
                break;
        }

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        Destroy(slashFx, 0.5f);

        if (combo > 2)
        {
            GameObject slashFx2 = Instantiate(_sickleSlashFx, new Vector3(transform.localPosition.x, 1.1f, transform.localPosition.z) + (transform.forward * 0.6f), Quaternion.identity);
            switch (combo)
            {
                case 3:
                    eulerAngle = new Vector3(0f, transform.eulerAngles.y, -145f);
                    break;
                case 4:
                    eulerAngle = new Vector3(0f, transform.eulerAngles.y, -320f);
                    break;
                default:
                    break;
            }

            slashFx2.transform.localEulerAngles = eulerAngle;
            slashFx2.GetComponentInChildren<VisualEffect>().Play();

            Destroy(slashFx2, 0.5f);
        }
    }
    #endregion

    #region SWORD
    public void SwordSlash(int combo)
    {
        GameObject slashFx = Instantiate(_swordSlashFx, new Vector3(transform.localPosition.x, transform.position.y + 1.1f, transform.localPosition.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        switch (combo)
        {
            case 1:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -145f);
                break;
            case 2:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -145f);
                break;
            case 3:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -180f);
                break;
            case 4:
                eulerAngle = new Vector3(0f, transform.eulerAngles.y, -45f);
                Instantiate(_lastBladeWaveFx, transform.position, Quaternion.identity);
                Collider[] colliders = Physics.OverlapSphere(transform.position, 3f);
                foreach (Collider collider in colliders)
                {
                    if (collider.TryGetComponent(out EnemyManager enemy))
                    {
                        VisualEffect bite = Instantiate(_biteFx, enemy.transform.position + Vector3.up, Quaternion.identity).GetComponent<VisualEffect>();
                        bite.Play();
                        enemy.TakeDamage(20f);
                    }
                }
                break;
            default:
                break;
        }

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        Destroy(slashFx, 0.5f);
    }
    #endregion

    #region JAVELIN
    public void JavelinSlash(int combo)
    {
        GameObject slashFx = Instantiate(_javelinSlashFx, new Vector3(transform.position.x, transform.position.y + 1.1f, transform.position.z) + (transform.forward * 0.6f), Quaternion.identity);
        Vector3 eulerAngle = Vector3.zero;

        switch (combo)
        {
            case 2:
                eulerAngle = new Vector3(0, transform.eulerAngles.y, 40);
                break;
            case 3:
                eulerAngle = new Vector3(0, transform.eulerAngles.y, 220);
                break;
            default:
                break;
        }

        slashFx.transform.localEulerAngles = eulerAngle;
        slashFx.GetComponentInChildren<VisualEffect>().Play();

        slashFx.LeanScale(Vector3.one, 0.4f);

        Destroy(slashFx, 0.5f);
    }

    public void JavelinDash()
    {
        _javelinDashFx.Play();
        StartCoroutine(JavelinDashCoroutine());
    }

    private IEnumerator JavelinDashCoroutine()
    {
        float duration = 0.2f;
        Vector3 dashDirection = transform.forward.normalized;

        while (duration > 0f)
        {
            _rigidBody.AddForce(dashDirection.normalized * 10f, ForceMode.Impulse);
            duration -= Time.deltaTime;
            yield return null;
        }

        _javelinDashFx.Stop();
    }
    #endregion
}

[System.Serializable]
public class ComboGroup
{
    public string name;
    [TextArea(5, 10)]
    public string description;
    public Sprite icon;
    public GameObject[] weaponObjects;
    public Weapon[] weapons;
    public Combo[] combos;
    public bool isUnlocked;
}
