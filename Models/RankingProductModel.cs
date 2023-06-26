namespace ChannelEngineAssignment.Models {
    public class RankingProductModel {
        public string Name { get; set; }
        public string Gtin { get; set; }
        public int Quantity { get; set; }

        public RankingProductModel(string name, string gtin, int quantity) {
            this.Name = name;
            this.Gtin = gtin;
            this.Quantity = quantity;
        }
    }
}
