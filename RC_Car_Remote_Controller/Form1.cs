using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows.Input;

namespace RC_Car_Remote_Controller
{
    public partial class Form1 : Form
    {
        ps4controller inputMonitor = new ps4controller();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            inputMonitor.Start();
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            Controller();

        }
        async void Controller()
        {
            Socket socket_send = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress IPadresa_ciel = IPAddress.Parse(textBox1.Text);
            int port_ciel = Convert.ToUInt16(textBox2.Text);
            IPEndPoint KoncovyBod_ciel = new IPEndPoint(IPadresa_ciel, port_ciel);
            label2.Text = "Connected";

            while (true)
            {
                await Task.Delay(40);
                float Forward = inputMonitor.m_RightTrigger;
                float Backward = inputMonitor.m_LeftTrigger;
                float Right = inputMonitor.m_LeftThumbX > 1768 ? (((inputMonitor.m_LeftThumbX-1767)*10)/1215) : 0;
                float Left = inputMonitor.m_LeftThumbX < -1768 ? (((Math.Abs(inputMonitor.m_LeftThumbX) - 1768) * 10) / 1215) : 0;
                Forward = (Keyboard.GetKeyStates(Key.W) & KeyStates.Down) > 0 ? 255 : Forward;
                Backward = (Keyboard.GetKeyStates(Key.S) & KeyStates.Down) > 0 ? 255 : Backward;
                Right = (Keyboard.GetKeyStates(Key.A) & KeyStates.Down) > 0 ? 255 : Right;
                Left = (Keyboard.GetKeyStates(Key.D) & KeyStates.Down) > 0 ? 255 : Left;
                float RightLeft = Right + (Left * -1);
                float ForwardBackward = Forward + (Backward * -1);

                string Data = Convert.ToString(Convert.ToInt16(ForwardBackward)) + "," + 
                              Convert.ToString(Convert.ToInt16(RightLeft)); ;
                label1.Text = Data;

                byte[] buffer = Encoding.UTF8.GetBytes(Data);
                try
                {
                    socket_send.SendTo(buffer, KoncovyBod_ciel);
                }
                catch (Exception ex)
                {
                    label2.Text = "Doslo k chybe";
                }

            }
        }

    }
}
