// Ref.: http://www.dotnetperls.com/prime

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeregnPrimTal
{
    public partial class Form1 : Form
    {
        CancellationTokenSource tcs = new CancellationTokenSource();
        CancellationToken ct;
        public Form1()
        {
            InitializeComponent();
            txtBegin.Text = "1";
            txtEnd.Text = "1000000";
            ct = tcs.Token;
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            
            Parameters param = new Parameters();

            param.startNumber = Convert.ToInt32(txtBegin.Text);
            param.endNumber = Convert.ToInt32(txtEnd.Text);
            progressBarPrime.Minimum = param.startNumber;
            progressBarPrime.Maximum = param.endNumber;

            //async and await

            List<int> result = await Task.Run(() => TestPrimeNumbers(param));
            lbPrime.DataSource = result;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            tcs.Cancel();
        }

        List<int> TestPrimeNumbers(Parameters data)
        {
            List<int> Primtal = new List<int>();
            for (int i = data.startNumber; i < data.endNumber; i++)
            {
                ct.ThrowIfCancellationRequested();
                bool prime = IsPrime(i);
                if (prime)
                {
                    Primtal.Add(i);
                }
            }
            return Primtal;
        }

        #region Prime test
        public static bool IsPrime(int candidate)
        {
            // Test whether the parameter is a prime number.
            if ((candidate & 1) == 0)
            {
                if (candidate == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            for (int i = 3; (i * i) <= candidate; i += 2)
            {
                if ((candidate % i) == 0)
                {
                    return false;
                }
            }
            return candidate != 1;
        }
        #endregion
    }
    class Parameters
    {
        public int startNumber { get; set; }

        public int endNumber { get; set; }
    }
}
