using Cinemachine;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Player
{
    public class CameraZoom : MonoBehaviour
    {
        [Tooltip("If set to false, the camera will be zoomed in from max zoom to min zoom")]
        [SerializeField] private bool zoomOut = true;
        [SerializeField] private float minZoom = 30f;
        [SerializeField] private float maxZoom = 50f;
        [SerializeField] private CinemachineVirtualCamera cmCam;
        [SerializeField] private Transform startPosition;
        [SerializeField] private Transform endPosition;
        [SerializeField] private Transform player;



        private CinemachineComponentBase _componentBase;
        private CinemachineFramingTransposer _transposer; //don't ask about this name...
        private float _xStart;
        private float _xEnd;
        private bool _isPlayerInside;
        private bool _enabled;

        // Start is called before the first frame update
        void Start()
        {
            _xStart = startPosition.position.x;
            _xEnd = endPosition.position.x;
            _isPlayerInside = false;
            _enabled = true;
            
            _componentBase = cmCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
            if (_componentBase is CinemachineFramingTransposer transposer)
            {
                _transposer = transposer;
            }
            else
            {
                enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_isPlayerInside)
            {
                var xPlayer = player.position.x;
                if (zoomOut) // from 30 to 50
                {
                    if (xPlayer < _xStart)
                    {
                        _transposer.m_CameraDistance = minZoom;
                    }
                    else if (xPlayer > _xEnd)
                    {
                        _transposer.m_CameraDistance = maxZoom;
                    }
                    else
                    {
                        _transposer.m_CameraDistance = minZoom +
                                                       (maxZoom - minZoom) * (player.position.x - _xStart) /
                                                       (_xEnd - _xStart);
                    }
                }
                else //zoomIn
                {
                    if (xPlayer < _xStart)
                    {
                        _transposer.m_CameraDistance = maxZoom;
                    }
                    else if (xPlayer > _xEnd)
                    {
                        _transposer.m_CameraDistance = minZoom;
                    }
                    else
                    {
                        _transposer.m_CameraDistance = maxZoom +
                                                       (minZoom - maxZoom) * (player.position.x - _xStart) /
                                                       (_xEnd - _xStart);
                    }
                }
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_enabled && other.CompareTag("Player"))
                _isPlayerInside = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (_enabled && other.CompareTag("Player"))
                _isPlayerInside = false;
        }
    }
}
