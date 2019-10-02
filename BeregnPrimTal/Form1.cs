// Ref.: http://www.dotnetperls.com/prime

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BeregnPrimTal
{
    public partial class Form1 : Form
    {
        SynchronizationContext ctx = null;
        Stopwatch stopwatch = new Stopwatch();
        CancellationTokenSource tcs = new CancellationTokenSource();

        CancellationToken ct;
        public Form1()
        {
            InitializeComponent();
            txtBegin.Text = "1";
            txtEnd.Text = "1000000";
            ct = tcs.Token;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            
            Parameters param = new Parameters();
            ctx = SynchronizationContext.Current;
            stopwatch.Start();

            param.startNumber = Convert.ToInt32(txtBegin.Text);
            param.endNumber = Convert.ToInt32(txtEnd.Text);
            progressBarPrime.Minimum = param.startNumber;
            progressBarPrime.Maximum = param.endNumber;

            //Almindelig Thread
            //Thread t = new Thread(TestPrimeNumbers);
            //t.IsBackground = true;
            //t.Start(param);

            //Thread Pool
            //ThreadPool.QueueUserWorkItem(TestPrimeNumbers, param);

            //Task with Factory.StartNew
            //Task t = Task.Factory.StartNew(TestPrimeNumbers, param);

            //Task with start
            //Task t = new Task(TestPrimeNumbers, param);
            //t.Start();

            //Task with run
            //Task.Run(() => TestPrimeNumbers(param,ct));

        }

        void TestPrimeNumbers(object data, CancellationToken ct)
        {
            Parameters param = (Parameters)data;
            ctx.Send(state => this.Text = "Test igang!", null);
            for (int i = param.startNumber; i < param.endNumber; i++)
            {
                ct.ThrowIfCancellationRequested();
                bool prime = IsPrime(i);
                if (prime)
                {
                    ctx.Send(state => lbPrime.Items.Add(i), null);
                }
                ctx.Send(state => progressBarPrime.Value = i, null);
            }
            ctx.Send(state => this.Text = "Test færdig!", null);

            stopwatch.Stop();
            ctx.Send(state => label3.Text = Convert.ToString(stopwatch.ElapsedMilliseconds), null);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            tcs.Cancel();
        }
    }
    class Parameters
    {
        public int startNumber { get; set; }

        public int endNumber { get; set; }
    }
}
