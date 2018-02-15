using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardSharp
{
    public class Constants
    {
        public const int CardAmount = 10 - 2 /*3456789[10]*/ + 3 /*JQK*/ + 2 /*A2*/ + 2 /*王*/;
        public const int AmountCardMax = 10 - 2 /*3456789[10]*/ + 3 /*JQK*/ + 2 /*A2*/;
        public const int AmountCardNum = 4; /*每种牌4张*/
        public const int MaxPlayer = 3; /*最多3人*/
        
        public class Cards
        {
            public const int C3 = 0;
            public const int C4 = 1;
            public const int C5 = 2;
            public const int C6 = 3;
            public const int C7 = 4;
            public const int C8 = 5;
            public const int C9 = 6;
            public const int C10 = 7;
            public const int CJ = 8;
            public const int CQ = 9;
            public const int CK = 10;
            public const int CA = 11;
            public const int C2 = 12;
            public const int CGhost = 13;
            public const int CKing = 14;
        }
    }
}
