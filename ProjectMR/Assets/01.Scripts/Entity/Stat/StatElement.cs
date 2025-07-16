using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace ProjectMR.Stat
{
    public enum EModifyMode
    {
        Add,
        Percent,
    }

    public enum EModifyLayer
    {
        StatUp,
        Default,
        Last,
    }

    public struct StatModifier
    {
        private float _originValue;
        private BigInteger _bigIntOriginValue;
        private bool _canValueOverlap;

        private int _overlapCount;
        public EModifyMode Mode { get; private set; }
        public float Value { get; private set; }
        public BigInteger BigIntValue { get; private set; }

        public bool IsBigInteger { get; private set; }

        public StatModifier(float originValue, EModifyMode mode, bool canValueOverlap)
        {
            _originValue = originValue;
            _bigIntOriginValue = 0;
            Mode = mode;
            _overlapCount = 1;
            _canValueOverlap = canValueOverlap;

            Value = originValue;
            BigIntValue = 0;
            IsBigInteger = false;
        }
        public StatModifier(BigInteger originValue, EModifyMode mode, bool canValueOverlap)
        {
            _originValue = 0;
            _bigIntOriginValue = originValue;
            Mode = mode;
            _overlapCount = 1;
            _canValueOverlap = canValueOverlap;

            Value = 0;
            BigIntValue = originValue;
            IsBigInteger = true;
        }

        public static StatModifier operator ++(StatModifier modifier)
        {
            modifier._overlapCount++;
            if (modifier._canValueOverlap)
            {
                if (modifier.IsBigInteger)
                    modifier.BigIntValue += modifier._bigIntOriginValue;
                else
                    modifier.Value += modifier._originValue;
            }
            return modifier;
        }
        public static StatModifier operator --(StatModifier modifier)
        {
            modifier._overlapCount--;
            if (modifier._canValueOverlap)
            {
                if (modifier.IsBigInteger)
                    modifier.BigIntValue -= modifier._bigIntOriginValue;
                else
                    modifier.Value -= modifier._originValue;
            }
            return modifier;
        }
        public static implicit operator int(StatModifier modifier)
        {
            return modifier._overlapCount;
        }
    }


    [Serializable]
    public class StatElement : ICloneable
    {
        [HideInInspector] public string Name;
        public StatElementSO elementSO;
        private bool _IsBigInteger => elementSO.isBigInteger;

        [SerializeField] private string _valueString;
        private BigInteger _bigIntBaseValue;
        private float _baseValue;

        private Dictionary<EModifyLayer, Dictionary<string, StatModifier>> _modifiers;

        public event Action<float, float> OnValueChanged;
        public event Action<int, int> OnIntValueChanged;
        public event Action<BigInteger, BigInteger> OnBigIntValueChanged;

        public float Value { get; private set; }
        public int IntValue { get; private set; }
        public BigInteger BigIntValue { get; private set; }

        private bool _isUseClamp;
        private bool _isUseModifier;

        public void Initialize(bool isUseClamp = true, bool isUseModifier = true)
        {
            _isUseClamp = isUseClamp;
            _isUseModifier = isUseModifier;

            if (_IsBigInteger) _bigIntBaseValue = BigInteger.Parse(_valueString);
            else _baseValue = float.Parse(_valueString);

            SetDictionary();
            SetValue();
        }

        private void SetDictionary()
        {
            _modifiers ??= new Dictionary<EModifyLayer, Dictionary<string, StatModifier>>()
            {
                { EModifyLayer.StatUp, new Dictionary<string, StatModifier>() },
                { EModifyLayer.Default, new Dictionary<string, StatModifier>() },
                { EModifyLayer.Last, new Dictionary<string, StatModifier>() },
            };
        }

        private void SetValue()
        {
            // BigInteger전용
            if (_IsBigInteger)
            {
                BigInteger value = _bigIntBaseValue;
                if (_isUseModifier)
                {
                    foreach (var modifier in _modifiers.Values)
                    {
                        BigInteger bigIntTotalAddModifier = 0;
                        float bigIntTotalPercentModifier = 0;
                        foreach (var statModifier in modifier.Values)
                        {
                            switch (statModifier.Mode)
                            {
                                case EModifyMode.Add:
                                    {
                                        bigIntTotalAddModifier += statModifier.BigIntValue;
                                        bigIntTotalAddModifier += (BigInteger)statModifier.Value;
                                    }
                                    break;
                                case EModifyMode.Percent:
                                    {
                                        bigIntTotalPercentModifier += (float)statModifier.BigIntValue;
                                        bigIntTotalPercentModifier += statModifier.Value;
                                    }
                                    break;
                            }
                        }
                        value = (value + bigIntTotalAddModifier) * (BigInteger)((100 + bigIntTotalPercentModifier) * 100) / 10000;
                    }
                }

                if (BigIntValue != value) OnBigIntValueChanged?.Invoke(BigIntValue, value);

                BigIntValue = value;
            }
            // 일반 연산
            else
            {
                float value = _baseValue;
                if (_isUseModifier)
                {
                    foreach (var modifier in _modifiers.Values)
                    {
                        float totalAddModifier = 0;
                        float totalPercentModifier = 0;
                        foreach (var statModifier in modifier.Values)
                        {
                            switch (statModifier.Mode)
                            {
                                case EModifyMode.Add:
                                    totalAddModifier += statModifier.Value;
                                    break;
                                case EModifyMode.Percent:
                                    totalPercentModifier += statModifier.Value;
                                    break;
                            }
                        }
                        value = (value + totalAddModifier) * (1 + totalPercentModifier / 100);
                    }
                }

                if (elementSO != null && _isUseClamp)
                    value = Mathf.Clamp(value, elementSO.minMaxValue.x, elementSO.minMaxValue.y);

                int intValue = Mathf.CeilToInt(value);

                if (Value != value) OnValueChanged?.Invoke(Value, value);
                if (IntValue != intValue) OnIntValueChanged?.Invoke(IntValue, intValue);

                Value = value;
                IntValue = intValue;
            }
        }

        public void AddModify(string key, float value, EModifyMode eModifyMode, EModifyLayer eModifyLayer, bool canValueOverlap = true)
        {
            StatModifier modifier = new StatModifier(value, eModifyMode, canValueOverlap);
            if (_modifiers[eModifyLayer].ContainsKey(key))
            {
                if (canValueOverlap)
                    _modifiers[eModifyLayer][key]++;
                else
                    _modifiers[eModifyLayer][key] = modifier;
            }
            else
            {
                _modifiers[eModifyLayer][key] = modifier;
            }

            SetValue();
        }
        public void AddModify(string key, BigInteger value, EModifyMode eModifyMode, EModifyLayer eModifyLayer, bool canValueOverlap = true)
        {
            StatModifier modifier = new StatModifier(value, eModifyMode, canValueOverlap);
            if (_modifiers[eModifyLayer].ContainsKey(key))
            {
                if (canValueOverlap)
                    _modifiers[eModifyLayer][key]++;
                else
                    _modifiers[eModifyLayer][key] = modifier;
            }
            else
            {
                _modifiers[eModifyLayer][key] = modifier;
            }

            SetValue();
        }
        public void RemoveModify(string key, EModifyLayer eModifyLayer)
        {
            if (_modifiers[eModifyLayer].ContainsKey(key))
            {
                _modifiers[eModifyLayer][key]--;
                if (_modifiers[eModifyLayer][key] == 0)
                    _modifiers[eModifyLayer].Remove(key);
                SetValue();
            }
            else
                Debug.LogWarning($"[{key}]Key not found for statModifier");
        }

        public object Clone()
        {
            StatElement clonedStatElement = (StatElement)MemberwiseClone();
            clonedStatElement._modifiers = new Dictionary<EModifyLayer, Dictionary<string, StatModifier>>();
            return clonedStatElement;
        }
    }
}
