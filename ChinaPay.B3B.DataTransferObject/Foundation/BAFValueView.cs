namespace ChinaPay.B3B.DataTransferObject.Foundation {
    public class BAFValueView {
        public BAFValueView(decimal adult, decimal child) {
            this.Adult = adult;
            this.Child = child;
        }
        public decimal Adult { get; private set; }
        public decimal Child { get; private set; }
    }
}
