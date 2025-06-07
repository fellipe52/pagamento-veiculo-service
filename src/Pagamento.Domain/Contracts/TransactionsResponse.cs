namespace Pagamento.Domain.Contracts
{
    public class TransactionsResponse
    {
        public string reference { get; set; }
        public string tid { get; set; }
        public string nsu { get; set; }
        public string authorizationCode { get; set; }
        public string brandTid { get; set; }
        public DateTime dateTime { get; set; }
        public int amount { get; set; }
        public int installments { get; set; }
        public string cardBin { get; set; }
        public string last4 { get; set; }
        public string returnCode { get; set; }
        public string returnMessage { get; set; }
        public TransactionsResponseLink[] links { get; set; }
    }

    public class TransactionsResponseLink
    {
        public string method { get; set; }
        public string rel { get; set; }
        public string href { get; set; }
    }
}