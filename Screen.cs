using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1
{
    public class Screen
    {
        public string number { get; private set; }
        public double num { get; set; }
        public long dec = 0;
        public bool isDec = false;

        public Screen(double n = 0)
        {
            this.num = n;
            this.number = n.ToString();
        }

        public void reset()
        {
            num = 0;
            dec = 0;
            isDec = false;
            number = "0";
        }

        public void setNum(double n)
        {
            reset();
            num = n;
            makeNumber();
        }

        private string makeNumber()
        {
            double n = Math.Floor(num), deci = num - n;
            long tg;
            string s = "", ss= "";
            while( n >= 1000 )
            {
                tg = (long)( n % 1000 );
                ss = tg.ToString();
                while (ss.Length < 3)
                    ss = "0" + ss;
                s = "," + ss + s;
                n = (long) ( n/1000 );
                
            }
            if (n >= 1)
                s = ((long)n).ToString() + s;
            if (s.Length == 0)
                s = "0";

            if( s.Length < 24 && deci > 0d )
            {
                ss = ".";
                while( ss.Length + s.Length < 24 && dec < 12 && deci > 0d )
                {
                    if (ss.Length % 4 == 0)
                        ss += ",";
                    deci *= 10;
                    ss = ss + Math.Floor(deci);
                    deci -= Math.Floor(deci);
                    dec++;
                }
            }
            s = s + ss;
            
            number = dec.ToString();
            return number;
        }

        private double toDouble()
        {
            return Double.Parse(number);
        }

        private bool setNumPri( double n )
        {
            if ( n < 1e16)
            {
                num = n;
                makeNumber();
                return true;
            }
            else
                return false;
        }

        public string getNumber() => makeNumber();

        public bool add(long n) => setNumPri(num * 10 + n);
        public bool del()
        {
            if (num == 0d)
                return false;
            if (dec == 0)
                setNum((double) ( (long)num / 10 ) );
            else
                dec--;

            return true;
        }
    }
}
