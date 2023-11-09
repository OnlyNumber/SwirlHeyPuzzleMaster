using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinManager : MonoBehaviour
{
    [SerializeField]
    private float RotatePower;

    [SerializeField]
    private float StopPower;

    private Rigidbody2D _rb;

    public List<Action> Rewards = new List<Action>();

    public float Angle;

    public float AngleOffset;

    public RectTransform _rectTransform;

    [SerializeField]
    private PlayerDataManager _playerDataManager;

    private bool _isRolling;

    [SerializeField]
    private GameObject _freeButton;

    private int _reward;

    private void Awake()
    {
        Debug.Log(_playerDataManager.GetDate());
        Debug.Log(DateTime.Today);

        if (DateTime.Compare(_playerDataManager.GetDate(), DateTime.Today) >= 0 && _playerDataManager.CurrentData.Spin <= 0)
        {
            _freeButton.SetActive(false);
        }

        Rewards.Add(() => _playerDataManager.TryChangeValue(ValueType.coin, 100));
        Rewards.Add(() => _playerDataManager.TryChangeValue(ValueType.gem, 100));
        Rewards.Add(() => _playerDataManager.TryChangeValue(ValueType.spin, 0));
        Rewards.Add(() => _playerDataManager.TryChangeValue(ValueType.coin, 100));
        Rewards.Add(() => _playerDataManager.TryChangeValue(ValueType.gem, 200));
        Rewards.Add(() => _playerDataManager.TryChangeValue(ValueType.spin, 0));


        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //.Log(Mathf.Abs(_rb.angularVelocity));

        if (Mathf.Abs(_rb.angularVelocity) > 1)
        {

            if (Mathf.Abs(_rb.angularVelocity) > 400)
            {
                _rb.angularVelocity -= Time.deltaTime * StopPower;

                _rb.angularVelocity = Mathf.Clamp(_rb.angularVelocity, 0, 1600);
                _isRolling = true;
            }
            else
            {
                float _rotationAngle = _rectTransform.rotation.eulerAngles.z;

                if (_rotationAngle < 0)
                {
                    _rotationAngle += 360;
                }

                if ((Angle) * _reward + AngleOffset < _rotationAngle && AngleOffset + (Angle) * (_reward + 1) > _rotationAngle)
                {
                    _rb.angularVelocity -= Time.deltaTime * StopPower * 20000000;

                    _rb.angularVelocity = Mathf.Clamp(_rb.angularVelocity, 0, 1600);
                    _isRolling = true;

                }
            }
        }

        if (_rb.angularVelocity == 0 && _isRolling)
        {
            GetReward();
            _isRolling = false;
        }
    }

    private void GetReward()
    {
        float _rotationAngle = _rectTransform.rotation.eulerAngles.z;

        if (_rotationAngle < 0)
        {
            _rotationAngle += 360;
        }

        for (int i = 0; i < Rewards.Count; i++)
        {
            if ((Angle) * i + AngleOffset < _rotationAngle && AngleOffset + (Angle) * (i + 1) > _rotationAngle)
            {
                if (Rewards[i] != null)
                {
                    Rewards[i]?.Invoke();
                    Debug.Log("Reward " + i);
                }
            }
        }

        if (DateTime.Compare(_playerDataManager.GetDate(), DateTime.Today) >= 0 && _playerDataManager.CurrentData.Spin <= 0)
        {
            _freeButton.SetActive(false);
        }

    }

    public void StartFreeRoll()
    {
        if(_isRolling)
        {
            return;
        }

        StartRotate();

        if (_playerDataManager.GetDate() < DateTime.Today)
        {
            _playerDataManager.SetDate(DateTime.Today);

            Debug.Log(_playerDataManager.CurrentData.LastDate);
            Debug.Log(DateTime.Today);

            if (_playerDataManager.CurrentData.Spin <= 0)
            {
                _freeButton.SetActive(false);
            }
            return;
        }


        if (_playerDataManager.CurrentData.Spin > 0)
        {
            _playerDataManager.TryChangeValue(ValueType.spin, -1);

            if (_playerDataManager.CurrentData.Spin <= 0)
            {
                _freeButton.SetActive(false);
            }

        }
        else
        {
            _freeButton.SetActive(false);
        }

    }

    public void StartPayRoll(int cost)
    {
        if (_isRolling)
        {
            return;
        }

        if (_playerDataManager.TryChangeValue(ValueType.coin, cost))
        {
            StartRotate();
        }
    }

    public void StartRotate()
    {
        _reward = UnityEngine.Random.Range(0, Rewards.Count);

        Debug.Log("Reward random " + _reward);

        transform.rotation = Quaternion.identity;

        _rb.AddTorque(RotatePower);
    }





}
