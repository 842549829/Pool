namespace ChinaPay.B3B.DataTransferObject.Common {
    //using System;
    //using System.Runtime.InteropServices;

    ///// <summary>
    ///// 货币类
    ///// </summary>
    //[Serializable]
    //[ComVisible(true)]
    //public struct Currency : IComparable, IComparable<Currency>, IEquatable<Currency> {
    //    private decimal _amount;
    //    //private CurrencyUnit _unit;
    //    public Currency(decimal value) {
    //        _amount = value;
    //        //_unit = CurrencyUnit.RMB;
    //    }
    //    public Currency(double value) {
    //        _amount = (decimal)value;
    //        //_unit = CurrencyUnit.RMB;
    //    }
    //    public Currency(float value) {
    //        _amount = (decimal)value;
    //        //_unit = CurrencyUnit.RMB;
    //    }
    //    public Currency(int value) {
    //        _amount = (decimal)value;
    //        //_unit = CurrencyUnit.RMB;
    //    }
    //    public Currency(long value) {
    //        _amount = (decimal)value;
    //        //_unit = CurrencyUnit.RMB;
    //    }
    //    public Currency(uint value) {
    //        _amount = (decimal)value;
    //        //_unit = CurrencyUnit.RMB;
    //    }
    //    public Currency(ulong value) {
    //        _amount = (decimal)value;
    //        // _unit = CurrencyUnit.RMB;
    //    }
    //    public decimal Value {
    //        get { return _amount; }
    //    }
    //    public static Currency operator -(Currency left, Currency right) {
    //        return new Currency(left._amount - right._amount);
    //    }
    //    public static bool operator !=(Currency left, Currency right) {
    //        return left._amount != right._amount; //|| c1._unit != c2._unit;
    //    }
    //    public static Currency operator +(Currency left, Currency right) {
    //        return new Currency(left._amount + right._amount);
    //    }
    //    public static bool operator <(Currency left, Currency right) {
    //        return left._amount < right._amount;
    //    }
    //    public static bool operator <=(Currency left, Currency right) {
    //        return left._amount <= right._amount;
    //    }
    //    public static bool operator ==(Currency left, Currency right) {
    //        return left._amount == right._amount;// && c1._unit == c2._unit;
    //    }
    //    public static bool operator >(Currency left, Currency right) {
    //        return left._amount > right._amount;
    //    }
    //    public static bool operator >=(Currency left, Currency right) {
    //        return left._amount >= right._amount;
    //    }
    //    public Currency Muliply(decimal value) {
    //        return Muliply(value, CalculationMode.Round);
    //    }
    //    public Currency Muliply(decimal value, CalculationMode mode) {
    //        return Muliply(value, mode, -2);
    //    }
    //    public Currency Muliply(decimal value, CalculationMode mode, int digits) {
    //        var result = _amount * value;
    //        switch (mode) {
    //            case CalculationMode.Celling:
    //                return ChinaPay.Utility.Calculator.Ceiling(result, digits);
    //            case CalculationMode.Floor:
    //                return ChinaPay.Utility.Calculator.Floor(result, digits);
    //            default:
    //                return ChinaPay.Utility.Calculator.Round(result, digits);
    //        }
    //    }
    //    public Currency Muliply(double value) {
    //        return Muliply(value, CalculationMode.Round);
    //    }
    //    public Currency Muliply(double value, CalculationMode mode) {
    //        return Muliply(value, mode, -2);
    //    }
    //    public Currency Muliply(double value, CalculationMode mode, int digits) {
    //        var result = _amount * (decimal)value;
    //        switch (mode) {
    //            case CalculationMode.Celling:
    //                return ChinaPay.Utility.Calculator.Ceiling(result, digits);
    //            case CalculationMode.Floor:
    //                return ChinaPay.Utility.Calculator.Floor(result, digits);
    //            default:
    //                return ChinaPay.Utility.Calculator.Round(result, digits);
    //        }
    //    }
    //    public Currency Muliply(float value) {
    //        return Muliply(value, CalculationMode.Round);
    //    }
    //    public Currency Muliply(float value, CalculationMode mode) {
    //        return Muliply(value, mode, -2);
    //    }
    //    public Currency Muliply(float value, CalculationMode mode, int digits) {
    //        var result = _amount * (decimal)value;
    //        switch (mode) {
    //            case CalculationMode.Celling:
    //                return ChinaPay.Utility.Calculator.Ceiling(result, digits);
    //            case CalculationMode.Floor:
    //                return ChinaPay.Utility.Calculator.Floor(result, digits);
    //            default:
    //                return ChinaPay.Utility.Calculator.Round(result, digits);
    //        }
    //    }
    //    public Currency Muliply(long value) {
    //        return new Currency(_amount * value);
    //    }
    //    public Currency Muliply(int value) {
    //        return new Currency(_amount * value);
    //    }
    //    public Currency Muliply(uint value) {
    //        return new Currency(_amount * value);
    //    }
    //    public Currency Divide(decimal value) {
    //        return Divide(value, CalculationMode.Round);
    //    }
    //    public Currency Divide(decimal value, CalculationMode mode) {
    //        return Divide(value, mode, -2);
    //    }
    //    public Currency Divide(decimal value, CalculationMode mode, int digits) {
    //        var result = _amount / value;
    //        switch (mode) {
    //            case CalculationMode.Celling:
    //                return ChinaPay.Utility.Calculator.Ceiling(result, digits);
    //            case CalculationMode.Floor:
    //                return ChinaPay.Utility.Calculator.Floor(result, digits);
    //            default:
    //                return ChinaPay.Utility.Calculator.Round(result, digits);
    //        }
    //    }
    //    public Currency Divide(double value) {
    //        return Divide(value, CalculationMode.Round);
    //    }
    //    public Currency Divide(double value, CalculationMode mode) {
    //        return Divide(value, mode, -2);
    //    }
    //    public Currency Divide(double value, CalculationMode mode, int digits) {
    //        var result = _amount / (decimal)value;
    //        switch (mode) {
    //            case CalculationMode.Celling:
    //                return ChinaPay.Utility.Calculator.Ceiling(result, digits);
    //            case CalculationMode.Floor:
    //                return ChinaPay.Utility.Calculator.Floor(result, digits);
    //            default:
    //                return ChinaPay.Utility.Calculator.Round(result, digits);
    //        }
    //    }
    //    public Currency Divide(float value) {
    //        return Divide(value, CalculationMode.Round);
    //    }
    //    public Currency Divide(float value, CalculationMode mode) {
    //        return Divide(value, mode, -2);
    //    }
    //    public Currency Divide(float value, CalculationMode mode, int digits) {
    //        var result = _amount / (decimal)value;
    //        switch (mode) {
    //            case CalculationMode.Celling:
    //                return ChinaPay.Utility.Calculator.Ceiling(result, digits);
    //            case CalculationMode.Floor:
    //                return ChinaPay.Utility.Calculator.Floor(result, digits);
    //            default:
    //                return ChinaPay.Utility.Calculator.Round(result, digits);
    //        }
    //    }
    //    public Currency Divide(long value) {
    //        return new Currency(_amount / value);
    //    }
    //    public Currency Divide(int value) {
    //        return new Currency(_amount / value);
    //    }
    //    public Currency Divide(uint value) {
    //        return new Currency(_amount / value);
    //    }
    //    public int CompareTo(object obj) {
    //        if (obj == null) return 1;
    //        if (obj is Currency) {
    //            return this.CompareTo((Currency)obj);
    //        }
    //        throw new ArgumentException("对象的类型必须是 Currency");
    //    }
    //    public int CompareTo(Currency other) {
    //        if (this._amount == other._amount)
    //            return 0;
    //        else if (this._amount > other._amount)
    //            return 1;
    //        else
    //            return -1;
    //    }
    //    public bool Equals(Currency other) {
    //        return this._amount.Equals(other._amount);
    //    }
    //    public override int GetHashCode() {
    //        return this._amount.GetHashCode();
    //    }
    //    public override bool Equals(object obj) {
    //        if (obj != null && obj is Currency) {
    //            return this._amount.Equals(((Currency)obj)._amount);
    //        }
    //        return false;
    //    }
    //    public override string ToString() {
    //        return this._amount.ToString("F2");
    //    }
    //    public string ToNormalString() {
    //        return this._amount.ToString("N2");
    //    }
    //    public static implicit operator Currency(decimal value) {
    //        return new Currency(value);
    //    }
    //    public static implicit operator Currency(double value) {
    //        return new Currency(value);
    //    }
    //    public static implicit operator Currency(float value) {
    //        return new Currency(value);
    //    }
    //    public static implicit operator Currency(int value) {
    //        return new Currency(value);
    //    }
    //    public static implicit operator Currency(long value) {
    //        return new Currency(value);
    //    }
    //    public static implicit operator Currency(uint value) {
    //        return new Currency(value);
    //    }
    //    public static implicit operator Currency(ulong value) {
    //        return new Currency(value);
    //    }
    //}
    ////public enum CurrencyUnit{
    ////    RMB
    ////}
    //public enum CalculationMode {
    //    Round,
    //    Celling,
    //    Floor
    //}
}
