using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinimapHud : MonoBehaviour
{
    [Header("Map")]
    [SerializeField]
    private RectTransform _mapMarkerParentRectTransform;
    [SerializeField]
    private RawImage _mapImage;
    [SerializeField]
    private RectTransform _playerIconMapRectTransform;
    [SerializeField]
    private TMP_Text _mapNameText;

    [Header("Icons")]
    [SerializeField]
    private GameObject _interactMarkerIcon;
    [SerializeField]
    private GameObject _portalIcon;
    [SerializeField]
    private GameObject _enemyMarkerIcon;

    private Camera _mapCamera;
    private Transform _playerTransform;
    private List<(Interact interactivePosition, RectTransform markerRectTransform)> _currentMapInteractiveObjects = new();
    private List<(EnemyManager enemyPosition, RectTransform markerRectTransform)> _currentEnemyInteractiveObjects = new();

    [Header("Zooming")]
    [SerializeField]
    private float _zoomSpeed = 0.1f;
    [SerializeField]
    private float _maxZoom = 10f;
    private Vector3 _initialScale;

    private void Awake()
    {
        _initialScale = _mapImage.rectTransform.localScale;

        MessagingCenter.Subscribe<Minimap, Camera>(this, Minimap.MessageInitMapCamera, (sender, camera) =>
        {
            InitMapCamera(camera);
            _mapNameText.text = sender.mapName;
        });

        MessagingCenter.Subscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyAppeared, (sender) =>
        {
            AddEnemyMarkerInMap(sender);
        });

        MessagingCenter.Subscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyDead, (sender) =>
        {
            RemoveEnemyMarkerInMap(sender);
        });
    }

    private void OnDestroy()
    {
        MessagingCenter.Unsubscribe<Minimap, Camera>(this, Minimap.MessageInitMapCamera);
        MessagingCenter.Unsubscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyAppeared);
        MessagingCenter.Unsubscribe<EnemyManager>(this, EnemyManager.MessageOnEnemyDead);
    }

    private void Start()
    {
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        InitPlayerTransform(playerTransform);
        InitInteractiveMarkerDataInMap();
    }

    private void Update()
    {
        if (_mapCamera == null) return;
        FollowPlayerTargetInMap();
        UpdateEnemyMarkerPosition();

        if (Input.mouseScrollDelta.y != 0)
        {
            OnScroll(Input.mouseScrollDelta.y);
        }
    }

    private void InitPlayerTransform(Transform playerTransform) => _playerTransform = playerTransform;
    private void InitMapCamera(Camera camera)
    {
        _mapCamera = camera;
        _mapImage.texture = camera.targetTexture;
        _mapMarkerParentRectTransform.localRotation = Quaternion.Euler(0, 0, 90f);
    }

    private void InitInteractiveMarkerDataInMap()
    {
        var interactObjs = FindObjectsOfType<Interact>();
        foreach (var interactObj in interactObjs)
        {
            if (interactObj.hideFromMinimap) return;

            //Assign icon follow interactive type
            GameObject icon = _interactMarkerIcon;
            if (interactObj.TryGetComponent(out Portal portal))
            {
                icon = _portalIcon;
            }

            //Spawn icon
            GameObject GO = Instantiate(icon, _mapMarkerParentRectTransform);
            RectTransform rectTransform = GO.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
            _currentMapInteractiveObjects.Add((interactObj, rectTransform));

            //Active prefab
            GO.SetActive(true);

            //Init icon position
            if (_mapCamera == null)
            {
                Debug.LogWarning(nameof(_mapCamera) + "is not exist.");
                return;
            }

            foreach ((Interact interactivePosition, RectTransform markerRectTransform) marker in _currentMapInteractiveObjects)
            {
                Vector3 offset = marker.interactivePosition.transform.position - _mapCamera.transform.position;
                offset = offset / _mapCamera.orthographicSize * (_mapMarkerParentRectTransform.rect.height / 2);
                marker.markerRectTransform.anchoredPosition = new Vector2(offset.x, offset.z);
            }
        }
    }

    private void RemoveInteractiveMarkerInMap(Interact interactObj)
    {
        if (!_currentMapInteractiveObjects.Exists(interactive => interactive.interactivePosition == interactObj))
            return;

        (Interact pos, RectTransform rectTrans) foundObj = _currentMapInteractiveObjects.Find(interactive => interactive.interactivePosition == interactObj);
        Destroy(foundObj.rectTrans.gameObject);
        _currentMapInteractiveObjects.Remove(foundObj);
    }

    private void AddEnemyMarkerInMap(EnemyManager enemyObj)
    {
        GameObject icon = _enemyMarkerIcon;

        GameObject GO = Instantiate(icon, _mapMarkerParentRectTransform);
        RectTransform rectTransform = GO.GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
        _currentEnemyInteractiveObjects.Add((enemyObj, rectTransform));

        GO.SetActive(true);

        if (_mapCamera == null)
        {
            Debug.LogWarning(nameof(_mapCamera) + "is not exist.");
            return;
        }
    }

    private void UpdateEnemyMarkerPosition()
    {
        foreach ((EnemyManager enemyPosition, RectTransform markerRectTransform) marker in _currentEnemyInteractiveObjects)
        {
            Vector3 offset = marker.enemyPosition.transform.position - _mapCamera.transform.position;
            offset = offset / _mapCamera.orthographicSize * (_mapMarkerParentRectTransform.rect.height / 2);
            marker.markerRectTransform.anchoredPosition = new Vector2(offset.x, offset.z);
        }
    }

    private void RemoveEnemyMarkerInMap(EnemyManager enemyObj)
    {
        if (!_currentEnemyInteractiveObjects.Exists(enemy => enemy.enemyPosition == enemyObj))
            return;

        (EnemyManager pos, RectTransform rectTrans) foundObj = _currentEnemyInteractiveObjects.Find(enemy => enemy.enemyPosition == enemyObj);
        Destroy(foundObj.rectTrans.gameObject);
        _currentEnemyInteractiveObjects.Remove(foundObj);
    }

    private void FollowPlayerTargetInMap()
    {
        if (_playerTransform == null)
        {
            Debug.LogWarning(nameof(_playerTransform) + "is not exist.");
            return;
        }

        Vector3 offset = _playerTransform.position - _mapCamera.transform.position;
        offset = offset / (_mapCamera.orthographicSize) * (_mapMarkerParentRectTransform.rect.height / 2);

        _playerIconMapRectTransform.anchoredPosition = new Vector2(offset.x, offset.z);
    }

    public void OnScroll(float scrollDelta)
    {
        var delta = Vector3.one * (scrollDelta * _zoomSpeed);
        var desiredScale = _mapImage.rectTransform.localScale + delta;
        desiredScale = ClampDesiredScale(desiredScale);
        _mapImage.rectTransform.localScale = desiredScale;
    }

    private Vector3 ClampDesiredScale(Vector3 desiredScale)
    {
        desiredScale = Vector3.Max(_initialScale, desiredScale);
        desiredScale = Vector3.Min(_initialScale * _maxZoom, desiredScale);
        return desiredScale;
    }
}
