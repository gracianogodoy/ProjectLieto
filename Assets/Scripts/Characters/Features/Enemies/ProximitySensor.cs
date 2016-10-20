using UnityEngine;

public class ProximitySensor : MonoBehaviour
{
    [SerializeField]
    private LayerMask _targetLayer;
    [SerializeField]
    private Sensor _readySensor;
    [SerializeField]
    private Sensor _attackSensor;

    public Sensor ReadySensor { get { return _readySensor; } }
    public Sensor AttackSensor { get { return _attackSensor; } }

    void Update()
    {
        _readySensor.CheckTarget(transform.position, _targetLayer);
        _attackSensor.CheckTarget(transform.position, _targetLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        drawSensor(_readySensor.Offset, _readySensor.Size);
        Gizmos.color = Color.red;
        drawSensor(_attackSensor.Offset, _attackSensor.Size);
    }

    private void drawSensor(float offset, Vector2 size)
    {
        var position = new Vector2(transform.position.x, transform.position.y + offset);
        Gizmos.DrawWireCube(position, size);
    }

    [System.Serializable]
    public class Sensor
    {
        public delegate void EnterSensorEvent(GameObject target);
        public delegate void LeaveSensorEvent(GameObject target);
        public delegate void InsideSensorEvent(GameObject target);

        [SerializeField]
        private float _sensorOffset;
        [SerializeField]
        private Vector2 _sensorSize;

        public float Offset { get { return _sensorOffset; } }

        public Vector2 Size { get { return _sensorSize; } }

        public EnterSensorEvent OnEnterSensor { get; set; }
        public LeaveSensorEvent OnLeaveSensor { get; set; }
        public InsideSensorEvent OnInsideSensor { get; set; }

        private GameObject _target;

        public void CheckTarget(Vector2 origin, LayerMask targetLayer)
        {
            var position = new Vector2(origin.x, origin.y + _sensorOffset);
            var hit = Physics2D.BoxCast(position, _sensorSize, 0, Vector2.right, 0, targetLayer);

            if (hit && !_target)
            {
                _target = hit.transform.gameObject;

                if (OnEnterSensor != null)
                    OnEnterSensor(_target);
            }

            if (hit)
            {
                if (OnInsideSensor != null)
                    OnInsideSensor(_target);
            }

            if (!hit && _target)
            {
                if (OnLeaveSensor != null)
                    OnLeaveSensor(_target);

                _target = null;
            }
        }
    }
}
