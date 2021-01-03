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
        [SerializeField] private Transform start;
        [SerializeField] private Transform end;
        [SerializeField] private Transform player;



        private CinemachineComponentBase _componentBase;
        private CinemachineFramingTransposer _transposer; //don't ask about this name...
        private float _xStart;
        private float _xEnd;
        private bool _isPlayerInside;
        private bool _enabled;

        private Vector3 _startPos;
        private Vector3 _endPos;
        private Vector3 _line;
        private Vector3 _startOnLine;
        private Vector3 _endOnLine;
        private Vector3 _playerOnLine;

        // Start is called before the first frame update
        void Start()
        {
            // _xStart = start.position.x;
            // _xEnd = end.position.x;
            _isPlayerInside = false;
            _enabled = true;

            _startPos = start.position;
            _endPos = end.position;
            _line = _endPos - _startPos;
            _startOnLine =  Vector3.Project(_startPos - _startPos, _line);
            _endOnLine =  Vector3.Project(_endPos - _startPos, _line);
            
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
                var playerOnLine = Vector3.Project(player.position - _startPos, _line);
                // Debug.DrawLine(Vector3.one,Vector3.one + line, Color.yellow);
                // Debug.DrawLine(Vector3.one, Vector3.one + playerOnLine, Color.green);
                
                if (zoomOut)
                {
                    if ((playerOnLine - _endOnLine).magnitude > (_startOnLine - _endOnLine).magnitude)
                    {
                        _transposer.m_CameraDistance = minZoom;
                    }
                    else if ((playerOnLine - _startOnLine).magnitude > (_startOnLine - _endOnLine).magnitude)
                    {
                        _transposer.m_CameraDistance = maxZoom;
                    }
                    else
                    {
                        _transposer.m_CameraDistance = minZoom +
                                                       (maxZoom - minZoom) * (playerOnLine - _startOnLine).magnitude /
                                                       (_endOnLine - _startOnLine).magnitude;
                    }
                }
                else
                {
                    if ((playerOnLine - _endOnLine).magnitude > (_startOnLine - _endOnLine).magnitude)
                    {
                        _transposer.m_CameraDistance = maxZoom;
                    }
                    else if ((playerOnLine - _startOnLine).magnitude > (_startOnLine - _endOnLine).magnitude)
                    {
                        _transposer.m_CameraDistance = minZoom;
                    }
                    else
                    {
                        _transposer.m_CameraDistance = maxZoom +
                                                       (minZoom - maxZoom) * (playerOnLine - _startOnLine).magnitude /
                                                       (_endOnLine - _startOnLine).magnitude;
                    }
                }
                //
                // var xPlayer = player.position.x;
                // if (zoomOut) // from 30 to 50
                // {
                //     if (xPlayer < _xStart)
                //     {
                //         _transposer.m_CameraDistance = minZoom;
                //     }
                //     else if (xPlayer > _xEnd)
                //     {
                //         _transposer.m_CameraDistance = maxZoom;
                //     }
                //     else
                //     {
                //         _transposer.m_CameraDistance = minZoom +
                //                                        (maxZoom - minZoom) * (player.position.x - _xStart) /
                //                                        (_xEnd - _xStart);
                //     }
                // }
                // else //zoomIn
                // {
                //     if (xPlayer < _xStart)
                //     {
                //         _transposer.m_CameraDistance = maxZoom;
                //     }
                //     else if (xPlayer > _xEnd)
                //     {
                //         _transposer.m_CameraDistance = minZoom;
                //     }
                //     else
                //     {
                //         _transposer.m_CameraDistance = maxZoom +
                //                                        (minZoom - maxZoom) * (player.position.x - _xStart) /
                //                                        (_xEnd - _xStart);
                //     }
                // }
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
