using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App1
{
    public partial class Calculater : Form
    {
        double ans = 0;
//        double num = 0, ans1 = 0, ans2 = 0; // ans1 for +, - | ans2 for *, /
        char ope = '=';
//        char ope1 = '=', ope2 = '='; // operation | ope1 for +, - | ope2 = *, /
        bool neg = false, pus = false; // negative, pushTheNum
        Screen sc;

        struct ObjCal
        {
//            int type; // 1 = num, 2 = operator
            public double num;
            public char ope;

            public void setNum( double n )
            {
//                type = 1;
                num = n;
                ope = '\n';
            }
            public void setOpe( char o)
            {
//                type = 2;
                ope = o;
                num = 0;
            }
            
        }
        Stack<ObjCal> st = new Stack<ObjCal>();
        ObjCal tg = new ObjCal(); // trung gian

/*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Construct ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
        
        public Calculater()
        {
            InitializeComponent();

            sc = new Screen();
//            tg.setOpe('=');
//            st.Push(tg);

            this.KeyPreview = true;
            this.KeyPress += new KeyPressEventHandler(Calculater_Load);
        }

        const int inNum = 1;
        const int inOpe = 2;
        private void addOpe(char o = '=', bool kt = true)
        {
            if (kt) // Tu dong them ope
                o = ope;
            if( st.Count % 2 == 0 ) // Operation luon o vi ti le
            {
                tg.setOpe(o);
                st.Push(tg);
                ope = '=';
            }
            else
                MessageBox.Show("Wrong addOperation!\n" + st.First().ope + "\n" + st.Count);
        }
        private void addNum(double n = 0d)
        {
            if (n == 0d)
                n = sc.num;
            if (st.Count % 2 == 1) // Number luon o vi ti chan
            {
                tg.setNum(n);
                st.Push(tg);
                sc.reset();
                resetScreen();
            }
            else
            {
                if( st.Any() )
                    MessageBox.Show("Wrong addNumber!\n" + st.First().num + "\n" + st.Count);
                else
                    MessageBox.Show("Wrong addNumber!\n" + "Stack empty" + "\n" + st.Count);
            }
        }
        private void changeInput(int typeInput = inNum) // 1 = Number, 2 = Operator
        {
            /*
            switch( typeInput ) // Kieu nhap
            {
                case inNum:             // Dang nhap so
                    if (pus)            // Ma nhap so
                    {
                        if (!st.Any())
                            addOpe('=');
                        if (st.Count == 1 && ans != 0)
                        {
                            ans = 0;
                            sc.num = 0;
                        }
                    }
                    else                // Ma nhap dau
                    {
                        if (st.Count == 1)
                            addNum(ans);
                        addOpe();
                        pus = true;
                    }
                    break;
                case inOpe:             // Dang nhap dau
                    if (pus)            // Ma nhap so
                    {
                        addNum();
                        sc.num = 0;
                        resetScreen();
                        pus = false;
                    }
                    else                // Ma nhap dau
                    {
                    }
                    break;
            }*/

            if (pus)                    // Dang nhap so
            {
                if(typeInput == inNum)     // Ma nhap so
                {
                    if (!st.Any())                  // st chua co '='
                    {
                        addOpe('=', false);
                        sc.reset();
                    }
                }
                else                        // Ma nhap dau
                {
                    pus = false;
                    if (!st.Any())                  // st chua co '='
                    {
                        addOpe('=', false);
                        addNum(ans);
                    }
                    else                            // nhap bt
                        addNum();
                }
            }
            else                        // Dang nhap dau
            {
                if(typeInput == inNum)
                {
                    pus = true;
                    addOpe();
                }
            }
            console.Text = st.Count.ToString();
        }

        private void Calculater_Load(object sender, KeyPressEventArgs e)
        {
            this.console.Text = "Form.KeyPress: '" + e.KeyChar.ToString() + "' pressed.";
            switch (e.KeyChar)
            {
                case '.':
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    changeInput(inNum);
                    if (sc.add(e.KeyChar - '0'))
                        resetScreen();
                    break;
                case '+':
                case '-':
                case '*':
                case '/':
                    changeInput(inOpe);
                    ope = e.KeyChar;
                    break;
                case '=':
                case (char)13:  // enter
                    changeInput(inOpe);
                    ope = '=';
                    calculate();
                    break;
                case (char)8:   // backspace
                    if (sc.del())
                        resetScreen();
                    break;
                case (char)27:

                    break;
            }
            //            MessageBox.Show("Form.KeyPress: '" + e.KeyChar.ToString() + "' pressed.");
        }
        private void resetCalculater()
        {
            while (st.Any())
                st.Pop();
            ope = '=';
            sc.reset();
            resetScreen();
            pus = true;
        }
        private void resetScreen() => this.screen.Text = sc.getNumber();

        private void result_Click(object sender, EventArgs e)
        {
            changeInput(inOpe);
            ope = '=';
            calculate();
        }


        private void del_Click(object sender, EventArgs e)
        {
            if (st.Any() && sc.del())
                resetScreen();
        }

        private void calculate()
        {
            if (!st.Any())
                return;
            Stack<ObjCal> st2 = new Stack<ObjCal>();
            ObjCal num1, num2, ope1, ope2;
            console.Text = "cal result";
            while ( st.Any() )
            {
                num1 = st.Pop();
                ope1 = st.Pop();
                if( ope1.ope == '\n' || num1.ope != '\n' )
                    MessageBox.Show("Wanning!\n" +
                        "st = "+st.Count+"     st2 = "+st2.Count+"\n"+
                        ( ope1.ope == '\n' )+" || "+(num1.ope != '\n') );
                if (ope1.ope == '=')
                {
                    while (st2.Any())
                    {
                        ope2 = st2.Pop();
                        num2 = st2.Pop();
                        switch (ope2.ope)
                        {
                            case '*':
                                num1.num *= num2.num;
                                break;
                            case '/':
                                if (num2.num == 0)
                                {
                                    while (st.Any())
                                        st.Pop();
                                    while (st2.Any())
                                        st2.Pop();
                                    screen.Text = "Cannot divide by Zero";
                                    return;
                                }
                                num1.num /= num2.num;
                                break;
                            case '+':
                                num1.num += num2.num;
                                break;
                            case '-':
                                num1.num -= num2.num;
                                break;
                        }
                    }
                    st2.Push(num1);
                }
                else if( ope1.ope == '+' || ope1.ope == '-' )
                {
                    while( st2.Any())
                    {
                        ope2 = st2.Pop();
                        num2 = st2.Pop();
                        switch( ope2.ope )
                        {
                            case '*':
                                num1.num *= num2.num;
                                break;
                            case '/':
                                if( num2.num == 0)
                                {
                                    while (st.Any())
                                        st.Pop();
                                    while (st2.Any())
                                        st2.Pop();
                                    screen.Text = "Cannot divide by Zero";
                                    return;
                                }
                                num1.num /= num2.num;
                                break;
                            case '+':
                                num1.num += num2.num;
                                break;
                            case '-':
                                num1.num -= num2.num;
                                break;
                        }
                    }
                    st2.Push(num1);
                    st2.Push(ope1);
                }
                else
                {
                    st2.Push(num1);
                    st2.Push(ope1);
                }
            }
            ans = st2.Pop().num;
            //            tg.setOpe('=');
            //            st.Push(tg);
            //            st.Push(st2.Pop());
            sc.setNum(ans);
            resetScreen();
            pus = true;
            console.Text = "result = "+ans+" size = "+st.Count;
        }

        private void add_Click(object sender, EventArgs e)
        {
            changeInput(inOpe);
            ope = '+';
        }

        private void sub_Click(object sender, EventArgs e)
        {
            changeInput(inOpe);
            ope = '-';
        }

        private void mul_Click(object sender, EventArgs e)
        {
            changeInput(inOpe);
            ope = '*';
        }

        private void div_Click(object sender, EventArgs e)
        {
            changeInput(inOpe);
            ope = '/';
        }

        private void clear_Click(object sender, EventArgs e)
        {
            sc.num = 0d;
            resetScreen();
        }

        private void reset_Click(object sender, EventArgs e)
        {
            resetCalculater();
        }






        /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ Number ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/


        private void n0_Click(object sender, EventArgs e)
        {
            changeInput(inNum);
            if (sc.add(0))
                resetScreen();
        }

        private void n1_Click(object sender, EventArgs e)
        {
            changeInput(inNum);
            if (sc.add(1))
                resetScreen();
        }

        private void n2_Click(object sender, EventArgs e)
        {
            changeInput(inNum);
            if (sc.add(2))
                resetScreen();
        }

        private void n3_Click(object sender, EventArgs e)
        {
            changeInput(inNum);
            if (sc.add(3))
                resetScreen();
        }

        private void n4_Click(object sender, EventArgs e)
        {
            changeInput(inNum);
            if (sc.add(4))
                resetScreen();
        }

        private void n5_Click(object sender, EventArgs e)
        {
            changeInput(inNum);
            if (sc.add(5))
                resetScreen();
        }

        private void n6_Click(object sender, EventArgs e)
        {
            changeInput(inNum);
            if (sc.add(6))
                resetScreen();
        }

        private void n7_Click(object sender, EventArgs e)
        {
            changeInput(inNum);
            if (sc.add(7))
                resetScreen();
        }

        private void n8_Click(object sender, EventArgs e)
        {
            changeInput(inNum);
            if (sc.add(8))
                resetScreen();
        }

        private void n9_Click(object sender, EventArgs e)
        {
            changeInput(inNum);
            if (sc.add(9))
                resetScreen();
        }
    }
}
