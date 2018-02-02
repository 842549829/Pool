using System;

namespace ChinaPay.B3B.DataTransferObject
{
    public static class RefreshService
    {
        public static event Action BunkChaned;
        public static event Action BasePriceChanged;
        public static event Action BAFChanged;
        public static event Action ServicePhoneChanged;
        public static event Action<Guid> OEMSettingChanged;
        public static event Action<Guid> OEMStyleSetChanged;
        public static event Action OEMAdded;

        public static void FlushBunk() {
            if (BunkChaned!=null)  BunkChaned(); 
        }

        public static void FlushBasePrice()
        {
            if (BasePriceChanged != null)  BasePriceChanged(); 
        }

        public static void FlushBAF() {
            if (BAFChanged != null) BAFChanged();
        }

        public static void FlushServicePhone() {
            if (ServicePhoneChanged != null) ServicePhoneChanged();
        }

        public static void FlushOEM(Guid oemId) {
            if (OEMSettingChanged != null) OEMSettingChanged(oemId);
        }

        public static void FlushStyles(Guid styleId) {
            if (OEMStyleSetChanged != null) OEMStyleSetChanged(styleId);
        }

        public static void FlushOEMErrorCache() {
            if (OEMAdded != null) OEMAdded();
        }
    }
}