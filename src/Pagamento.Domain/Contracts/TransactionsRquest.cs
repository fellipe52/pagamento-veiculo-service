    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Pagamento.Domain.Contracts
    {
        public class TransactionsRquest
        {
            public bool capture { get; set; }
            public string kind { get; set; }
            public string reference { get; set; }
            public int amount { get; set; }
            public int installments { get; set; }
            public string cardholderName { get; set; }
            public string cardNumber { get; set; }
            public int expirationMonth { get; set; }
            public int expirationYear { get; set; }
            public string securityCode { get; set; }
            public string softDescriptor { get; set; }
            public bool subscription { get; set; }
            public int origin { get; set; }
            public int distributorAffiliation { get; set; }
            public string brandTid { get; set; }
            public string storageCard { get; set; }
            public Transactioncredentials transactionCredentials { get; set; }
        }

        public class Transactioncredentials
        {
            public string credentialId { get; set; }
        }
    }